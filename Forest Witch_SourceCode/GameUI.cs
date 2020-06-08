using UnityEngine;
using UnityEngine.UI;//UIコンポーネントに接近するために追加したネームスペース
using System.Collections;
using System;

public class GameUI : MonoBehaviour {
    public Text txtScore;//Text UI項目連結のための変数
    public GameObject AnyObject;
    public GameObject FinalBoss;
    public GameObject Dragon;
    public GameObject ChukanBoss;
    public GameObject Help;
    public GameObject MiniMaping;
    public Text txtLevel;//Text UI Level表記のための変数표기를 위한 변수
    public Text txtHP;//Text UI HP表記のための変数
    public Text txtMP;//Text UI MP表記のための変数
    public Text txtEXP;//Text UI EXP表記のための変数
    public Text txtDamage;//攻撃力表記のための変数
    public Text txtHighScore;//HighScore表記のための変数
    public Text txtHPpotion;
    public Text txtMPpotion;
    public Slider EXPbar;
    public Slider HPBar;
    public Slider FinalBossbar = null;
    public Slider Dragonbar = null;
    public Slider ChukanBossbar;
    public GameObject HeadUpPosition;
    public GameObject ChukanBossSyutsugen = null; // 中間ボス出現 
    public GameObject ChukanBossSyutsugenAnnai = null;//中間ボス出現のアナウンス
    public GameObject ChukanBossSyutsugenHPBar = null;//中間ボスHPBarの出現
    public GameObject Annai;
    public float ChukanBossSyutsugenTiming = 0;  // ゲームが始まった後中間ボス出現タイミング時間。。実際のゲームでは2分となっています。
    

    private MonsterCtrl MonsterHP;//Monsterの HP情報を持ってくるための変数
    private SoldierCtrl SoldierHP;//Soldierの HP情報を持ってくるための変数
    private TurretCtrl TurretHP;//Turretの HP情報を持ってくるための変数
    public int totScore = 0; // 累積経験値を記録するための変数
    public int maeScore = 0;　// レベルアップの前の経験値を記録するための変数
    public int tsugiScore = 200; // 次のレベルアップのための経験値
    public int HPpotion = 2;
    public int MPpotion = 2;
    public int Helper = 0;
    public int MiniMaphyouji = 1;
    public int IsFullScreen = 1; //FullScreenであるかないか
    private int Level = 1;
    public int Saisoku = 0;
    
    public int HighScore;
    [SerializeField]
    Image[] images = new Image[4];

    [SerializeField]
    Sprite[] numberSprites = new Sprite[10];

    public float timeCount { get; private set; }
    void Start () {
        AnyObject = GameObject.Find("Player");
        DispScore(0);
        SetTime(1800);//時間制限30分
        Help.SetActive(false);
        MiniMaping.SetActive(true);
        Invoke("CyukanBoss", 120.0f);//ゲームスタート120秒後中間ボス登場
        Invoke("Annaisuru", 3.0f);//ゲームスタート３秒後案内アナウンス

    }
    void Awake()
    {
        
    }
    void Update()
    {
        

        if (ChukanBossSyutsugenTiming <= 120)//120秒後登場
        {
            ChukanBossSyutsugenTiming += Time.deltaTime;
        }
        HighScore = PlayerPrefs.GetInt("HighScore");
        ChukanBoss = GameObject.Find("ChukanBoss");
        Dragon = GameObject.Find("Dragon");
        FinalBoss = GameObject.Find("FinalBoss");
        txtHP.text = "<color=#ffffff>" + " HP " + AnyObject.GetComponent<PlayerCtrl>().hp.ToString() + " / " + AnyObject.GetComponent<PlayerCtrl>().initHp.ToString() + "</color>";
        txtMP.text = "<color=#ffffff>" + " MP " + AnyObject.GetComponent<PlayerCtrl>().mp.ToString() + " / " + AnyObject.GetComponent<PlayerCtrl>().initMp.ToString() + "</color>";
        txtHPpotion.text = "HP potion " + "<color=#ffff00>" + HPpotion + "</color>";
        txtMPpotion.text = "MP potion " + "<color=#ffff00>" + MPpotion + "</color>";
        if (PlayerPrefs.HasKey("HighScore"))//HighScoreの値があるかないかをチェック
        {
            txtHighScore.text = "<color=#ff7f00>" + "Fastest Clear" + " </color>" + "<color=#ffffff>" + HighScore/60 + " 分 " + HighScore%60 + " 秒" + "</color>";
        }
        else //HighScoreの値がない場合
        {
            txtHighScore.text = "<color=#ff7f00>" + "Fastest Clear" + " </color>" + "<color=#ffffff>" + "None" + "</color>";
        }
        if (Level < 15)//最高レベルではない場合
        {
            txtEXP.text = "<color=#ffffff>" + totScore + " / " + tsugiScore + "</color>";
        }
        if (Level == 15)//最高レベルである場合
        {
            txtEXP.text = "<color=#ffffff>" + tsugiScore + " / " + tsugiScore + "</color>";//Image UI項目のfillAmount属性を調節してEXPゲージ調節
        }
        if(FinalBoss != null)//FinalBossがいる場合だけHPバーが作動するように
        {
        FinalBossbar.value = (((float)FinalBoss.GetComponent<FinalBossCtrl>().hp) / (FinalBoss.GetComponent<FinalBossCtrl>().initHp));//FinalBoss　HPバー調節
        }
        if (Dragon != null)//Dragonがいる場合だけHPバーが作動するように
        {
            Dragonbar.value = (((float)Dragon.GetComponent<DragonCtrl>().hp) / (Dragon.GetComponent<DragonCtrl>().initHp));//Dragon　HPバー調節
        }
        if (ChukanBoss != null)//ChukanBossがいる場合だけHPバーが作動するように
        {
            ChukanBossbar.value = (((float)ChukanBoss.GetComponent<ChukanBossCtrl>().hp) / (ChukanBoss.GetComponent<ChukanBossCtrl>().initHp));//ChukanBoss　HPバー調節
        }
        if (Input.GetKeyDown(KeyCode.R) && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0 && HPpotion != 0 && AnyObject.GetComponent<PlayerCtrl>().hp != AnyObject.GetComponent<PlayerCtrl>().initHp)
        {
            AnyObject.GetComponent<PlayerCtrl>().hp = AnyObject.GetComponent<PlayerCtrl>().initHp;//プレイヤーHP回復
            HPpotion -= 1;
            return;
        }
        if (Input.GetKeyDown(KeyCode.T) && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0 && MPpotion != 0 && AnyObject.GetComponent<PlayerCtrl>().mp != AnyObject.GetComponent<PlayerCtrl>().initMp)
        {
            AnyObject.GetComponent<PlayerCtrl>().mp = AnyObject.GetComponent<PlayerCtrl>().initMp;//プレイヤーMP回復
            MPpotion -= 1;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Tab))//HELP表示関連
        {
            Helping();   
        }
        if (Input.GetKeyDown(KeyCode.M))//ミニマップ表示関連
        {
            MiniMapWinker();
        }
        if (Input.GetKeyDown(KeyCode.L))//フルスクリーン切り替え関連
        {
            FullScreener();
        }
        

    }
    void Helping()
    {
        if (Helper == 0)
        {
            Help.SetActive(true);
            Helper = 1;//HELPを見せる
        }
        else
        {
            Help.SetActive(false);
            Helper = 0;//HELPを見せない
        }
    }
    void MiniMapWinker()
    {
        if (MiniMaphyouji == 0)
        {
            MiniMaping.SetActive(true);
            MiniMaphyouji = 1;
        }
        else
        {
            MiniMaping.SetActive(false);
            MiniMaphyouji = 0;
        }
    }
    void FullScreener()
    {
        if(IsFullScreen == 0)
        {
            Screen.SetResolution(1280, 800, !Screen.fullScreen);
            IsFullScreen = 1;
        }
        else
        {
            Screen.SetResolution(1280, 800, !Screen.fullScreen);
            IsFullScreen = 0;
        }
    }
    public void SetTime(float time)
    {
        timeCount = time;
        
        StartCoroutine(TimerStart());
        
    }
    void SetNumbers(int sec, int val1, int val2)//ゲーム画面上の左下の時間制限の見せ方
    {
        string str = String.Format("{0:00}", sec);
        images[val1].sprite = numberSprites[Convert.ToInt32(str.Substring(0, 1))];
        images[val2].sprite = numberSprites[Convert.ToInt32(str.Substring(1, 1))];
    }
    IEnumerator TimerStart()
    {
        while (timeCount >= 0 && AnyObject.GetComponent<PlayerCtrl>().FinalBoss == 0)
        {
            timeCount -= 1.0f;
            Saisoku += 1;
            int sec = Mathf.FloorToInt(timeCount % 60);
            SetNumbers(sec, 2, 3);
            int minu = Mathf.FloorToInt((timeCount - sec) / 60);
            SetNumbers(minu, 0, 1);
            yield return new WaitForSeconds(1.0f);
        }
        if (AnyObject.GetComponent<PlayerCtrl>().FinalBoss == 0)//0秒になってもFINALBOSSが死んでなかったらプレイヤーを殺す。
        {
            AnyObject.GetComponent<PlayerCtrl>().hp = 0;
            
        }
        
    }
    public void DispScore(int score)
    {
        totScore += score;//スコアセーブ
        
        if (totScore >= 200 && Level == 1 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//レベル2にアップ基準。。レベル1でないとレベル2に上がれない。
        {
            Level = 2;
            maeScore = 200;
            tsugiScore = 500;
            
            AnyObject.GetComponent<PlayerCtrl>().Level = 2;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 500 && Level == 2 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//レベル3にアップ基準。。レベル2でないとレベル2に上がれない。
        {
            Level = 3;
            maeScore = 500;
            tsugiScore = 1000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 3;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 1000 && Level == 3 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//レベル4にアップ基準。。レベル3でないとレベル2に上がれない。
        {
            Level = 4;
            maeScore = 1000;
            tsugiScore = 1600;

            AnyObject.GetComponent<PlayerCtrl>().Level = 4;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 1600 && Level == 4 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)
        {
            Level = 5;
            maeScore = 1600;
            tsugiScore = 2500;

            AnyObject.GetComponent<PlayerCtrl>().Level = 5;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 2500 && Level == 5 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)
        {
            Level = 6;
            maeScore = 2500;
            tsugiScore = 3500;

            AnyObject.GetComponent<PlayerCtrl>().Level = 6;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 3500 && Level == 6 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)
        {
            Level = 7;
            maeScore = 3500;
            tsugiScore = 5000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 7;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 5000 && Level == 7 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)
        {
            Level = 8;
            maeScore = 5000;
            tsugiScore = 7000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 8;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 7000 && Level == 8 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)
        {
            Level = 9;
            maeScore = 7000;
            tsugiScore = 9000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 9;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 9000 && Level == 9 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)
        {
            Level = 10;
            maeScore = 9000;
            tsugiScore = 12000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 10;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 12000 && Level == 10 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)
        {
            Level = 11;
            maeScore = 12000;
            tsugiScore = 16000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 11;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 16000 && Level == 11 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)
        {
            Level = 12;
            maeScore = 16000;
            tsugiScore = 21000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 12;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 21000 && Level == 12 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)
        {
            Level = 13;
            maeScore = 21000;
            tsugiScore = 27000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 13;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 27000 && Level == 13 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//レベル14にアップ基準
        {
            Level = 14;
            maeScore = 27000;
            tsugiScore = 35000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 14;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        if (totScore >= 35000 && Level == 14 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//レベル15にアップ基準
        {
            Level = 15;
            maeScore = 35000;
            tsugiScore = 35000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 15;
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;
        }
        txtLevel.text = "Level <color=#ffff00>" + Level.ToString() + "</color>";
        if(Level != 15 || AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//プレイヤーが生きているか、レベルが最高レベルではないならEXPゲージが動くようにする。（レベル15ならゲージは満タン）
        { 
        EXPbar.value= ((float)totScore - (float)maeScore) / ((float)tsugiScore - (float)maeScore);
        }
        
    }
    void CyukanBoss()
    {
            ChukanBossSyutsugenTiming = 200;
            ChukanBossSyutsugen.SetActive(true);
            ChukanBossSyutsugenAnnai.SetActive(true);
            ChukanBossSyutsugenHPBar.SetActive(true);
            Invoke("chukanBossSyutsugenAnnai", 5.0f);
    }
    void chukanBossSyutsugenAnnai()
    {
        ChukanBossSyutsugenAnnai.SetActive(false);
    }
    void Annaisuru()
    {
        Annai.SetActive(true);
        Destroy(Annai,10.0f);
    }
}
