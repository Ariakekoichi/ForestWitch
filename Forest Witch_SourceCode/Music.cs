using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
	private int Mute = 0;
	public AudioSource BGMSource;
	private int loadIndex;
	private static int loadCount;
    
	void Start () {
		BGMSource = GetComponent<AudioSource>();

	}
	void Awake(){
		loadIndex = loadCount;
		loadCount++;
		if (loadIndex == 0) {
			DontDestroyOnLoad(gameObject);// 最初にロードされた時にすることをここに作成

        } else {
            Destroy(gameObject);//ここで既存SCENEにあるオブジェクトを除去する。
        }

	}
	
	void Update ()
    {// Update is called once per frame



        if (Input.GetKeyDown(KeyCode.P))//pを押すとBGMが鳴る
		{
			BGM();
		}
	
	}
	void BGM()//BGM処理
	{
		if (Mute == 0)//最初の設定
		{
			BGMSource.volume = 0;
			Mute = 1;//ミュートモードに
		}
		else
		{
			BGMSource.volume = 0.1f;
			Mute = 0;//再生モードに
		}

	}
}
