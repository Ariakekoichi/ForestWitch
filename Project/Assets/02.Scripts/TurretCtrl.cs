using UnityEngine;
using System.Collections;

public class TurretCtrl : MonoBehaviour
{

    public enum MonsterState { idle, trace, attack, die };// 몬스터의 상태 정보가 있는 Enumerable 변수 선언
    public MonsterState monsterState = MonsterState.idle;// 몬스터의 현재 상태 정보를 저장할 Enum변수
    //속도 향상을 위해 각종 컴포넌트를 변수에 할당                                                    
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    public GameObject TurretCtrl1;

    public GameObject FireObject;//총알발사위치를 불러오기위한 변수
    private GameUI gameUI;//GameUI에 접근하기 위한 변수

  //  private float traceDist = 30.0f;//추적 사정거리
    private float attackDist = 16.25f;//공격 사정 거리
    private float ftime = 0.0f;//공격할수있는 타이밍

    //몬스터의 사망 여부
    public bool isDie = false;
    //혈흔효과 프리팹
    public GameObject bloodEffect;
    public GameObject DestroyEffect;
    public int hp = 200;
    public int initHp = 200;
    public float Speed = 0.0f;//Turret이므로 움직일필요가읎다
    //public float rotSpeed = 10.0f;//어디선가쓰엿던거같은데..
    public GameObject Target;//추적대상
    public GameObject Damage;//PlayerCtrl에서 데미지를 가져오기위한 설정
    public GameObject Monster;//몹 자기자신의 Y자표를 쳐다보게하기위한 변수
    int lookat = 1;//몬스터가 쳐다보다가 안쳐다보게하기위한변수
    public int fire = 0;//방아쇠
    MonsterState newState;
    public GameObject Turret;

    void start()
    {
        TurretCtrl1 = GameObject.Find("Turret");
    }
    void Update()
    {
        Vector3 LookPos = Target.transform.position;
        LookPos.y = Monster.transform.position.y;
        ftime += Time.deltaTime;
        if (lookat == 1)//몬스터가 쳐다보도록하기
        {
            transform.LookAt(LookPos);//몬스터가 플레이어(타겟)를 쳐다보도록한다
        }

    }

    void Awake()
    {
        
        //몬스터의 트랜스폼 할당
        monsterTr = GetComponent<Transform>();
        //추적 대상인 플레이어의 트랜스폼 할당
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Target = playerTr.gameObject;
        //NavMeshAgent 컴포넌트 할당
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        // Animator 컴포넌트 할당
        
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();//GameUI 게임오브젝트의 GameUI 스크립트를 할당
                                                                  //  nvAgent.destination = playerTr.position;//추적대상의 위치를 설정하면 바로추적시작

        Damage = GameObject.Find("Player");//Damage가져오기!
    }
    void OnEnable()//이벤트 발생시 수행할 함수 연결
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
        //일정한 간격으로 몬스터의 행동 상태를 체크하는 코루틴 함수 실행
        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());//몬스터의 상태에 따라 동작하는 루틴을 실행하는 코루틴 함수
    }
    void OnDisable()//이벤트 발생시 연결된 함수 해제
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
        StopAllCoroutines();//코루틴 함수의 완벽한 해제를 위해 한 번 더 정지시킨다.
    }

    IEnumerator CheckMonsterState()//일정한 간격으로 몬스터의 행동 상태를 체크하고 몬스터스테이트 값 변경
    {
        while (!isDie)
        {
            
            yield return new WaitForSeconds(0.2f);
            //몬스터와 플레이어 사이의 거리 측정
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist && ftime > 2.0f && Damage.GetComponent<PlayerCtrl>().Kanchi == 1)
            {//공격거리범위 이내로 들어왔는지 확인
                
                monsterState = MonsterState.attack;
            
            }
            
            else
            {
                monsterState = MonsterState.idle;
                
            }
            
            fire = 0;
            // yield return new WaitForSeconds(0.8f);//0.8초간 가만잇기
        }
    }


    //몬스터의 상태값에 따라 적정한 동작을 수행하는 함수

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            if (hp <= 0)
            {
                MonsterDie();
            }
            switch (monsterState)
            {
                //idle 상태
                case MonsterState.idle:
                    //추적 중지
                    fire = 0;
                    nvAgent.Stop();//★안끌리게하려고 잠깐 멈춰놓기!
                    //Animator의 IsAttack 변수를 false로 설정
                    
                    
                    break;

                //추적 상태
                case MonsterState.trace:
                    fire = 0;
                    Vector3 dir = Target.transform.position - transform.position;
                    dir.Normalize();
                   // float distance = Vector3.Distance(Target.transform.position, transform.position);

                   // CharacterAttribute targetAttribute = Target.GetComponent<CharacterAttribute>();
                   // CharacterAttribute myAttribute = gameObject.GetComponent<CharacterAttribute>();
                    

                    //추적 대상의 위치를 넘겨줌

                    nvAgent.destination = playerTr.position;
                   

                    
                    break;

                //공격 상태
                case MonsterState.attack:
                    //추적 중지
                    nvAgent.Stop();//★안끌리게하려고 잠깐 멈춰놓기!
                    //IsAttack을 true로 설정해 attack State로 전
                    fire = 1;
                    ftime = 0;
                    // StartCoroutine("TransStateToMove", 0);
                    //yield return new WaitForSeconds(1);//1초후 상태변경
                    break;

            }
            yield return null;

        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "BULLET")//플레이어의 총알에 맞앗을때
        {
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));
            hp -= Damage.GetComponent<PlayerCtrl>().damage;//플레이어의 총알에 맞았을때의 피감소
            if (hp <= 0)
            {
                MonsterDie();
            }
            //Bullet 삭제
            Destroy(coll.gameObject);
            //IsHit Trigger를 발생시키면 Any State에서 gothit로 전이됨
           
        }
        if (coll.gameObject.tag == "FIREBALL")//플레이어의 파이어볼에 맞앗을때
        {
            
            hp -= Damage.GetComponent<PlayerCtrl>().damage;//플레이어의 총알에 맞았을때의 피감소
            if (hp <= 0)
            {
                MonsterDie();
            }
            //Bullet 삭제
            Destroy(coll.gameObject);
            //IsHit Trigger를 발생시키면 Any State에서 gothit로 전이됨
            
        }
    }

    void MonsterDie()//몬스터 사망시 처리과정들
    {

        StopAllCoroutines();//모든 코루틴 정지
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.Stop();//★안끌리게하려고 잠깐 멈춰놓기!

        //몬스터에 추가된 콜라이더를 비활성화
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        
        gameUI.DispScore(200);//경험치 부여
        StartCoroutine(CreateDestroyEffect(Turret.transform.position));//터렛이 터지는 이펙트!
        Speed = 0.0f;//움직이지 못하게!
        //rotSpeed = 0.0f;//회전하지 못하게
        lookat = 0;//시체가 안쳐다보게!
        FireObject.SetActive(false);//총을 못쏘게!
        StartCoroutine(this.PushObjectPool());//몬스터 오브젝트 풀로 환원시키는 코루틴 함수 호출

    }
    //몬스터를 오브젝트 풀 환원 루틴
    IEnumerator PushObjectPool()
    {
        yield return new WaitForSeconds(0.1f);//파괴된후 0.1초후에 사라짐
        //각종변수 초기화
        isDie = false;
        hp = initHp;
        monsterState = MonsterState.idle;
        Speed = 0.0f;
        lookat = 1;
        //rotSpeed = 10.0f;
        FireObject.SetActive(true);//총을 쏠수있게!
        //몬스터에 추가된 Collider을 다시 활성화
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = true;

        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = true;
        }

        gameObject.SetActive(false);//몬스터를 비활성화
    }
    IEnumerator CreateBloodEffect(Vector3 pos)
    {
        //혈흔 효과 생성
        GameObject _blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(_blood1, 2.0f);

        yield return null;
    }
    IEnumerator CreateDestroyEffect(Vector3 pos)
    {
        //혈흔 효과 생성
        GameObject _blood1 = (GameObject)Instantiate(DestroyEffect, pos, Quaternion.identity);
        Destroy(_blood1, 2.0f);

        yield return null;
    }
    IEnumerator TransStateToMove()
    {
        
        yield return new WaitForSeconds(0.5f);
       
        fire = 0;//안발사되도록 다시한번 확인
    }
    void OnPlayerDie()//플레이어가 사망햇을때 실행되는 함수
    {
        StopAllCoroutines();//몬스터의 상태를 체크하는 코루틴함수를 모두 정지시킴
        nvAgent.Stop();//★안끌리게하려고 잠깐 멈춰놓기!//추적을 정지하고 애니메이션을 수행

        Speed = 0.0f;//움직이지않게함
        fire = 0;
    }
    


}
