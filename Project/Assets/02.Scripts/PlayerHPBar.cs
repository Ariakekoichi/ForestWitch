using UnityEngine;
using UnityEngine.UI;//UI컴포넌트에 접근하기 위해 추가한 네임스페이스
using System.Collections;

public class PlayerHPBar : MonoBehaviour
{
    public Slider HPBar;
    public GameObject HeadUpPosition;
    

    private PlayerCtrl PlayerHP;//Player의 HP정보를 가져오기위한 변수
   


    void Start()
    {
        PlayerHP = gameObject.GetComponent<PlayerCtrl>();//HP정보를 가져오기위해서
    }
    void Update()
    {
        HPBar.value = (float)PlayerHP.hp / (float)PlayerHP.initHp;
     
    }
}
