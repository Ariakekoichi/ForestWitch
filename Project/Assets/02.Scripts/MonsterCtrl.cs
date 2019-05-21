using UnityEngine;
using System.Collections;

public class MonsterCtrl : MonoBehaviour
{

    public enum MonsterState { idle, trace, attack, die };// 몬스터의 상태 정보가 있는 Enumerable 변수 선언
    public MonsterState monsterState = MonsterState.idle;// 몬스터의 현재 상태 정보를 저장할 Enum변수
    //속도 향상을 위해 각종 컴포넌트를 변수에 할당                                                    
    public Transform monsterTr;
    private Transform playerTr;
    //private Transform Bakuhatsu;
    public NavMeshAgent nvAgent;
    private GameUI gameUI;//GameUI에 접근하기 위한 변수

    private float traceDist = 25.0f;//추적 사정거리
    private float attackDist = 1.5f;//공격 사정 거리

    //몬스터의 사망 여부
    public bool isDie = false;
    //혈흔효과 프리팹
    public GameObject bloodEffect;
    public GameObject PlayerBloodEffect;
    //public GameObject bloodDecal;
    public int hp = 20;
    public int initHp = 20;
    public int attackdamage = 5;
    private float Speed = 4.0f;
    //private float rotSpeed = 10.0f;//어디선가쓰엿던거같은데..
    public GameObject Target;//추적대상
    public GameObject Damage;//PlayerCtrl에서 데미지를 가져오기위한 설정
    public GameObject Monster;//몹 자기자신의 Y자표를 쳐다보게하기위한 변수
    [System.Serializable]
    public class Anim
    {
        public AnimationClip idle;
        public AnimationClip walk;
        public AnimationClip attack;
        public AnimationClip die;
    }
    int lookat = 1;//몬스터가 쳐다보다가 안쳐다보게하기위한변수
    public Anim anim;
    public Animation _animation;

    void Update()
    {

        Vector3 LookPos = Target.transform.position;
        LookPos.y = Monster.transform.position.y;
        
        if (lookat == 1)//몬스터가 쳐다보도록하기
        {
            transform.LookAt(LookPos);//몬스터가 플레이어(타겟)를 쳐다보도록한다
        }
        

    }

    void Awake()
    {
        //this.gameObject;
        //몬스터의 트랜스폼 할당
        monsterTr = GetComponent<Transform>();
        //추적 대상인 플레이어의 트랜스폼 할당
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Target = playerTr.gameObject;
       // Vector3 Bakuhatsu = playerTr.transform.position;
        //Bakuhatsu.y = playerTr.transform.position.y + 0.6f;
        //NavMeshAgent 컴포넌트 할당
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        // Animator 컴포넌트 할당
        _animation = this.gameObject.GetComponentInChildren<Animation>();
        _animation.clip = anim.idle;
        _animation.Play();
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();//GameUI 게임오브젝트의 GameUI 스크립트를 할당
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
            //몬스터와 플레리어 사이의 거리 측정

            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)//공격거리범위 이내로 들어왔는지 확인
            {
                if (dist <= 1.5f)
                {
                    Debug.Log(2222);
                    Damage.GetComponent<PlayerCtrl>().hp -= 5;
                    StartCoroutine(this.CreatePlayerBloodEffect(playerTr.transform.position));//★y축을 0.6을올려서 해보고싶다!
                }
                if (monsterState != MonsterState.attack) { 
                    monsterState = MonsterState.attack;
                }
                yield return new WaitForSeconds(0.8f);
            }
            else if (dist <= traceDist)
            {//추적거리 범위 이내로 들어왔는지 확인
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
            }

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
                    nvAgent.Stop();
                    //Animator의 IsAttack 변수를 false로 설정
                    _animation.CrossFade(anim.idle.name,0.3f);
                    foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
                    {
                        coll.enabled = false;
                    }
                    break;

                //추적 상태
                case MonsterState.trace:
                    Vector3 dir = Target.transform.position - transform.position;
                    dir.Normalize();
                    float distance = Vector3.Distance(Target.transform.position, transform.position);

                    CharacterAttribute targetAttribute = Target.GetComponent<CharacterAttribute>();
                    CharacterAttribute myAttribute = gameObject.GetComponent<CharacterAttribute>();
                    if (distance > targetAttribute.Radius + myAttribute.Radius)
                    {

                        transform.position += (dir * Speed * Time.deltaTime);//슬라임 입장에서 대상과 나의 몸통의 반지름 정보를 얻어온다.
                                                                             //그후에 대상과 나의거리를 체크하여,거리가 대상과 나의 반지름의 합보다 크면 이동하게되어있다.
                    }
                    //추적 대상의 위치를 넘겨줌

                    nvAgent.destination = playerTr.position;
                    _animation.CrossFade(anim.walk.name, 0.3f);
                    foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
                    {
                        coll.enabled = false;
                    }
                    break;

                //공격 상태
                case MonsterState.attack:
                    //추적 중지
                    nvAgent.Stop();
                    //IsAttack을 true로 설정해 attack State로 전
                    _animation.CrossFade(anim.attack.name, 0.3f);
                  
                    break;

            }
            yield return null;
        }
    }
    void OnCollisionEnter(Collision coll)
    {

        if (coll.gameObject.tag == "BULLET")
        {
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));
            hp -= Damage.GetComponent<PlayerCtrl>().damage;
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
        nvAgent.Stop();
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        _animation.CrossFade(anim.die.name, 0.5f);
        //몬스터에 추가된 콜라이더를 비활성화
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
        gameUI.DispScore(30);//경험치 부여

        Speed = 0.0f;//움직이지 못하게!
                     // rotSpeed = 0.0f;//회전하지 못하게
        lookat = 0;//시체가 안쳐다보게!
        StartCoroutine(this.PushObjectPool());//몬스터 오브젝트 풀로 환원시키는 코루틴 함수 호출
    }
    //몬스터를 오브젝트 풀 환원 루틴
    IEnumerator PushObjectPool()
    {
        yield return new WaitForSeconds(1.0f);//1.0초후 시체치움
        //각종변수 초기화
        isDie = false;
        hp = initHp;
        monsterState = MonsterState.idle;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        Speed = 4.0f;
        lookat = 1;
        //rotSpeed = 10.0f;
        //몬스터에 추가된 Collider을 다시 활성화
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

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
        //emitter.Emit();
        //_blood1.GetComponent<Particle>().position += Vector3.up;
        Destroy(_blood1, 2.0f);

        yield return null;
    }
    IEnumerator TransStateToMove()
    {
        yield return new WaitForSeconds(0.2f);

        //    monsterState = MonsterState.idle;
    }

    void OnPlayerDie()//플레이어가 사망햇을때 실행되는 함수
    {
        monsterState = MonsterState.idle;
        StopAllCoroutines();//몬스터의 상태를 체크하는 코루틴함수를 모두 정지시킴
        nvAgent.Stop();//추적을 정지하고 애니메이션을 수행
        _animation.CrossFade(anim.idle.name, 0.3f);
        Speed = 0.0f;//움직이지않게함
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
    
    }

    public void ColliderTrue()
    {
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = true;
        }
    }
    IEnumerator CreatePlayerBloodEffect(Vector3 pos)
    {
        //혈흔 효과 생성
        GameObject _blood2 = (GameObject)Instantiate(PlayerBloodEffect, pos += new Vector3(0, Random.Range(1, 1.5f),0),Quaternion.identity);
        Destroy(_blood2, 2.0f);

        yield return null;
    }

}