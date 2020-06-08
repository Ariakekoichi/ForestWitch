using UnityEngine;
using UnityEngine.UI;//UIコンポーネントに接近するため
using System.Collections;

public class MonsterHPBar : MonoBehaviour
{
    public Slider HPBar;
    public GameObject HeadUpPosition;
    private MonsterCtrl MonsterHP;


    void Start()
    {
        MonsterHP = gameObject.GetComponent<MonsterCtrl>();//HPの情報をを持ってくるための
    }
    void Update()
    {
        HPBar.value = (float)MonsterHP.hp / (float)MonsterHP.initHp;
        HPBar.transform.position = HeadUpPosition.transform.position;
    }
}
