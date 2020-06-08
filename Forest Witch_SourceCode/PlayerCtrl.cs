using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCtrl : MonoBehaviour
{
    public AudioClip GameBGM;
    public AudioClip BossBGM;
	public GameObject TitleBGM;
    public float speed = 10.0f;
    public float rotSpeed = 1.0f;
    public float jumpPower = 7.0f;
    public bool isJumping = true;
    public float Jumptime = 0;//ジャンプタイミング関連関数
    public float Spacetime = 1;
    private Transform ModoruPoint;

  
    public GameObject bloodEffect;  //血痕効果プレハブ
    public GameObject GunDamage;//レベルアップするときに一般攻撃のダメージを上げるための関数
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    private Animator _Animator;//Animatorに接近するための変数
    private GameMgr _gameMgr;//ゲームマネージャに接近するための変数
    public GameObject firePos;
    public GameObject GameOver = null;
    public GameObject Win = null;
    public GameObject ChukanBoss1Alarm = null;
    public GameObject ChukanBoss2Alarm = null;
    public GameObject FinalBossAlarm = null;
    public GameObject SoldierFireObject;//Soldierのダメージを持ってくるための変数
    public GameObject AnyObject;//UIを持ってくるための変数
    public GameObject Monster;
    public GameObject Tobiishi1;
    public GameObject Tobiishi2;
    public GameObject Tobiishi3;
    public GameObject Tobiishi4;
    public GameObject Tobiishi5;
    public GameObject Tobiishi6;
    public GameObject Tobiishi7;
    public GameObject Tobiishi8;
    public GameObject TobiishiKeikoku = null;
    public GameObject ShizumuEffect;
    public GameObject ChukanBoss1HPBar;
    public GameObject ChukanBoss2HPBar;
    public GameObject FinalBossHPBar = null;
    public GameObject Revivemessage = null;
    public AudioSource BGMSource;
    
    private int Mute = 0;
    public int HPkaifukuryoku = 1;//HP回復力
    public float HPkaifukutime = 0;//HP回復タイミング
    public int MPkaifukuryoku = 1;//MP回復力
    public float MPkaifukutime = 0;//MP回復タイミング
    public float Tobiishitime = 0;//3病患警告文を見せるためのタイム

    public int damage = 5;//銃弾のダメージ
    private Transform tr;
    private Animator Anim;
    public int hp = 50;// Player HP
    public int initHp = 50;//PlayerのHP初期値であり最大値
    public Image imgHpbar;//Player Health bar イメージ
    public int mp = 10;
    public int initMp = 10;//PlayerのMP初期値であり最大値
    public Image imgMpbar;//Player MP bar イメージ 
    public int Level = 1;//レベル
    public int LevelUp = 0;
    public int PlayerDead = 0;//レベルアップするときに、もうプレイヤーが死んでたらレベルアップできなくするように必要な変数
    public int Kanchi = 1;
    public int BossKanchi = 1;
	public int ChukanBossKanchi = 1;
    public float Saisoku ;
    public int FinalBoss = 0;//最終ボスのダメージを持ってくるための変数
    public int Dragon = 0;//ドラゴンのダメージを持ってくるための変数
    public int revive = 0;//リスポーンを一回可能にさせるための変数
   

    void Start()
    {
        
        initHp = hp;
        tr = GetComponent<Transform>();
        Anim = GetComponent<Animator>();
        _gameMgr = GameObject.Find("GameManager").GetComponent<GameMgr>();
        GunDamage = GameObject.Find("Bullet");
        SoldierFireObject = GameObject.Find("SoldierFire");
        BGMSource = GetComponent<AudioSource>();
        BGMSource.clip = GameBGM;
        BGMSource.Play();
        AnyObject = GameObject.Find("GameUI");
        Monster = GameObject.FindGameObjectWithTag("MONSTER");
        Tobiishi1 = GameObject.Find("Floor (1)");
        Tobiishi2 = GameObject.Find("Floor (2)");
        Tobiishi3 = GameObject.Find("Floor (3)");
        Tobiishi4 = GameObject.Find("Floor (4)");
        Tobiishi5 = GameObject.Find("Floor (5)");
        Tobiishi6 = GameObject.Find("Floor (6)");
        Tobiishi7 = GameObject.Find("Floor (7)");
        Tobiishi8 = GameObject.Find("Floor (8)");
        ModoruPoint = GameObject.Find("PlayerPoint").GetComponent<Transform>();
		TitleBGM = GameObject.Find ("BGM");
		TitleBGM.SetActive(false);
        
    }

    void Update()
    {
       
        Spacetime += Time.deltaTime;
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        tr.Translate(moveDir * Time.deltaTime * speed, Space.Self);
        
        if (Input.GetKey(KeyCode.E) && PlayerDead == 0)
        {
            this.transform.Rotate(0.0f, 90.0f * rotSpeed * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey(KeyCode.Q) && PlayerDead == 0)
        {
            this.transform.Rotate(0.0f, -90.0f * rotSpeed * Time.deltaTime, 0.0f);
        }
   
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            speed = 10;
            Anim.SetBool("IsForward", true);
            Anim.SetBool("IsLeft", false);
            Anim.SetBool("IsBack", false);
            Anim.SetBool("IsRight", false);

        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            speed = 10;
            Anim.SetBool("IsForward", false);
            Anim.SetBool("IsLeft", false);
            Anim.SetBool("IsBack", true);
            Anim.SetBool("IsRight", false);

        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            speed = 10;
            Anim.SetBool("IsForward", false);
            Anim.SetBool("IsLeft", true);
            Anim.SetBool("IsBack", false);
            Anim.SetBool("IsRight", false);

        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            speed = 10;
            Anim.SetBool("IsForward", false);
            Anim.SetBool("IsLeft", false);
            Anim.SetBool("IsBack", false);
            Anim.SetBool("IsRight", true);

        }
        if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow)))
        {ｒ
            speed = 7.07f;//斜めで行くと、√2 の移動距離で移動してしまうため、√2 倍遅い速度を入れる。これでだいたいのゲームで見える速度は同じになる。
            Anim.SetBool("IsForward", true);
            Anim.SetBool("IsLeft", true);
            Anim.SetBool("IsBack", false);
            Anim.SetBool("IsRight", false);
        }
        if ((Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow)))
        {
            speed = 7.07f;
            Anim.SetBool("IsForward", false);
            Anim.SetBool("IsLeft", true);
            Anim.SetBool("IsBack", true);
            Anim.SetBool("IsRight", false);
        }
        if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.RightArrow)))
        {
            speed = 7.07f;
            Anim.SetBool("IsForward", true);
            Anim.SetBool("IsLeft", false);
            Anim.SetBool("IsBack", false);
            Anim.SetBool("IsRight", true);
        }
        if ((Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow)))
        {
            speed = 7.07f;
            Anim.SetBool("IsForward", false);
            Anim.SetBool("IsLeft", false);
            Anim.SetBool("IsBack", true);
            Anim.SetBool("IsRight", true);
        }
        if (v >= 0.1f)//앞으로이동시
        {
            Anim.SetBool("IsForward", true);
            
        }
        else if(v <= -0.1f)//뒤로이동시
        {
           
            Anim.SetBool("IsBack", true);
           
        }
        else if(h >= 0.1f)//오른쪽이동시
        {
            
            Anim.SetBool("IsRight", true);
        }
        else if (h <= -0.1f)//왼쪽이동시
        {
            Anim.SetBool("IsLeft", true);
         
        }
        else
        {
            Anim.SetBool("IsForward", false);
            Anim.SetBool("IsBack", false);
            Anim.SetBool("IsLeft", false);
            Anim.SetBool("IsRight", false);
        }
        if (Input.GetKey(KeyCode.Escape))//Esc押せばゲーム終了
        {
            Application.Quit();
        }
        if (isJumping == false && Input.GetButtonDown("Jump") && Spacetime >= 0.8)//점프관련 처리
        {
            isJumping = true;//점프중임
            GetComponent<Rigidbody>().AddForce(transform.up * jumpPower, ForceMode.Impulse);//점프하는 동작자체
            Spacetime = 0;
            
        }
       
        if (Input.GetKeyDown(KeyCode.P))//p을 누른다
        {
            BGM();
        }

        if (hp <= 0 && revive == 1)//0이하면 사망처리
        {
            PlayerDie();
            OnPlayerDie();
            PlayerDead = 1;
            _gameMgr.isGameOver = true;//게임매니저의 isGameOver 변수값을 변경해 몬스터 출현을 중지시킴
        }
        if (hp <= 0 && revive == 0)//0이하면 사망처리
        {
            hp = 0;
            speed = 0.0f;////플레이어를 움직이지 못하게
            rotSpeed = 0.0f;//플레이어를 회전하지 못하게
            jumpPower = 0.0f;//죽은뒤 점프못하게!
            firePos.SetActive(false);//플레이어를 총을 못쏘게!
            PlayerDead = 1;
            Revivemessage.SetActive(true);
            HPkaifukuryoku = 0;
            MPkaifukuryoku = 0;
            Invoke("PlayerMove", 1.5f);
            Invoke("PlayerRevive", 3.0f);
            Anim.SetBool("Revive", true);
            

        }
        if (FinalBoss == 1)//최종보스를 이겻을때 게임승리
        {
            PlayerWin();
            OnPlayerDie();
            _gameMgr.isGameOver = true;//게임매니저의 isGameOver 변수값을 변경해 몬스터 출현을 중지시킴
        }
       
        _Animator = this.gameObject.GetComponent<Animator>();
        if (Level == 2 && LevelUp == 1 && PlayerDead == 0)//레벨2기준
        {
           LevelUp = 0;
            hp = 55;
            initHp = 55;//Player의 생명 초기값 이자 최대치
            mp = 15;
            initMp = 15;//Player의 생명 초기값 이자 최대치
      
            damage = 10;
            

        }
        if (Level == 3 && LevelUp == 1 && PlayerDead == 0)//레벨3기준
        {
            LevelUp = 0;
            
            hp = 65;
            initHp = 65;//Player의 생명 초기값 이자 최대치
            mp = 20;
            initMp = 20;
           
            damage = 15;

        }
        if (Level == 4 && LevelUp == 1 && PlayerDead == 0)//레벨4기준
        {
            LevelUp = 0;
            hp = 80;
            initHp = 80;//Player의 생명 초기값 이자 최대치
            mp = 25;
            initMp = 25;
         
            //GunDamage.GetComponent<BulletCtrl>().damage += 5;
            damage = 20;
        }
        if (Level == 5 && LevelUp == 1 && PlayerDead == 0)//레벨5기준
        {
            LevelUp = 0;
            hp = 100;
            initHp = 100;//Player의 생명 초기값 이자 최대치
            mp = 30;
            initMp = 30;
          
            damage = 25;
        }
        if (Level == 6 && LevelUp == 1 && PlayerDead == 0)//레벨6기준
        {
            LevelUp = 0;
            hp = 125;
            initHp = 125;//Player의 생명 초기값 이자 최대치
            mp = 35;
            initMp = 35;
            
            damage = 30;
        }
        if (Level == 7 && LevelUp == 1 && PlayerDead == 0)//레벨7기준
        {
            LevelUp = 0;
            hp = 150;
            initHp = 150;//Player의 생명 초기값 이자 최대치
            mp = 40;
            initMp = 40;
           
            damage = 35;
        }
        if (Level == 8 && LevelUp == 1 && PlayerDead == 0)//레벨8기준
        {
            LevelUp = 0;
            hp = 180;
            initHp = 180;//Player의 생명 초기값 이자 최대치
            mp = 45;
            initMp = 45;
          
            damage = 40;
        }
        if (Level == 9 && LevelUp == 1 && PlayerDead == 0)//레벨9기준
        {
            LevelUp = 0;
            hp = 215;
            initHp = 215;//Player의 생명 초기값 이자 최대치
            mp = 50;
            initMp = 50;

            damage = 45;
        }
        if (Level == 10 && LevelUp == 1 && PlayerDead == 0)//레벨10기준
        {
            LevelUp = 0;
            hp = 250;
            initHp = 250;//Player의 생명 초기값 이자 최대치
            mp = 55;
            initMp = 55;
          
            damage = 50;
        }
        if (Level == 11 && LevelUp == 1 && PlayerDead == 0)//레벨11기준
        {
            LevelUp = 0;
            hp = 300;
            initHp = 300;//Player의 생명 초기값 이자 최대치
            mp = 60;
            initMp = 60;
        
            damage = 55;
        }
        if (Level == 12 && LevelUp == 1 && PlayerDead == 0)//레벨12기준
        {
            LevelUp = 0;
            hp = 355;
            initHp = 355;//Player의 생명 초기값 이자 최대치
            mp = 65;
            initMp = 65;
         
            damage = 60;
        }
        if (Level == 13 && LevelUp == 1 && PlayerDead == 0)//레벨13기준
        {
            LevelUp = 0;
            hp = 415;
            initHp = 415;//Player의 생명 초기값 이자 최대치
            mp = 70;
            initMp = 70;

            damage = 65;
        }
        if (Level == 14 && LevelUp == 1 && PlayerDead == 0)//레벨14기준
        {
            LevelUp = 0;
            hp = 480;
            initHp = 480;//Player의 생명 초기값 이자 최대치
            mp = 75;
            initMp = 75;

            damage = 70;
        }
        if (Level == 15 && LevelUp == 1 && PlayerDead == 0)//레벨15기준
        {
            LevelUp = 0;
            hp = 550;
            initHp = 550;//Player의 생명 초기값 이자 최대치
            mp = 80;
            initMp = 80;

            damage = 75;
        }
        if (MPkaifukutime >= 0 && MPkaifukutime < 1)
        {
            MPkaifukutime += Time.deltaTime;
        }
        while(MPkaifukutime >= 1)
        {
            if(mp < initMp)
            { 
            mp += MPkaifukuryoku;
            }
            MPkaifukutime = 0;
        }
        if(hp != initHp)
        {
            if (HPkaifukutime >= 0 && HPkaifukutime < 2)
            {
                HPkaifukutime += Time.deltaTime;
            }
            while (HPkaifukutime >= 2)
            {
            if (hp < initHp)
                {
                    hp += HPkaifukuryoku;
                }
                HPkaifukutime = 0;
            }
        }

    }
    
void OnTriggerEnter(Collider coll)//데미지 입히는종류들
    {
        if (coll.gameObject.tag == "Water")
        {
            if (FinalBoss == 0)
            {
                hp -= 1000;
                Debug.Log("Player HP " + hp.ToString());
            }
        }
        if (coll.gameObject.tag == "PUNCH")
        {
            if (FinalBoss == 0)
            {
                hp -= 5;
                Debug.Log("Player HP = " + hp.ToString());
                Debug.Log(coll.gameObject);
                coll.enabled = false; 

            }

            StartCoroutine(this.CreateBloodEffect(coll.transform.position));//Player에서 나타나게 하기위해서는 어떻게 해야되지?
           


        }
        if (coll.gameObject.tag == "SoldierBullet")
        {
            if (FinalBoss == 0)
            {
                hp -= coll.gameObject.GetComponent<SoldierBulletCtrl>().damage;
                Debug.Log("Player HP = " + hp.ToString());
            }
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));//Player에서 나타나게 하기위해서는 어떻게 해야되지?
            Destroy(coll.gameObject);//총알이 사라지도록 한다

        }
        if (coll.gameObject.tag == "TurretBullet")
        {
            if (FinalBoss == 0)
            {
                hp -= coll.gameObject.GetComponent<TurretBulletCtrl>().damage;
                Debug.Log("Player HP = " + hp.ToString());
            }
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));//Player에서 나타나게 하기위해서는 어떻게 해야되지?
            Destroy(coll.gameObject);//총알이 사라지도록 한다

        }
        if (coll.gameObject.tag == "SuperTurretBullet")
        {
            if (FinalBoss == 0)
            {
                hp -= coll.gameObject.GetComponent<SuperTurretBulletCtrl>().damage;
                Debug.Log("Player HP = " + hp.ToString());
            }
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));//Player에서 나타나게 하기위해서는 어떻게 해야되지?
            Destroy(coll.gameObject);//총알이 사라지도록 한다

        }
        if (coll.gameObject.tag == "FinalBossBullet")
        {
            if (FinalBoss == 0)
            {
                hp -= coll.gameObject.GetComponent<FinalBossBulletCtrl>().damage;
                Debug.Log("Player HP = " + hp.ToString());
            }
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));//Player에서 나타나게 하기위해서는 어떻게 해야되지?
            Destroy(coll.gameObject);//총알이 사라지도록 한다

        }
        if (coll.gameObject.tag == "DragonBullet")
        {
            if (FinalBoss == 0)
            {
                hp -= coll.gameObject.GetComponent<DragonBulletCtrl>().damage;
                Debug.Log("Player HP = " + hp.ToString());
            }
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));//Player에서 나타나게 하기위해서는 어떻게 해야되지?
            Destroy(coll.gameObject);//총알이 사라지도록 한다

        }
        if (coll.gameObject.tag == "ChukanBossBullet")
        {
            if (FinalBoss == 0)
            {
                hp -= coll.gameObject.GetComponent<ChukanBossBulletCtrl>().damage;
                Debug.Log("Player HP = " + hp.ToString());
            }
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));//Player에서 나타나게 하기위해서는 어떻게 해야되지?
            Destroy(coll.gameObject);//총알이 사라지도록 한다

        }
       // imgHpbar.fillAmount = (float)hp / (float)initHp;//Image UI 항목의 fillAmount 속성을 조절해 생명 게이지 값 조절
     
    }
    void FixedUpdate()
    {
        
    }
    
    private void OnCollisionStay(Collision collision)//점프할수 있는 공간
    {
        
        
            if (collision.gameObject.name == "Floor"|| 
                collision.gameObject.name == "Barrel"||
                collision.gameObject.name == "Turret" ||
                collision.gameObject.name == "SuperTurret" ||
                collision.gameObject.name == "Floor (1)" ||
                collision.gameObject.name == "Floor (2)" ||
                collision.gameObject.name == "Floor (3)" ||
                collision.gameObject.name == "Floor (4)" ||
                collision.gameObject.name == "Floor (5)" ||
                collision.gameObject.name == "Floor (6)" ||
                collision.gameObject.name == "Floor (7)" ||
                collision.gameObject.name == "Floor (8)" )
            {
            isJumping = false;//점프중이아님
            Jumptime = 0;//점프중이아니므로
            if (collision.gameObject.name == "Floor (1)")//밟은뒤에 징검다리가 사라지게하기
            {
                Destroy(collision.collider, 0.0f);
                Destroy(Tobiishi1, 5.0f);
                TobiishiKeikoku.SetActive(true);
                StartCoroutine(this.DeleteTobiishi(collision.transform.position));
         
                return;
                

            }
            if (collision.gameObject.name == "Floor (2)")
            {
                Destroy(collision.collider, 0.0f);
                Destroy(Tobiishi2, 5.0f);
                TobiishiKeikoku.SetActive(true);
                StartCoroutine(this.DeleteTobiishi(collision.transform.position));
               
                return;
            }
            if (collision.gameObject.name == "Floor (3)")
            {
                Destroy(collision.collider, 0.0f);
                Destroy(Tobiishi3, 5.0f);
                TobiishiKeikoku.SetActive(true);
                StartCoroutine(this.DeleteTobiishi(collision.transform.position));
             
                return;
            }
            if (collision.gameObject.name == "Floor (4)")
            {
                Destroy(collision.collider, 0.0f);
                Destroy(Tobiishi4, 5.0f);
                TobiishiKeikoku.SetActive(true);
                StartCoroutine(this.DeleteTobiishi(collision.transform.position));
              
                return;
            }
            if (collision.gameObject.name == "Floor (5)")
            {
                Destroy(collision.collider, 0.0f);
                Destroy(Tobiishi5, 5.0f);
                TobiishiKeikoku.SetActive(true);
                StartCoroutine(this.DeleteTobiishi(collision.transform.position));
              
                return;
            }
            if (collision.gameObject.name == "Floor (6)")
            {
                Destroy(collision.collider, 0.0f);
                Destroy(Tobiishi6, 5.0f);
                TobiishiKeikoku.SetActive(true);
                StartCoroutine(this.DeleteTobiishi(collision.transform.position));
              
                return;
            }
            if (collision.gameObject.name == "Floor (7)")
            {
                Destroy(collision.collider, 0.0f);
                Destroy(Tobiishi7, 5.0f);
                TobiishiKeikoku.SetActive(true);
                StartCoroutine(this.DeleteTobiishi(collision.transform.position));
              
                return;
            }
            if (collision.gameObject.name == "Floor (8)")
            {
                Destroy(collision.collider, 0.0f);
                Destroy(Tobiishi8, 5.0f);
                TobiishiKeikoku.SetActive(true);

                StartCoroutine(this.DeleteTobiishi(collision.transform.position));
              
                
                return;
            }


        }
        if (collision.gameObject.name == "MainStage")
        {
            isJumping = false;//점프중이아님
            Jumptime = 0;//점프중이아니므로
            Kanchi = 0;
            BossKanchi = 0;
			//ChukanBossKanchi = 0;

        }
        if (collision.gameObject.name == "Kaidan")
        {
            isJumping = false;//점프중이아님
            Jumptime = 0;//점프중이아니므로
            BossKanchi = 0;
			ChukanBossKanchi = 0;

        }
    }

    private void OnCollisionExit(Collision collision)//점프할수 있는 공간이 아닐때
    {
        
        Jumptime += Time.deltaTime;//점프하기시작한타임
        isJumping = true;//점프중임
        TobiishiKeikoku.SetActive(false);
        Kanchi = 1;
        BossKanchi = 1;
		ChukanBossKanchi = 1;

    }
    void PlayerDie()//사망관련처리
    {
        
        GameOver.SetActive(true);
        _Animator.SetTrigger("IsPlayerdie");
        speed = 0.0f;////플레이어를 움직이지 못하게
        rotSpeed = 0.0f;//플레이어를 회전하지 못하게
        jumpPower = 0.0f;//죽은뒤 점프못하게!
        firePos.SetActive(false);//플레이어를 총을 못쏘게!
        TobiishiKeikoku.SetActive(false);
        hp = 0;
        HPkaifukuryoku = 0;
        MPkaifukuryoku = 0;
        
        
    }
    void PlayerWin()//승리관련처리
    {
        if (PlayerPrefs.HasKey("HighScore"))//HighScore 키값이 있는지 체크
        {
            if (AnyObject.GetComponent<GameUI>().Saisoku < PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", AnyObject.GetComponent<GameUI>().Saisoku);//HighScore 에 최고기록을 경신
                                                                                            //     HighScore = PlayerPrefs.GetInt("HighScore");
            }
            
        }
        else //HighScore 키값이 읎을경우
        {
            PlayerPrefs.SetInt("HighScore", AnyObject.GetComponent<GameUI>().Saisoku);//HighScore에 최고점수를 갱신
                                                                                       //   HighScore = PlayerPrefs.GetInt("HighScore");
        }
        AnyObject.GetComponent<GameUI>().HighScore = PlayerPrefs.GetInt("HighScore");
        Win.SetActive(true);
        speed = 0.0f;////플레이어를 움직이지 못하게
        rotSpeed = 0.0f;//플레이어를 회전하지 못하게
        jumpPower = 0.0f;//죽은뒤 점프못하게!
        firePos.SetActive(false);//플레이어를 총을 못쏘게!
        TobiishiKeikoku.SetActive(false);
       
        FinalBossHPBar.SetActive(false);
    }
    IEnumerator CreateBloodEffect(Vector3 pos)
    {
        //혈흔 효과 생성
        GameObject _blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(_blood1, 2.0f);

        yield return null;
    }
    void BGM()//BGM처리
    {
        if (Mute == 0)//맨처음설정
        {
            BGMSource.volume = 0;
            Mute = 1;//뮤트모드로
        }
        else
        {
            BGMSource.volume = 0.1f;
            Mute = 0;//재생모드로
        }
    }
    public void Replay()
    {
		TitleBGM.SetActive(true);
        Application.LoadLevel("scMain");
    }
    IEnumerator MPKaifuku()//일정한 간격으로 몬스터의 행동 상태를 체크하고 몬스터스테이트 값 변경
    {
        yield return new WaitForSeconds(0.1f);
        mp += 0;    
    }
    IEnumerator DeleteTobiishi(Vector3 pos)//쓰여지고있나...?응용은가능할듯
    {
        yield return new WaitForSeconds(5.0f);
        GameObject Bakuhatsu = (GameObject)Instantiate(ShizumuEffect, pos, Quaternion.identity);
       // Destroy(Bakuhatsu, 1.0f);
        yield return null;
       
    }
    IEnumerator ChukanBoss1()//중간보스1 등장!
    { 
        ChukanBoss1Alarm.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        ChukanBoss1Alarm.SetActive(false);
        yield return null;

    }
    IEnumerator ChukanBoss2()//중간보스2 등장!
    {
        ChukanBoss2Alarm.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        ChukanBoss2Alarm.SetActive(false);
        yield return null;
    }
    IEnumerator FinalBossAlarming()//최종보스 등장!
    {
        FinalBossAlarm.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        FinalBossAlarm.SetActive(false);
        yield return null;
    }
    void PlayerRevive()
    {
        
        Anim.SetBool("Revive", false);
        hp = initHp;
        mp = initMp;
        revive = 1;
        speed = 10.0f;////플레이어를 움직이지 못하게
        rotSpeed = 1.0f;//플레이어를 회전하지 못하게
        jumpPower = 7.0f;//죽은뒤 점프못하게!
        firePos.SetActive(true);//플레이어를 총을 못쏘게!
        PlayerDead = 0;
        Revivemessage.SetActive(false);
        HPkaifukuryoku = 1;
        MPkaifukuryoku = 1;
    }
    void PlayerMove() {
        
        transform.position = ModoruPoint.transform.position;
        

    }
}