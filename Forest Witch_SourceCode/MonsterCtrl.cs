using UnityEngine;
using System.Collections;

public class MonsterCtrl : MonoBehaviour
{

    public enum MonsterState { idle, trace, attack, die };// モンスターの状態情報のある Enumerable 変数宣言
    public MonsterState monsterState = MonsterState.idle;// モンスターの現在状態の情報を入れるEnum変数  

    public Transform monsterTr;
    private Transform playerTr;
    
    public NavMeshAgent nvAgent;
    private GameUI gameUI;//GameUIを参照するための変数

    private float traceDist = 25.0f;//この距離以内なら相手を追跡する
    private float attackDist = 1.5f;//この距離以内なら相手を攻撃する←近接攻撃モンスターなので短く


    public bool isDie = false;//モンスターの生死
    public GameObject bloodEffect;//血痕効果
    public GameObject PlayerBloodEffect;
    public int hp = 20;//MonsterのHP
    public int initHp = 20;//Monsterの最大HP
    public int attackdamage = 5;
    private float Speed = 4.0f;
    public GameObject Target;
    public GameObject Damage;
    public GameObject Monster;
    [System.Serializable]
    public class Anim
    {
        public AnimationClip idle;
        public AnimationClip walk;
        public AnimationClip attack;
        public AnimationClip die;
    }
    int lookat = 1;//モンスターの視線に関する変数
    public Anim anim;
    public Animation _animation;

    void Update()
    {

        Vector3 LookPos = Target.transform.position;
        LookPos.y = Monster.transform.position.y;//モンスター自分のY座標を見る。これがないとPLAYERのY座標を見てしまうため、モンスターの動きが雑になる。

        if (lookat == 1)
        {
            transform.LookAt(LookPos);//モンスターがプレイヤーを見るようにする
        }
        

    }

    void Awake()
    {
        monsterTr = GetComponent<Transform>();//モンスターのTransform割当
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();//追跡対象のプレイヤーのTransform割当
        Target = playerTr.gameObject;
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();//NavMeshAgent コンポーネント割当
        _animation = this.gameObject.GetComponentInChildren<Animation>();
        _animation.clip = anim.idle;
        _animation.Play();
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();//GameUI オブジェクトの GameUI スクリプトを割当
        Damage = GameObject.Find("Player");//Damageの値を"Player"から持ってくる

    }
    void OnEnable()//イベント発生時遂行する関数連結
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
        StartCoroutine(this.CheckMonsterState());//一定の間隔でモンスターの行動の状態をチェックするコルーチン関数実行
        StartCoroutine(this.MonsterAction());//モンスターの状態によって動作するルーチンを実行するコルーチン関数
    }
    void OnDisable()//イベント発生時連結した関数解除
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
        StopAllCoroutines();//コルーチン関数の完璧な解除のため入れてある停止コード
    }

    IEnumerator CheckMonsterState()//一定な間隔でモンスターの行動状態チェックしモンスターステート値変更
    {
        while (!isDie)
        {

            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)
            {
                if (dist <= 1.5f)//1.5f範囲以内にプレイヤーがいると、ダメージ５を受ける
                {
                    Debug.Log(2222);
                    Damage.GetComponent<PlayerCtrl>().hp -= 5;
                    StartCoroutine(this.CreatePlayerBloodEffect(playerTr.transform.position));
                }
                if (monsterState != MonsterState.attack) { 
                    monsterState = MonsterState.attack;
                }
                yield return new WaitForSeconds(0.8f);//アタックタイミング、これがないとひたすらプレイヤーのHPが下がる
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
            }

        }
    }
    
    IEnumerator MonsterAction()//TURRETの状態によって適切な動作を遂行する関数
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
                    
                    nvAgent.Stop();
                   
                    _animation.CrossFade(anim.idle.name,0.3f);
                    foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
                    {
                        coll.enabled = false;
                    }
                    break;
                case MonsterState.trace:
                    Vector3 dir = Target.transform.position - transform.position;
                    dir.Normalize();
                    float distance = Vector3.Distance(Target.transform.position, transform.position);

                    CharacterAttribute targetAttribute = Target.GetComponent<CharacterAttribute>();
                    CharacterAttribute myAttribute = gameObject.GetComponent<CharacterAttribute>();
                    if (distance > targetAttribute.Radius + myAttribute.Radius)
                    {

                        transform.position += (dir * Speed * Time.deltaTime);
                    }
                    nvAgent.destination = playerTr.position;
                    _animation.CrossFade(anim.walk.name, 0.3f);
                    foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
                    {
                        coll.enabled = false;
                    }
                    break;
                case MonsterState.attack:
                    nvAgent.Stop();
                    _animation.CrossFade(anim.attack.name, 0.3f);
                  
                    break;

            }
            yield return null;
        }
    }
    void OnCollisionEnter(Collision coll)
    {

        if (coll.gameObject.tag == "BULLET")//中間ボスがプレイヤーの銃弾に当たった時
        {
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));
            hp -= Damage.GetComponent<PlayerCtrl>().damage;//HP減少
            if (hp <= 0)
            {
                MonsterDie();
            }
            Destroy(coll.gameObject);
        }
        if (coll.gameObject.tag == "FIREBALL")//中間ボスがプレイヤーのファイアーボールに当たった時
        {
            
            hp -= Damage.GetComponent<PlayerCtrl>().damage;//HP減少
            if (hp <= 0)
            {
                MonsterDie();
            }
            Destroy(coll.gameObject);
        }


    }

    void MonsterDie()
    {
        StopAllCoroutines();
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.Stop();
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        _animation.CrossFade(anim.die.name, 0.5f);
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
        gameUI.DispScore(30);//経験値付与

        Speed = 0.0f;
        lookat = 0;
        StartCoroutine(this.PushObjectPool());
    }
    IEnumerator PushObjectPool()//モンスターオブジェクト還元ルーチン
    {
        yield return new WaitForSeconds(1.0f);//１秒後死体を退く
        isDie = false;
        hp = initHp;
        monsterState = MonsterState.idle;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        Speed = 4.0f;
        lookat = 1;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

         foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = true;
        }

        gameObject.SetActive(false);
    }
    IEnumerator CreateBloodEffect(Vector3 pos)
    {
        GameObject _blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(_blood1, 2.0f);

        yield return null;
    }
    IEnumerator TransStateToMove()
    {
        yield return new WaitForSeconds(0.2f);
    }

    void OnPlayerDie()//プレイヤーが死亡した時に実行される関数
    {
        monsterState = MonsterState.idle;
        StopAllCoroutines();
        nvAgent.Stop();
        _animation.CrossFade(anim.idle.name, 0.3f);
        Speed = 0.0f;
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
    
    }

    public void ColliderTrue()//コライダーを当ててダメージを当てる形式で攻撃をしようとしましたが、実際は距離内にダメージを与えるだけで、実際は働きが無いです。
    {
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = true;
        }
    }
    IEnumerator CreatePlayerBloodEffect(Vector3 pos)
    {
        GameObject _blood2 = (GameObject)Instantiate(PlayerBloodEffect, pos += new Vector3(0, Random.Range(1, 1.5f),0),Quaternion.identity);
        Destroy(_blood2, 2.0f);

        yield return null;
    }

}