using UnityEngine;
using System.Collections;

public class ChukanBossCtrl : MonoBehaviour
{

    public enum MonsterState { idle, trace, attack, die, modori };// モンスターの状態情報のある Enumerable 変数宣言
    public MonsterState monsterState = MonsterState.idle;// モンスターの現在状態の情報を入れるEnum変数
 　　　                                             
    private Transform monsterTr;
    private Transform playerTr;
    private Transform ModoruPoint;
    private NavMeshAgent nvAgent;
  
    public GameObject FireObject;//銃弾発車位置の変数
    public GameObject PlayerBloodEffect;
    private GameUI gameUI;//GameUIに接近するための変数

    private float traceDist = 23.0f;//この距離以内なら相手を追跡する
    private float attackDist = 1.5f;//この距離以内なら相手を攻撃する

    public bool isDie = false;//モンスターの生死
    public GameObject bloodEffect;//血痕効果
    public GameObject ChukanBossHPBar;//ChukanBossHPBarを無くすために設定
    public GameObject DragonHPBar = null;//最初はドラゴンのHPBARが見えないように
    public GameObject Dragon = null;//最初はドラゴンが見えないように
    public GameObject DragonSyutsugen = null;//ドラゴン出没メッセージが最初は見えないように
    public int hp = 1000;
    public int initHp = 1000;
    public float Speed = 5.0f;
    public float rotSpeed = 10.0f;
    public GameObject Target;//追跡対象（プレイヤー）
    public GameObject Damage;//PlayerCtrlからダメージ値を持ってくるための設定
    int lookat = 1;//モンスターの視線に関する変数
    public Anim anim;
    public Animation _animation;
    public GameObject Monster;//モンスター自身のY座標を見るための変数。（これがないとモンスターが変なところを見るようになる）
    public int fire = 0;
	private int HPkaifukuryoku = 5;//HP回復力←HP回復がないと非常に難易度が下がる
    public float HPkaifukutime = 0;//HP回復タイミング

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
        Vector3 Modoru = ModoruPoint.transform.position;
		Modoru.y = Monster.transform.position.y;//モンスター自分のY座標を見る。これがないとPLAYERのY座標を見てしまうため、モンスターの動きが雑になる。

        if (lookat == 1 && Damage.GetComponent<PlayerCtrl> ().ChukanBossKanchi == 1) {
			transform.LookAt (LookPos);//モンスターがプレイヤーを見るようにする
        }
		if(lookat == 1 && Damage.GetComponent<PlayerCtrl> ().ChukanBossKanchi == 0)
		{
			transform.LookAt (Modoru);
		}
    }
    
    void Awake()
    {
        
        monsterTr = GetComponent<Transform>();//モンスターのTransform割当
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();//追跡対象のプレイヤーのTransform割当
        Target = playerTr.gameObject;
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();//NavMeshAgent コンポーネント割当
        _animation = this.gameObject.GetComponentInChildren<Animation>();// Animation コンポーネント割当
        _animation.clip = anim.idle;
        _animation.Play();
        ModoruPoint = GameObject.Find("ChukanBossPoint").GetComponent<Transform>();
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();//GameUI オブジェクトの GameUI スクリプトを割当
        Damage = GameObject.Find("Player");//Damageの値を"Player"から持ってくる

    }
    void OnEnable()//イベント発生時遂行する関数連結
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
        StartCoroutine(this.CheckMonsterState()); //一定の間隔でモンスターの行動の状態をチェックするコルーチン関数実行
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
           
            yield return new WaitForSeconds(0.1f);

            float dist = Vector3.Distance(playerTr.position, monsterTr.position);//モンスターとプレイヤーの間の距離測定
            float Modorukyori = Vector3.Distance(ModoruPoint.position,monsterTr.position);

			if (dist <= attackDist) {//攻撃距離範囲内に入ったかを確認
				if (dist <= 1.5f) {
					Debug.Log (4444);
					Damage.GetComponent<PlayerCtrl> ().hp -= 20;
					StartCoroutine (this.CreatePlayerBloodEffect (playerTr.transform.position));
				}
				if (monsterState != MonsterState.attack) {
					monsterState = MonsterState.attack;

				}
				yield return new WaitForSeconds (0.9f);
			} else if (dist <= traceDist) {//追跡距離範囲以内に入ったかを確認
				monsterState = MonsterState.trace;
				if (Damage.GetComponent<PlayerCtrl> ().ChukanBossKanchi == 0) //真ん中のフロアに来た時、中間ボスを生成位置に返す
                {
					monsterState = MonsterState.modori;
				}
			} 
			else if(Modorukyori <= 1.0f)
			{
				monsterState = MonsterState.idle;
			}
            else
            {
                if (Damage.GetComponent<PlayerCtrl>().ChukanBossKanchi == 1)//真ん中ではない時
                {
					monsterState = MonsterState.modori;

                }
                if (Damage.GetComponent<PlayerCtrl>().ChukanBossKanchi == 0)//真ん中のフロアに来た時、中間ボスを生成位置に返す
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
               
                case MonsterState.idle: //idle 状態
                    fire = 0;
                    nvAgent.Stop();//追跡中止
                    _animation.CrossFade(anim.idle.name, 0.3f);

                    break;
                case MonsterState.modori://中間ボスが生成位置に戻る
                    fire = 0;
                    Vector3 dir1 = ModoruPoint.transform.position - transform.position;
					transform.position += (dir1 * Time.deltaTime * 0.18f);//中間ボスが生成位置に戻るスピード
                    _animation.CrossFade(anim.walk.name, 0.3f);
					Vector3 Modoru = ModoruPoint.transform.position;
					Modoru.y = Monster.transform.position.y;
					transform.LookAt (Modoru);
					
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
                    _animation.CrossFade(anim.walk.name, 0.3f);


                    break;

                
                case MonsterState.attack://攻撃状態
                    nvAgent.Stop(); //追跡中止
                    _animation.CrossFade(anim.attack.name, 0.2f);
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
        if (coll.gameObject.tag == "TurretBullet")//中間ボスがプレイヤーの "TurretBullet"に当たった時←実際には具現していません。
        {
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));
            hp -= 2;
            if (hp <= 0)
            {
                MonsterDie();
            }
            Destroy(coll.gameObject);//ぶつかったものの削除
        }
    }

    void MonsterDie()//モンスター死亡時の処理過程
    {
        ChukanBossHPBar.SetActive(false);
        Dragon.SetActive(true);//Dragon（次のボス）が現れる
        DragonHPBar.SetActive(true);//Dragon（次のボス）のHPバーが現れる
        DragonSyutsugen.SetActive(true);
        Destroy(DragonSyutsugen, 7.0f);//７秒後ドラゴン出没メッセージを消す
        gameUI.DispScore(500);//経験値付与
        gameUI.GetComponent<GameUI>().HPpotion += 1;//プレイヤーHPポーション獲得
        gameUI.GetComponent<GameUI>().MPpotion += 1;//プレイヤーMPポーション獲得
        StopAllCoroutines();//すべてのコルーチン停止
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.Stop();
        _animation.CrossFade(anim.die.name, 0.5f);
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;//モンスターに追加されたコライダーを非活性化
        gameObject.GetComponent<NavMeshAgent>().enabled = false;

        Speed = 0.0f;//死んでるのに動かないように
        rotSpeed = 0.0f;//死んでるのに回転しないように
        lookat = 0;//死んでるから視線を変えられないように
        FireObject.SetActive(false);
        StartCoroutine(this.PushObjectPool());
        
    }
    IEnumerator PushObjectPool() //モンスターオブジェクト還元ルーチン
    {
        yield return new WaitForSeconds(3.0f);//３秒後死体を退く
        isDie = false; //各種変数初期化
        hp = initHp;//ただし、中間ボスの場合は事実上働きが見えない。
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
        Destroy(gameObject, 0.0f);
    }
    IEnumerator CreateBloodEffect(Vector3 pos)
    {
        GameObject _blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity); //血痕効果生成
        Destroy(_blood1, 2.0f);
        yield return null;
    }
    IEnumerator TransStateToMove()
    {
        yield return new WaitForSeconds(0.005f);
        monsterState =MonsterState.idle;
        fire = 0;
        _animation.CrossFade(anim.attack.name, 0.5f);
    }
    void OnPlayerDie()//プレイヤーが死亡した時に実行される関数
    {
        StopAllCoroutines();//モンスター（若しくはボス）の状態をチェックするコルーチン関数を全部停止させる
        nvAgent.Stop();//追跡を停止してアニメーションを遂行
        _animation.CrossFade(anim.idle.name,0.3f);
        Speed = 0.0f;//モンスター（若しくはボス）を動かせない
        fire = 0;
    }
    IEnumerator CreatePlayerBloodEffect(Vector3 pos)
    {
        GameObject _blood2 = (GameObject)Instantiate(PlayerBloodEffect, pos += new Vector3(0, Random.Range(1, 1.5f), 0), Quaternion.identity); //血痕効果生成
        Destroy(_blood2, 2.0f);
        yield return null;
    }
}
