using UnityEngine;
using System.Collections;

public class UIMgr : MonoBehaviour
{
	int isBGM = 0;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))//フルスクリーン変換関連
        {
            Screen.SetResolution(1280, 800, !Screen.fullScreen);
        }
        if (Input.GetKey(KeyCode.Escape))//Esc押せばゲーム終了
        {
            Application.Quit();
        }
        
    }
    public void JapanesePlay()
    {
        Application.LoadLevel("scPlay");
    }
    public void ChinesePlay()
    {
        Application.LoadLevel("ChinesescPlay");
    }
    public void KoreanPlay()
    {
        Application.LoadLevel("KoreanscPlay");
    }
    public void JapaneseMain()
    {
        Application.LoadLevel("scMain");
    }
    public void ChineseMain()
    {
        Application.LoadLevel("ChinesescMain");
    }
    public void KoreanMain()
    {
        Application.LoadLevel("KoreanscMain");
    }
    public void JapaneseHelp()
    {
        Application.LoadLevel("JPHelp");
    }
    public void ChineseHelp()
    {
        Application.LoadLevel("CHHelp");
    }
    public void KoreanHelp()
    {
        Application.LoadLevel("KRHelp");
    }
    public void JapaneseCredit()
    {
        Application.LoadLevel("JPCredit");
    }
    public void ChineseCredit()
    {
        Application.LoadLevel("CHCredit");
    }
    public void KoreanCredit()
    {
        Application.LoadLevel("KRCredit");
    }
    
}
