using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DragonCtrl : MonoBehaviour
{

    public enum MonsterState { idle, trace, attack, die, modori };// モンスターの状態情報のある Enumerable 変数宣言
    public MonsterState monsterState = MonsterState.idle;// モンスターの現在状態の情報を入れるEnum変数
                                              
    private Transform monsterTr;
    private Transform playerTr;
    private Transform ModoruPoint;
    private NavMeshAgent nvAgent;
    public GameObject FireObject;//銃弾発車位置の変数
    private GameUI gameUI;//GameUIを参照するための変数

    private float traceDist = 23.0f;//この距離以内なら相手を追跡する
    private float attackDist = 16.25f;//この距離以内なら相手を攻撃する
    
    public bool isDie = false;//モンスターの生死

    public GameObject bloodEffect;//血痕効果
    public GameObject DestroyEffect;
    public GameObject DragonHPBar;//DragonHPBarを無くすために設定
    public GameObject FinalBossHPBar = null;//처음에는 파이널 보스의 피통이 보이지않게
    public GameObject FinalBoss = null;//처음에는 파이널 보스가 보이지않게
    public GameObject FinalBossSyutsugen = null;
    public int hp = 800;//DragonのHP
    public int initHp = 800;//Dragonの最大HP
    public float Speed = 10.0f;
    public float rotSpeed = 10.0f;
    public GameObject Target;//追跡対象（プレイヤー）
    public GameObject Damage;//PlayerCtrlからダメージ値を持ってくるための設定
    int lookat = 1;//モンスターの視線に関する変数
    public GameObject Monster;//モンスター自身のY座標を見るための変数。（これがないとモンスターが変なところを見るようになる）
    public int fire = 0;//トリガ
    private Animator _animator;
    MonsterState newState;
	private int HPkaifukuryoku = 5;//HP回復力←HP回復がないと非常に難易度が下がる
    public float HPkaifukutime = 0;//HP回復タイミング

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
        LookPos.y = Monster.transform.position.y;//モンスター自分のY座標を見る。これがないとPLAYERのY座標を見てしまうため、モンスターの動きが雑になる。

        if (lookat == 1)
        {
            transform.LookAt(LookPos);//モンスターがプレイヤーを見るようにさせる
        }
    }
    
    void Awake()
    {
        
        monsterTr = GetComponent<Transform>();//モンスターのTransform割当
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();//追跡対象のプレイヤーのTransform割当
        Target = playerTr.gameObject;
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();//NavMeshAgent コンポーネント割当
        ModoruPoint = GameObject.Find("DragonPoint").GetComponent<Transform>();
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();//GameUI オブジェクトの GameUI スクリプトを割当
        _animator = this.gameObject.GetComponent<Animator>();
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
           
            yield return new WaitForSeconds(0.05f);
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);//モンスターとプレイヤーの間の距離測定

            if (dist <= attackDist)//攻撃距離範囲内に入ったかを確認
            {
                
                monsterState = MonsterState.attack;
                yield return new WaitForSeconds(1.45f);
                if (Damage.GetComponent<PlayerCtrl>().BossKanchi == 0)//真ん中のフロアに来た時、ドラゴンを生成位置に返す
                {
                    monsterState = MonsterState.modori;
                }
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;
                if (Damage.GetComponent<PlayerCtrl>().BossKanchi == 0)//真ん中のフロアに来た時、ドラゴンを生成位置に返す
                {
                    monsterState = MonsterState.modori;
                }
            }
            else
            {
                if (Damage.GetComponent<PlayerCtrl>().BossKanchi == 1)//真ん中ではない時
                {
                    monsterState = MonsterState.idle;
                }
                if (Damage.GetComponent<PlayerCtrl>().BossKanchi == 0)//真ん中のフロアに来た時、ドラゴンを生成位置に返す
                {
                    monsterState = MonsterState.modori;
                }
            }
        }
    }
    IEnumerator MonsterAction()//モンスターの状態の値によって適切な動作を遂行する関数
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
                    nvAgent.Stop();//追跡中止
                    _animator.SetBool("IsAttack", false);
                    _animator.SetBool("IsTrace", false);

                    break;
                case MonsterState.modori://中間ボスが生成位置に戻る
                    fire = 0;
                    Vector3 dir1 = ModoruPoint.transform.position - transform.position;
                    transform.position += (dir1 * Time.deltaTime * 0.3f);
                    _animator.SetBool("IsAttack", false);
                    _animator.SetBool("IsTrace", true);

                    break;
                
                case MonsterState.trace://追跡状態
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
                    nvAgent.destination = playerTr.position;//追跡対象の位置情報を提供
                    _animator.SetBool("IsAttack", false);
                    _animator.SetBool("IsTrace", true);


                    break;
                case MonsterState.attack://攻撃状態
                    nvAgent.Stop();//追跡中止
                    _animator.SetBool("IsAttack", true);
                    yield return new WaitForSeconds(0.15f);
                    fire = 1;
                    StartCoroutine("TransStateToMove", 0);
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
            if (Damage.GetComponent<PlayerCtrl>().Dragon == 0)
            {
                hp -= Damage.GetComponent<PlayerCtrl>().damage;//HP減少
                if (hp <= 0)
                {
                    MonsterDie();
                    
                }
            }
            Destroy(coll.gameObject);//ぶつかったものの削除
        }
        if (coll.gameObject.tag == "FIREBALL")//中間ボスがプレイヤーのファイアーボールに当たった時
        {
            
            if (Damage.GetComponent<PlayerCtrl>().Dragon == 0)
            {
                hp -= Damage.GetComponent<PlayerCtrl>().damage;//HP減少
                if (hp <= 0)
                {
                    MonsterDie();
                    
                }
            }
            Destroy(coll.gameObject);//ぶつかったものの削除

        }

    }

    void MonsterDie()//モンスター死亡時の処理過程
    {
        DragonHPBar.SetActive(false);
        FinalBoss.SetActive(true);//FinalBoss（次のボス）が現れる
        FinalBossHPBar.SetActive(true);//FinalBoss（次のボス）のHPバーが現れる
        FinalBossSyutsugen.SetActive(true);
        Damage.GetComponent<PlayerCtrl>().BGMSource.clip = Damage.GetComponent<PlayerCtrl>().BossBGM;
        Damage.GetComponent<PlayerCtrl>().BGMSource.Play();
        _animator.SetTrigger("IsDie");
        Destroy(FinalBossSyutsugen, 10.0f);//10秒後ドラゴン出没メッセージを消す
        gameUI.DispScore(1000);//経験値付与
        gameUI.GetComponent<GameUI>().HPpotion += 1;//プレイヤーHPポーション獲得
        gameUI.GetComponent<GameUI>().MPpotion += 1;//プレイヤーMPポーション獲得

        StopAllCoroutines();//すべてのコルーチン停止
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.Stop();
        gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameUI.DispScore(0);
        Speed = 0.0f;
        rotSpeed = 0.0f;
        lookat = 0;
        FireObject.SetActive(false);
        StartCoroutine(this.PushObjectPool());
        
    }
   
    IEnumerator PushObjectPool() //モンスターオブジェクト還元ルーチン
    {
        yield return new WaitForSeconds(2.0f);//2秒後死体を退く

        Damage.GetComponent<PlayerCtrl>().Dragon = 1;
        isDie = false;
        hp = initHp;
        monsterState = MonsterState.idle;
        Speed = 2.0f;
        lookat = 1;
        rotSpeed = 10.0f;
        FireObject.SetActive(true);
        gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = true;
        }

        gameObject.SetActive(false);
    }
    IEnumerator CreateBloodEffect(Vector3 pos)//血痕効果生成
    {
        GameObject _blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(_blood1, 2.0f);

        yield return null;
    }
    IEnumerator TransStateToMove()
    {
        yield return new WaitForSeconds(0.5f);//0.5秒間火を発射（しっぱなしに）する
        fire = 0;
        monsterState =MonsterState.idle;
        fire = 0;//攻撃しないようもう一回書いておく

    }
    void OnPlayerDie()//プレイヤーが死亡した時に実行される関数
    {
        StopAllCoroutines();//モンスター（若しくはボス）の状態をチェックするコルーチン関数を全部停止させる
        nvAgent.Stop();//追跡を停止してアニメーションを遂行

        Speed = 0.0f;//モンスター（若しくはボス）を動かせない
        fire = 0;
    }
    IEnumerator CreateDestroyEffect(Vector3 pos)
    {
        GameObject _blood1 = (GameObject)Instantiate(DestroyEffect, pos, Quaternion.identity);
        Destroy(_blood1, 2.0f);

        yield return null;
    }

}
