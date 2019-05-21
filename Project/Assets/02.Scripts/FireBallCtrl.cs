using UnityEngine;
using System.Collections;
using System;

public class FireBallCtrl : MonoBehaviour
{

    //총알 발사 속도
    public float speed = 1000.0f;
    
    public GameObject FireBall;//슈퍼파이어볼연결변수
    public GameObject sparkEffect;//스파크이펙트 불러오기
    private Transform tr;
    public GameObject Player;

    //발사 원점
    public Vector3 firePos = Vector3.zero;

    void Start()
    {
        tr = GetComponent<Transform>();
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        //총알이 생성된 위치가 FirePos 위치와 같음
        firePos = transform.position;
        GameObject.Destroy(FireBall, 0.75f);//0.75초후 총알삭제
        

    }
    
      //충돌시 발생하는 콜백함수(CallBack Function)
      void OnCollisionEnter(Collision coll)
      {
          if (coll.gameObject.name == "Kaidan" ||
              coll.gameObject.name == "Floor" ||
              coll.gameObject.name == "Barrel" ||
              coll.gameObject.name == "Monster_0" ||
              coll.gameObject.name == "Monster_1" ||
              coll.gameObject.name == "Monster_2" ||
              coll.gameObject.name == "Monster_3" ||
              coll.gameObject.name == "Monster_4" ||
              coll.gameObject.name == "Monster_5" ||
              coll.gameObject.name == "Monster_6" ||
              coll.gameObject.name == "Monster_7" ||
              coll.gameObject.name == "Monster_8" ||
              coll.gameObject.name == "Monster_9" ||
              coll.gameObject.name == "Monster_10" ||
              coll.gameObject.name == "Monster_11" ||
              coll.gameObject.name == "Monster_12" ||
              coll.gameObject.name == "Monster_13" ||
              coll.gameObject.name == "Monster" ||
              coll.gameObject.name == "Soldier0" ||
              coll.gameObject.name == "Soldier1" ||
              coll.gameObject.name == "Soldier2" ||
              coll.gameObject.name == "Soldier3" ||
              coll.gameObject.name == "Soldier4" ||
              coll.gameObject.name == "Soldier5" ||
              coll.gameObject.name == "Soldier6" ||
              coll.gameObject.name == "Soldier7" ||
              coll.gameObject.name == "Soldier8" ||
              coll.gameObject.name == "Soldier9" ||
              coll.gameObject.name == "Soldier10" ||
              coll.gameObject.name == "Soldier11" ||
              coll.gameObject.name == "Soldier12" ||
              coll.gameObject.name == "Soldier13" ||
              coll.gameObject.name == "Soldier14" ||
              coll.gameObject.name == "Soldier15" ||
              coll.gameObject.name == "Soldier16" ||
              coll.gameObject.name == "Soldier17" ||
              coll.gameObject.name == "Soldier" ||
              coll.gameObject.name == "Turret" ||
              coll.gameObject.name == "SuperTurret" ||
              coll.gameObject.name == "FinalBoss" ||
              coll.gameObject.name == "Dragon" ||
              coll.gameObject.name == "ChukanBoss")
          {
            StartCoroutine(this.Explosion());
            Destroy(FireBall);
        }
      }
      


    IEnumerator Explosion()
    {
        //폭발효과 파티클 생성
        GameObject blood = (GameObject)Instantiate(sparkEffect, tr.position, Quaternion.identity);
        Destroy(blood, 2.0f);
        //sparkEffect.SetActive(false);
        yield return null;
    }
}