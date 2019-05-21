using UnityEngine;
using UnityEngine.UI;//UI컴포넌트에 접근하기 위해 추가한 네임스페이스
using System.Collections;
using System;

public class GameUI : MonoBehaviour {
    public Text txtScore;//Text UI 항목 연결을 위한 변수
    public GameObject AnyObject;
    public GameObject FinalBoss;
    public GameObject Dragon;
    public GameObject ChukanBoss;
    public GameObject Help;
    public GameObject MiniMaping;
    public Text txtLevel;//Text UI Level표기를 위한 변수
    public Text txtHP;//Text UI HP표기를 위한 변수..쓰여지고있는지 의문
    public Text txtMP;//Text UI MP표기를 위한 변수
    public Text txtEXP;//Text UI EXP표기를 위한 변수
    public Text txtDamage;//공격력표기를 위한 변수
    public Text txtHighScore;//HighScore표기를 위한 변수
    public Text txtHPpotion;
    public Text txtMPpotion;
    public Slider EXPbar;
    public Slider HPBar;
    public Slider FinalBossbar = null;
    public Slider Dragonbar = null;
    public Slider ChukanBossbar;
    public GameObject HeadUpPosition;
    public GameObject ChukanBossSyutsugen = null; // 중간보스 출현관련 변수 
    public GameObject ChukanBossSyutsugenAnnai = null;
    public GameObject ChukanBossSyutsugenHPBar = null;
    public GameObject Annai;
    public float ChukanBossSyutsugenTiming = 0;  // 중간보스 출현 타이밍 시간.. 2분으로!
    

    private MonsterCtrl MonsterHP;//Monster의 HP정보를 가져오기위한 변수
    private SoldierCtrl SoldierHP;//Soldier의 HP정보를 가져오기위한 변수
    private TurretCtrl TurretHP;//Soldier의 HP정보를 가져오기위한 변수
    public int totScore = 0; // 누적경험치를 기록하기 위한 변수
    public int maeScore = 0;
    public int tsugiScore = 200; // 다음 레벨업을 위한 경험치
    public int HPpotion = 2;
    public int MPpotion = 2;
    public int Helper = 0;
    public int MiniMaphyouji = 1;
    public int IsFullScreen = 1;
    private int Level = 1;//표기를 위한 레벨
    public int Saisoku = 0;
    
    public int HighScore;
    [SerializeField]
    Image[] images = new Image[4];

    [SerializeField]
    Sprite[] numberSprites = new Sprite[10];

    public float timeCount { get; private set; }

    //private int LevelUp = 0;//레벨업해서 능력치를 늘리기위한 변수

    void Start () {
        AnyObject = GameObject.Find("Player");
        DispScore(0);
        SetTime(1800);//시간제한30분
        Help.SetActive(false);
        MiniMaping.SetActive(true);
        Invoke("CyukanBoss", 120.0f);//120초후에 등장!
        Invoke("Annaisuru", 3.0f);
        //PlayerPrefs.DeleteAll();//★ 최고시간에 문제생겼을때 써먹어!
    }
    void Awake()
    {
        
    }
    void Update()
    {
        

        if (ChukanBossSyutsugenTiming <= 120)//120초후에 등장!
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
        if (PlayerPrefs.HasKey("HighScore"))//HighScore 키값이 있는지 체크
        {
            txtHighScore.text = "<color=#ff7f00>" + "Fastest Clear" + " </color>" + "<color=#ffffff>" + HighScore/60 + " 分 " + HighScore%60 + " 秒" + "</color>";
        }
        else //HighScore 키값이 읎을경우
        {
            txtHighScore.text = "<color=#ff7f00>" + "Fastest Clear" + " </color>" + "<color=#ffffff>" + "None" + "</color>";
        }
        if (Level < 15)//레벨이 만렙이 아니라면
        {
            txtEXP.text = "<color=#ffffff>" + totScore + " / " + tsugiScore + "</color>";
        }
        if (Level == 15)//레벨이 만렙이라면
        {
            txtEXP.text = "<color=#ffffff>" + tsugiScore + " / " + tsugiScore + "</color>";//Image UI 항목의 fillAmount 속성을 조절해 생명 게이지 값 조절
        }
        if(FinalBoss != null)//FinalBoss가 있을때만 HP바가 작동이되도록하기
        {
        FinalBossbar.value = (((float)FinalBoss.GetComponent<FinalBossCtrl>().hp) / (FinalBoss.GetComponent<FinalBossCtrl>().initHp));//FinalBoss체력바 조절
        }
        if (Dragon != null)//Dragon이 있을때만 HP바가 작동이되도록하기
        {
            Dragonbar.value = (((float)Dragon.GetComponent<DragonCtrl>().hp) / (Dragon.GetComponent<DragonCtrl>().initHp));//Dragon체력바 조절
        }
        if (ChukanBoss != null)//ChukanBoss가 있을때만 HP바가 작동이되도록하기
        {
            ChukanBossbar.value = (((float)ChukanBoss.GetComponent<ChukanBossCtrl>().hp) / (ChukanBoss.GetComponent<ChukanBossCtrl>().initHp));//ChukanBoss체력바 조절
        }
        if (Input.GetKeyDown(KeyCode.R) && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0 && HPpotion != 0 && AnyObject.GetComponent<PlayerCtrl>().hp != AnyObject.GetComponent<PlayerCtrl>().initHp)
        {
            AnyObject.GetComponent<PlayerCtrl>().hp = AnyObject.GetComponent<PlayerCtrl>().initHp;//플레이어 HP회복
            HPpotion -= 1;
            return;
        }
        if (Input.GetKeyDown(KeyCode.T) && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0 && MPpotion != 0 && AnyObject.GetComponent<PlayerCtrl>().mp != AnyObject.GetComponent<PlayerCtrl>().initMp)
        {
            AnyObject.GetComponent<PlayerCtrl>().mp = AnyObject.GetComponent<PlayerCtrl>().initMp;//플레이어 MP회복
            MPpotion -= 1;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Tab))//HELP표시관련
        {
            Helping();   
        }
        if (Input.GetKeyDown(KeyCode.M))//미니맵표시관련
        {
            MiniMapWinker();
        }
        if (Input.GetKeyDown(KeyCode.L))//풀스크린변환관련
        {
            FullScreener();
        }
        

    }
    void Helping()
    {
        if (Helper == 0)//맨처음설정
        {
            Help.SetActive(true);
            Helper = 1;//뮤트모드로
        }
        else
        {
            Help.SetActive(false);
            Helper = 0;//재생모드로
        }
    }
    void MiniMapWinker()
    {
        if (MiniMaphyouji == 0)//맨처음설정
        {
            MiniMaping.SetActive(true);
            MiniMaphyouji = 1;//뮤트모드로
        }
        else
        {
            MiniMaping.SetActive(false);
            MiniMaphyouji = 0;//재생모드로
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
    public void SetTime(float time)//시간제한 죽였을때 time = 300해보기!
    {
        timeCount = time;
        
        StartCoroutine(TimerStart());
        
    }
    // 점수 누적 및 화면 표시
    void SetNumbers(int sec, int val1, int val2)
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
        //0초가되었는데도 파이널보스가 죽어있지않다면 플레이어를 죽인다
        
        if (AnyObject.GetComponent<PlayerCtrl>().FinalBoss == 0)
        {
            AnyObject.GetComponent<PlayerCtrl>().hp = 0;
            
        }
        
    }
    public void DispScore(int score)
    {
        totScore += score;

        //스코어 저장
        if (totScore >= 200 && Level == 1 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨2업기준 (레벨업 10까지만 가능하게 해보자)
        {
            Level = 2;
            maeScore = 200;
            tsugiScore = 500;
            
            AnyObject.GetComponent<PlayerCtrl>().Level = 2;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수 
        }
        if (totScore >= 500 && Level == 2 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨3업기준
        {
            Level = 3;
            maeScore = 500;
            tsugiScore = 1000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 3;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 1000 && Level == 3 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨4업기준
        {
            Level = 4;
            maeScore = 1000;
            tsugiScore = 1600;

            AnyObject.GetComponent<PlayerCtrl>().Level = 4;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 1600 && Level == 4 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨5업기준
        {
            Level = 5;
            maeScore = 1600;
            tsugiScore = 2500;

            AnyObject.GetComponent<PlayerCtrl>().Level = 5;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 2500 && Level == 5 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨6업기준
        {
            Level = 6;
            maeScore = 2500;
            tsugiScore = 3500;

            AnyObject.GetComponent<PlayerCtrl>().Level = 6;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 3500 && Level == 6 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨7업기준
        {
            Level = 7;
            maeScore = 3500;
            tsugiScore = 5000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 7;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 5000 && Level == 7 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨8업기준 
        {
            Level = 8;
            maeScore = 5000;
            tsugiScore = 7000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 8;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 7000 && Level == 8 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨9업기준
        {
            Level = 9;
            maeScore = 7000;
            tsugiScore = 9000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 9;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 9000 && Level == 9 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨10업기준
        {
            Level = 10;
            maeScore = 9000;
            tsugiScore = 12000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 10;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 12000 && Level == 10 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨10업기준
        {
            Level = 11;
            maeScore = 12000;
            tsugiScore = 16000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 10;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 16000 && Level == 11 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨10업기준
        {
            Level = 12;
            maeScore = 16000;
            tsugiScore = 21000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 10;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 21000 && Level == 12 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨10업기준
        {
            Level = 13;
            maeScore = 21000;
            tsugiScore = 27000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 10;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 27000 && Level == 13 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨10업기준
        {
            Level = 14;
            maeScore = 27000;
            tsugiScore = 35000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 10;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }
        if (totScore >= 35000 && Level == 14 && AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//레벨10업기준
        {
            Level = 15;
            maeScore = 35000;
            tsugiScore = 35000;

            AnyObject.GetComponent<PlayerCtrl>().Level = 10;//진짜레벨을 1을 늘림
            AnyObject.GetComponent<PlayerCtrl>().LevelUp = 1;//능력치를 늘리기위한 변수
        }

        //txtScore.text = "SCORE <color=#ffff00>" + totScore.ToString() + "</color>";
        txtLevel.text = "Level <color=#ffff00>" + Level.ToString() + "</color>";
        if(Level != 15 || AnyObject.GetComponent<PlayerCtrl>().PlayerDead == 0)//플레이어가 살아있거나 레벨이 만렙이 아니라면
        { 
        EXPbar.value= ((float)totScore - (float)maeScore) / ((float)tsugiScore - (float)maeScore);//Image UI 항목의 fillAmount 속성을 조절해 생명 게이지 값 조절
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
