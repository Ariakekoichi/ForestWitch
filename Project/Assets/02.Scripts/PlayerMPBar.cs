using UnityEngine;
using UnityEngine.UI;//UI컴포넌트에 접근하기 위해 추가한 네임스페이스
using System.Collections;

public class PlayerMPBar : MonoBehaviour
{
    public Slider MPBar;
    public GameObject HeadUpPosition;


    private PlayerCtrl PlayerMP;//Player의 HP정보를 가져오기위한 변수
    

    void Start()
    {
        PlayerMP = gameObject.GetComponent<PlayerCtrl>();//HP정보를 가져오기위해서
    }
    void Update()
    {
        MPBar.value = (float)PlayerMP.mp / (float)PlayerMP.initMp;

    }
}
