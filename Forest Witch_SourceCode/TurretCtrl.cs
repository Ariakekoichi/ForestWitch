using UnityEngine;
using System.Collections;

public class TurretCtrl : MonoBehaviour
{

    public enum MonsterState { idle, trace, attack, die };// モンスターの状態情報のある Enumerable 変数宣言
    public MonsterState monsterState = MonsterState.idle;// モンスターの現在状態の情報を入れるEnum変数                                           
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    public GameObject TurretCtrl1;

    public GameObject FireObject;//銃弾発車位置の変数
    private GameUI gameUI;//GameUIを参照するための変数

    private float attackDist = 16.25f;//この距離以内なら相手を攻撃する
    private float ftime = 0.0f;//攻撃できるタイミング

    public bool isDie = false;//モンスターの生死
 
    public GameObject bloodEffect;//血痕効果
    public GameObject DestroyEffect;
    public int hp = 200;//TurretのHP。。実際は働いてない関数
    public int initHp = 200;//Turretの最大HP。。実際は働いてない関数
    public float Speed = 0.0f;//Turretなので動く必要がありません。
 
    public GameObject Target;//追跡対象
    public GameObject Damage;//PlayerCtrlからダメージ値を持ってくるための設定
    public GameObject Monster;//モンスター自分のY座標を見るために設定する変数
    int lookat = 1;//モンスターの視線に関する変数
    public int fire = 0;//トリガ
    MonsterState newState;
    public GameObject Turret;

    void start()
    {
        TurretCtrl1 = GameObject.Find("Turret");
    }
    void Update()
    {
        Vector3 LookPos = Target.transform.position;
        LookPos.y = Monster.transform.position.y;//モンスター自分のY座標を見る。これがないとPLAYERのY座標を見てしまうため、モンスターの動きが雑になる。
        ftime += Time.deltaTime;
        if (lookat == 1)//モンスターが見るとき
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

            if (dist <= attackDist && ftime > 2.0f && Damage.GetComponent<PlayerCtrl>().Kanchi == 1)//攻撃距離範囲内に入ったかを確認
            {
                
                monsterState = MonsterState.attack;
            
            }
            
            else
            {
                monsterState = MonsterState.idle;
                
            }
            
            fire = 0;
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
                case MonsterState.idle://idle 状態
                   
                    fire = 0;
                    nvAgent.Stop();
                    break;

               
                case MonsterState.trace://追跡状態
                    fire = 0;
                    Vector3 dir = Target.transform.position - transform.position;
                    dir.Normalize();
              
                    nvAgent.destination = playerTr.position;
                   

                    
                    break;

               
                case MonsterState.attack: //攻撃状態
                    nvAgent.Stop();
                    fire = 1;
                    ftime = 0;
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
            Destroy(coll.gameObject);//ぶつかったものの削除

        }
        if (coll.gameObject.tag == "FIREBALL")//中間ボスがプレイヤーのファイアーボールに当たった時
        {
            
            hp -= Damage.GetComponent<PlayerCtrl>().damage;//HP減少
            if (hp <= 0)
            {
                MonsterDie();
            }
            Destroy(coll.gameObject);//ぶつかったものの削除

        }
    }

    void MonsterDie()//モンスター死亡時の処理過程。。TURRETは死なないので働きが無いです。
    {

        StopAllCoroutines();
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.Stop();
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        
        gameUI.DispScore(200);//経験値付与。。TURRETは死なないので働きが無いです。
        StartCoroutine(CreateDestroyEffect(Turret.transform.position));//TURRETが爆発するイベントですが、TURRETは死なないので働きが無いです。
        Speed = 0.0f;
 
        lookat = 0;
        FireObject.SetActive(false);
        StartCoroutine(this.PushObjectPool());

    }
    IEnumerator PushObjectPool()//モンスターオブジェクト還元ルーチン
    {
        yield return new WaitForSeconds(0.1f);
        isDie = false;
        hp = initHp;
        monsterState = MonsterState.idle;
        Speed = 0.0f;
        lookat = 1;
        FireObject.SetActive(true);
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = true;

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
    IEnumerator CreateDestroyEffect(Vector3 pos)
    {
        GameObject _blood1 = (GameObject)Instantiate(DestroyEffect, pos, Quaternion.identity);
        Destroy(_blood1, 2.0f);

        yield return null;
    }
    IEnumerator TransStateToMove()
    {
        
        yield return new WaitForSeconds(0.5f);
       
        fire = 0;
    }
    void OnPlayerDie()//プレイヤーが死亡した時に実行される関数
    {
        StopAllCoroutines();//モンスター（若しくはボス）の状態をチェックするコルーチン関数を全部停止させる
        nvAgent.Stop();

        Speed = 0.0f;
        fire = 0;
    }
    


}
