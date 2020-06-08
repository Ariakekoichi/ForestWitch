using UnityEngine;
using System.Collections;

public class FinalBossCtrl : MonoBehaviour
{

    public enum MonsterState { idle, trace, attack, die, modori };// モンスターの状態情報のある Enumerable 変数宣言
    public MonsterState monsterState = MonsterState.idle;// モンスターの現在状態の情報を入れるEnum変数
                                             
    private Transform monsterTr;
    private Transform playerTr;
    private Transform ModoruPoint;
    private NavMeshAgent nvAgent;
    public GameObject FireObject;//銃弾発車位置の変数
    private GameUI gameUI;//GameUIに接近するための変数

    private float traceDist = 25.0f;//この距離以内なら相手を追跡する
    private float attackDist = 16.25f;//この距離以内なら相手を攻撃する
    
    public bool isDie = false;//モンスターの生死
    public GameObject bloodEffect;
    public GameObject DestroyEffect;
    public GameObject FinalBossHPBar;
    public int hp = 1000;
    public int initHp = 1000;
    public float Speed = 10.0f;
    public float rotSpeed = 10.0f;
    public GameObject Target;//追跡対象（プレイヤー）
    public GameObject Damage;//PlayerCtrlからダメージ値を持ってくるための設定
    int lookat = 1;//モンスターの視線に関する変数
    public GameObject Monster;//モンスター自身のY座標を見るための変数。（これがないとモンスターが変なところを見るようになる）
    [System.Serializable]
    public class Anim
    {
        public AnimationClip idle;
        public AnimationClip die;
    }
    public int SuperFire = 0;
    public Anim anim;
    public Animation _animation;

    public int fire = 0;
	private int HPkaifukuryoku = 5;//HP回復力
    public float HPkaifukutime = 0;//HP回復タイミング
    MonsterState newState;

    void Update()
    {
		if(hp != initHp)
		{
			if (HPkaifukutime >= 0 && HPkaifukutime < 1)
			{
				HPkaifukutime += Time.deltaTime;
			}
			while (HPkaifukutime >= 1)
			{
				if (hp < initHp)
				{
					hp += HPkaifukuryoku;
				}
				HPkaifukutime = 0;
			}
		}
        Vector3 LookPos = Target.transform.position;
        LookPos.y = Monster.transform.position.y;

        if (lookat == 1)//モンスターがプレイヤーを見るようにする
        {
            transform.LookAt(LookPos);
        }
        
    }
    
    void Awake()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Target = playerTr.gameObject;
        ModoruPoint = GameObject.FindWithTag("ModoruPoint").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        Damage = GameObject.Find("Player");//Damageの値を"Player"から持ってくる!

    }
    void OnEnable()//イベント発生時遂行する関数連結
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
    }
    void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
        StopAllCoroutines();
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
           
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)
            {
                
                monsterState = MonsterState.attack;
                yield return new WaitForSeconds(0.8f);
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;
                if (Damage.GetComponent<PlayerCtrl>().BossKanchi == 0)
                {
                    monsterState = MonsterState.modori;
                    SuperFire = 0;
                }
            }
            else
            {
                if (Damage.GetComponent<PlayerCtrl>().BossKanchi == 1)
                {
                    monsterState = MonsterState.idle;
                }
                if (Damage.GetComponent<PlayerCtrl>().BossKanchi == 0)
                {
                    monsterState = MonsterState.modori;
                    SuperFire = 0;
                }
            }
        }
    }
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
                case MonsterState.idle:
                    fire = 0;
                    nvAgent.Stop();
                    _animation.CrossFade(anim.idle.name, 0.3f);
                    break;
                case MonsterState.modori:
                    fire = 0;
                    Vector3 dir1 = ModoruPoint.transform.position - transform.position;
                    transform.position += (dir1 * Time.deltaTime * 0.5f);
                    _animation.CrossFade(anim.idle.name, 0.3f);
                    break;
                case MonsterState.trace:
                    fire = 0;
                    Vector3 dir = Target.transform.position - transform.position;
                    dir.Normalize();
                    float distance = Vector3.Distance(Target.transform.position, transform.position);
                    _animation.CrossFade(anim.idle.name, 0.3f);
                    CharacterAttribute targetAttribute = Target.GetComponent<CharacterAttribute>();
                    CharacterAttribute myAttribute = gameObject.GetComponent<CharacterAttribute>();
                    if (distance > targetAttribute.Radius + myAttribute.Radius)
                    {

                        transform.position += (dir * Speed * Time.deltaTime);
                    }
                    nvAgent.destination = playerTr.position;
                    
                   
                    
                    break;
                    
                case MonsterState.attack:
                   
                    nvAgent.Stop();
                    _animation.CrossFade(anim.idle.name, 0.3f);
                    fire = 1;
                    StartCoroutine("TransStateToMove", 0);
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
            if (Damage.GetComponent<PlayerCtrl>().FinalBoss == 0)
            {
                hp -= Damage.GetComponent<PlayerCtrl>().damage;
                if (hp <= 0)
                {
                    MonsterDie();
                }
            }
            Destroy(coll.gameObject);
           
        }
        if (coll.gameObject.tag == "FIREBALL")
        {
            
            if (Damage.GetComponent<PlayerCtrl>().FinalBoss == 0)
            {
                hp -= Damage.GetComponent<PlayerCtrl>().damage;
                if (hp <= 0)
                {
                    MonsterDie();
                }
            }
            Destroy(coll.gameObject);
        }

    }

    void MonsterDie()//モンスター死亡時の処理過程
    {
        FinalBossHPBar.SetActive(false);
        StopAllCoroutines();//すべてのコルーチン停止
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.Stop();
        _animation.CrossFade(anim.die.name, 0.5f);
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameUI.DispScore(0);
        Speed = 0.0f;
        rotSpeed = 0.0f;
        lookat = 0;
        FireObject.SetActive(false);
        StartCoroutine(this.PushObjectPool());
        
    }
    IEnumerator PushObjectPool()
    {
		yield return new WaitForSeconds(0.0f);
        Damage.GetComponent<PlayerCtrl>().FinalBoss = 1;//FinalBossを倒したことにして、勝利条件を満たせる
        isDie = false;
        hp = initHp;
        monsterState = MonsterState.idle;
        Speed = 2.0f;
        lookat = 1;
        rotSpeed = 10.0f;
        FireObject.SetActive(true);
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = true;
        }
    }
    IEnumerator CreateBloodEffect(Vector3 pos)
    {
        GameObject _blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(_blood1, 2.0f);

        yield return null;
    }
    IEnumerator TransStateToMove()
    {
        yield return new WaitForSeconds(0.005f);
        fire = 0;
        monsterState =MonsterState.idle;
        fire = 0;
        SuperFire += 1;

    }
    void OnPlayerDie()
    {
        StopAllCoroutines();
        nvAgent.Stop();
       
        Speed = 0.0f;
        fire = 0;
    }
    IEnumerator CreateDestroyEffect(Vector3 pos)
    {
        GameObject _blood1 = (GameObject)Instantiate(DestroyEffect, pos, Quaternion.identity);
        Destroy(_blood1, 2.0f);

        yield return null;
    }

}
