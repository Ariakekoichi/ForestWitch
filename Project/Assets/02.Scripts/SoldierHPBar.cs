using UnityEngine;
using UnityEngine.UI;//UI컴포넌트에 접근하기 위해 추가한 네임스페이스
using System.Collections;

public class SoldierHPBar : MonoBehaviour
{
    public Slider HPBar;
    public GameObject HeadUpPosition;  
    //private MonsterCtrl MonsterHP;//Monster의 HP정보를 가져오기위한 변수
    private SoldierCtrl SoldierHP;//Soldier의 HP정보를 가져오기위한 변수
    //private TurretCtrl TurretHP;//Soldier의 HP정보를 가져오기위한 변수


    void Start()
    {
        SoldierHP = gameObject.GetComponent<SoldierCtrl>();//HP정보를 가져오기위해서
    }
    void Update()
    {
        HPBar.value = (float)SoldierHP.hp / (float)SoldierHP.initHp;
        HPBar.transform.position = HeadUpPosition.transform.position;
    }
}
