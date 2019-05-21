﻿using UnityEngine;
using System.Collections;

public class ChukanBossCtrl : MonoBehaviour
{

    public enum MonsterState { idle, trace, attack, die, modori };// 몬스터의 상태 정보가 있는 Enumerable 변수 선언
    public MonsterState monsterState = MonsterState.idle;// 몬스터의 현재 상태 정보를 저장할 Enum변수
    //속도 향상을 위해 각종 컴포넌트를 변수에 할당                                                    
    private Transform monsterTr;
    private Transform playerTr;
    private Transform ModoruPoint;
    private NavMeshAgent nvAgent;
  
    public GameObject FireObject;//총알발사위치를 불러오기위한 변수
    public GameObject PlayerBloodEffect;
    private GameUI gameUI;//GameUI에 접근하기 위한 변수

    private float traceDist = 23.0f;//추적 사정거리
    private float attackDist = 1.5f;//공격 사정 거리
    

    //몬스터의 사망 여부
    public bool isDie = false;
    //혈흔효과 프리팹
    public GameObject bloodEffect;
    //public GameObject bloodDecal;
    public GameObject ChukanBossHPBar;//ChukanBossHPBar를 없애기위함으로!
    public GameObject DragonHPBar = null;//처음에는 드래곤의 피통이 보이지않게
    public GameObject Dragon = null;//처음에는 드래곤이 보이지않게
    public GameObject DragonSyutsugen = null;//드래곤 출몰 메세지가 처음에 보이지않게
    public int hp = 1000;
    public int initHp = 1000;
    public float Speed = 5.0f;
    public float rotSpeed = 10.0f;//어디선가쓰엿던거같은데..
    public GameObject Target;//추적대상
    public GameObject Damage;//PlayerCtrl에서 데미지를 가져오기위한 설정
    int lookat = 1;//몬스터가 쳐다보다가 안쳐다보게하기위한변수
    public Anim anim;
    public Animation _animation;
    public GameObject Monster;//몹 자기자신의 Y자표를 쳐다보게하기위한 변수
    public int fire = 0;//방아쇠
    [System.Serializable]
    public class Anim
    {
        public AnimationClip idle;
        public AnimationClip walk;
        public AnimationClip attack;
        public AnimationClip die;
    }
    MonsterState newState;

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
        //this.gameObject
        //몬스터의 트랜스폼 할당
        monsterTr = GetComponent<Transform>();
        //추적 대상인 플레이어의 트랜스폼 할당
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Target = playerTr.gameObject;
        //NavMeshAgent 컴포넌트 할당
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        // Animator 컴포넌트 할당
        // Animator 컴포넌트 할당
        _animation = this.gameObject.GetComponentInChildren<Animation>();
        _animation.clip = anim.idle;
        _animation.Play();
        ModoruPoint = GameObject.Find("ChukanBossPoint").GetComponent<Transform>();
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
           
            yield return new WaitForSeconds(0.3f);
            
            //몬스터와 플레리어 사이의 거리 측정
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)
            {//공격거리범위 이내로 들어왔는지 확인
                if (dist <= 1.5f)
                {
                    Debug.Log(4444);
                    Damage.GetComponent<PlayerCtrl>().hp -= 20;
                    // StartCoroutine(this.CreatePlayerBloodEffect(playerTr.transform.position));
                    StartCoroutine(this.CreatePlayerBloodEffect(playerTr.transform.position));//★y축을 0.6을올려서 해보고싶다!
                    //Instantiate(PlayerBloodEffect, playerTr.position, Quaternion.identity);
                    
                }
                if (monsterState != MonsterState.attack)
                {
                    monsterState = MonsterState.attack;

                }
                yield return new WaitForSeconds(0.7f);
            }
            else if (dist <= traceDist)
            {//추적거리 범위 이내로 들어왔는지 확인
                monsterState = MonsterState.trace;
                if (Damage.GetComponent<PlayerCtrl>().BossKanchi == 0)//땅에떨어졋을때 본위치로 돌아가게한다
                {
                    monsterState = MonsterState.modori;
                }
            }
            else
            {
                if (Damage.GetComponent<PlayerCtrl>().BossKanchi == 1)//땅이 아닐때는 아무것도 안하게
                {
                    monsterState = MonsterState.idle;
                }
                if (Damage.GetComponent<PlayerCtrl>().BossKanchi == 0)//땅에떨어졋을때 본위치로 돌아가게한다
                {
                    monsterState = MonsterState.modori;
                }
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
                    fire = 0;
                    //추적 중지
                    nvAgent.Stop();
                    //Animator의 IsAttack 변수를 false로 설정
                    _animation.CrossFade(anim.idle.name, 0.3f);

                    break;
                case MonsterState.modori:
                    fire = 0;
                    Vector3 dir1 = ModoruPoint.transform.position - transform.position;
                    transform.position += (dir1 * Time.deltaTime * 0.5f);
                    // nvAgent.destination = ModoruPoint.position;
                    _animation.CrossFade(anim.walk.name, 0.3f);

                    break;

                //추적 상태
                case MonsterState.trace:
                    fire = 0;
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


                    break;

                //공격 상태
                case MonsterState.attack:
                    //추적 중지
                    
                    nvAgent.Stop();
                    _animation.CrossFade(anim.attack.name, 0.2f);

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
        if (coll.gameObject.tag == "TurretBullet")//터렛의 총알에 맞앗을때
        {
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));
            hp -= 2;//플레이어의 총알에 맞았을때의 피감소
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
        ChukanBossHPBar.SetActive(false);
        Dragon.SetActive(true);//Dragon이 나타난다
        DragonHPBar.SetActive(true);//FinalBoss의 피통이 나타난다후
        DragonSyutsugen.SetActive(true);
        Destroy(DragonSyutsugen, 7.0f);//10초후에 지운다
        gameUI.DispScore(500);//경험치 부여
        gameUI.GetComponent<GameUI>().HPpotion += 1;//플레이어 HP포션 획득
        gameUI.GetComponent<GameUI>().MPpotion += 1;//플레이어 MP포션 획득
        StopAllCoroutines();//모든 코루틴 정지
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.Stop();
        _animation.CrossFade(anim.die.name, 0.5f);
        //몬스터에 추가된 콜라이더를 비활성화
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;


        //foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())//아마 솔져일경우에는 이거필요없다
        // {
        // coll.enabled = false;
        // }


        Speed = 0.0f;//움직이지 못하게!
        rotSpeed = 0.0f;//회전하지 못하게
        lookat = 0;//시체가 안쳐다보게!
        FireObject.SetActive(false);//총을 못쏘게!
        StartCoroutine(this.PushObjectPool());//몬스터 오브젝트 풀로 환원시키는 코루틴 함수 호출
        
    }
    //몬스터를 오브젝트 풀 환원 루틴
    IEnumerator PushObjectPool()
    {
        yield return new WaitForSeconds(3.0f);//3초후 시체치움
        //각종변수 초기화
        isDie = false;
        hp = initHp;
        monsterState = MonsterState.idle;
        Speed = 2.0f;
        lookat = 1;
        rotSpeed = 10.0f;
        FireObject.SetActive(true);//총을 쏠수있게!
        //몬스터에 추가된 Collider을 다시 활성화
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = true;
        }

       // gameObject.SetActive(false);//몬스터를 비활성화
        Destroy(gameObject, 0.0f);
    }
    IEnumerator CreateBloodEffect(Vector3 pos)
    {
        //혈흔 효과 생성
        GameObject _blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(_blood1, 2.0f);

        yield return null;
    }
    IEnumerator TransStateToMove()
    {
        
        yield return new WaitForSeconds(0.005f);
        
        monsterState =MonsterState.idle;//★잠깐이리해본거!
        fire = 0;//다시한번 공격안하게 다시확인
        _animation.CrossFade(anim.attack.name, 0.5f);

    }
    void OnPlayerDie()//플레이어가 사망햇을때 실행되는 함수
    {
        StopAllCoroutines();//몬스터의 상태를 체크하는 코루틴함수를 모두 정지시킴
        nvAgent.Stop();//추적을 정지하고 애니메이션을 수행
        _animation.CrossFade(anim.idle.name,0.3f);
        Speed = 0.0f;//움직이지않게함
        fire = 0;
    }
    IEnumerator CreatePlayerBloodEffect(Vector3 pos)
    {
        //혈흔 효과 생성
        GameObject _blood2 = (GameObject)Instantiate(PlayerBloodEffect, pos += new Vector3(0, Random.Range(1, 1.5f), 0), Quaternion.identity);
        Destroy(_blood2, 2.0f);

        yield return null;
    }

}