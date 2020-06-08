using UnityEngine;
using System.Collections;
using System;

public class FireBallCtrl : MonoBehaviour
{
    
    public float speed = 1000.0f;//銃弾速度
    
    public GameObject FireBall;//スーパーファイアーボール連結変数
    public GameObject sparkEffect;//スパーク効果
    private Transform tr;
    public GameObject Player;
    
    public Vector3 firePos = Vector3.zero;

    void Start()
    {
        tr = GetComponent<Transform>();
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        firePos = transform.position;
        GameObject.Destroy(FireBall, 0.75f);//0.75秒後銃弾削除
        

    }
    
      void OnCollisionEnter(Collision coll)//追突時発生するコールバック関数
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
        GameObject blood = (GameObject)Instantiate(sparkEffect, tr.position, Quaternion.identity);//爆発効果
        Destroy(blood, 2.0f);
        yield return null;
    }
}