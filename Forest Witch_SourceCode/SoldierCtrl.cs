using UnityEngine;
using System.Collections;

public class SoldierCtrl : MonoBehaviour
{

    public enum MonsterState { idle, trace, attack, die };// モンスターの状態情報のある Enumerable 変数宣言
    public MonsterState monsterState = MonsterState.idle;// モンスターの現在状態の情報を入れるEnum変数
                                                      
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    private Animator _animator;
    public GameObject FireObject;//銃弾発車位置の変数
    private GameUI gameUI;//GameUIを参照するための変数

    private float traceDist = 20.0f;//この距離以内なら相手を追跡する
    private float attackDist = 16.25f;//この距離以内なら相手を攻撃する
    
    public bool isDie = false;//モンスターの生死
    public GameObject bloodEffect;//血痕効果
    public int hp = 20;//モンスターのHP
    public int initHp = 20;//モンスターの最大HP
    public float Speed = 2.0f;
    public float rotSpeed = 10.0f;
    public GameObject Target;//追跡対象（プレイヤー）
    public GameObject Damage;//PlayerCtrlからダメージ値を持ってくるための設定
    int lookat = 1;//モンスターの視線に関する変数
    public GameObject Monster;//モンスター自身のY座標を見るための変数。（これがないとモンスターが変なところを見るようになる）
    public int fire = 0;//トリガ
    MonsterState newState;

    void Update()
    {

        Vector3 LookPos = Target.transform.position;
        LookPos.y = Monster.transform.position.y;

        if (lookat == 1)
        {
            transform.LookAt(LookPos);//モンスターがプレイヤーを見るようにさせる
        }
        
    }
    
    void Awake()
    {
        monsterTr = GetComponent<Transform>();//モンスターのTransform割当
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Target = playerTr.gameObject;
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        _animator = this.gameObject.GetComponent<Animator>();
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();//GameUI オブジェクトの GameUI スクリプトを割当
        Damage = GameObject.Find("Player");//"Player"のDamageの値を持ってくる

    }
    void OnEnable()//イベント発生時遂行する関数連結
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
        StartCoroutine(this.CheckMonsterState());//一定の間隔でモンスターの行動の状態をチェックするコルーチン関数実行
        StartCoroutine(this.MonsterAction());//モンスターの状態によって動作するルーチンを実行するコルーチン関数
    }
    void OnDisable()//イベント発生時遂行する関数連結
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
        StopAllCoroutines();//コルーチン関数の完璧な解除のため入れてある停止コード
    }

    IEnumerator CheckMonsterState()//一定な間隔でモンスターの行動状態チェックしモンスターステート値変更
    {
        while (!isDie)
        {
           
            yield return new WaitForSeconds(0.2f);//0.2秒間状態を確認
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);//モンスターとプレイヤーの間の距離測定

            if (dist <= attackDist)//攻撃距離範囲内に入ったら
            {
                
                monsterState = MonsterState.attack;//攻撃範囲内に入ってたら攻撃
                yield return new WaitForSeconds(1.8f);//攻撃するターム（Term）
            }
            else if (dist <= traceDist)//攻撃範囲以内に入ってないなら
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
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
                    _animator.SetBool("IsAttack", false);
                    _animator.SetBool("IsTrace", false);
                    
                    break;
                case MonsterState.trace:
                    fire = 0;
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
                    _animator.SetBool("IsAttack", false);
                    _animator.SetBool("IsTrace", true);
                   
                    
                    break;
                case MonsterState.attack:
                    nvAgent.Stop();
                   
                    _animator.SetBool("IsAttack", true);
                    fire = 1;
                    StartCoroutine("TransStateToMove", 0);
                    break;

            }
            yield return null;
            
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "BULLET")//プレイヤーの銃弾に当たった時
        {
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));
            hp -= Damage.GetComponent<PlayerCtrl>().damage;//PlayerCtrlからダメージ値を持ってきてHP減少
            if (hp <= 0)
            {
                MonsterDie();
            }
            Destroy(coll.gameObject);//Bullet削除
            
            _animator.SetTrigger("IsHit");//IsHit Triggerを発生させたらAny Stateからgothitになる。
        }
        if (coll.gameObject.tag == "FIREBALL")//プレイヤーの必殺技に当たった時
        {
            
            hp -= Damage.GetComponent<PlayerCtrl>().damage;//PlayerCtrlからダメージ値を持ってきてHP減少
            if (hp <= 0)
            {
                MonsterDie();
            }
            Destroy(coll.gameObject);//Bullet削除
            _animator.SetTrigger("IsHit");//IsHit Triggerを発生させたらAny Stateからgothitになる。
        }

    }

    void MonsterDie()//モンスター死亡時の処理過程
    {
        
        StopAllCoroutines();//すべてのコルーチン停止
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.Stop();
        _animator.SetTrigger("IsDie");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        
        gameUI.DispScore(50);//経験値付与

        Speed = 0.0f;//動けないように
        rotSpeed = 0.0f;//回転できないように
        lookat = 0;//死体がプレイヤーを見ないように
        FireObject.SetActive(false);//銃を撃たないように
        StartCoroutine(this.PushObjectPool());
        
    }
    IEnumerator PushObjectPool()
    {
        yield return new WaitForSeconds(1.0f);//1.0秒後モンスターの死体が消える
        
        isDie = false;
        hp = initHp;
        monsterState = MonsterState.idle;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        Speed = 2.0f;
        lookat = 1;
        rotSpeed = 10.0f;
        FireObject.SetActive(true);
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = true;
        }

        gameObject.SetActive(false);//モンスターを非活性化
    }
    IEnumerator CreateBloodEffect(Vector3 pos)
    {
        GameObject _blood1 = (GameObject)Instantiate(bloodEffect, pos += new Vector3(0, Random.Range(1, 1.5f), 0), Quaternion.identity);
        Destroy(_blood1, 2.0f);

        yield return null;
    }
    IEnumerator TransStateToMove()
    {
        yield return new WaitForSeconds(0.005f);
        fire = 0;//攻撃しないように
        monsterState =MonsterState.idle;
        fire = 0;//攻撃しないようにもう一回確認

    }
    void OnPlayerDie()//プレイヤーが死亡した時実行される関数
    {
        StopAllCoroutines();//モンスターの状態をチェックするコルーチン関数をすべて停止させる
        nvAgent.Stop();//追跡を停止しアニメーションを遂行
        _animator.SetTrigger("IsPlayerDie");
        Speed = 0.0f;//動けなくなる
        fire = 0;//撃てなくなる
    }
   

}
