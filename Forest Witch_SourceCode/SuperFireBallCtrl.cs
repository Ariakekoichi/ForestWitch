using UnityEngine;
using System.Collections;

public class SuperFireBallCtrl : MonoBehaviour
{
    
    public GameObject expEffect;//爆発効果連結変数
    public GameObject SuperFireBall;//スーパーファイアーボール連結変数
    public GameObject MonsterHigai;//モンスターにダメージを与えるため
    public GameObject SoldierHigai;//ソルジャーにダメージを与えるため
    public GameObject TurretHigai;//Turretにダメージを与えるため。。実際は機能をしません。
    public GameObject Damage;//Playerの攻撃力
    private Transform tr;

    public float speed = 1000.0f;
    public GameObject bullet;
    public GameObject FireBall;//一般攻撃連結変数
    public GameObject sparkEffect;//スパーク効果
    
    public Vector3 firePos = Vector3.zero;

    
    void Start()
    {
        tr = GetComponent<Transform>();
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        firePos = transform.position;
        StartCoroutine("SizenExplosion", 0);


        Damage = GameObject.Find("Player");
    }

 
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.name == "Floor (1)" ||
            coll.gameObject.name == "Floor (2)" ||
            coll.gameObject.name == "Floor (3)" ||
            coll.gameObject.name == "Floor (4)" ||
            coll.gameObject.name == "Floor (5)" ||
            coll.gameObject.name == "Floor (6)" ||
            coll.gameObject.name == "Floor (7)" ||
            coll.gameObject.name == "Floor (8)" ||
            coll.gameObject.name == "Floor" ||
            coll.gameObject.name == "Kaidan" ||
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
            coll.gameObject.name == "Turret" ||
            coll.gameObject.name == "SuperTurret" ||
            coll.gameObject.name == "FinalBoss" ||
            coll.gameObject.name == "Dragon" ||
            coll.gameObject.name == "ChukanBoss")
        {
            StartCoroutine(this.Explosion());
            
            Destroy(SuperFireBall);
        }
    }

 
    IEnumerator Explosion()
    {
   
        Instantiate(expEffect, tr.position, Quaternion.identity);
		Collider[] colls = Physics.OverlapSphere(tr.position, 5.0f);//ポジションを中心に5.0fの範囲内にいるColliderオブジェクトの抽出


        foreach (Collider coll in colls)
        {
            if (coll.gameObject.name == "Monster_0" ||
            coll.gameObject.name == "Monster_1" ||
            coll.gameObject.name == "Monster_2" ||
            coll.gameObject.name == "Monster_3" ||
            coll.gameObject.name == "Monster_4" ||
            coll.gameObject.name == "Monster_5" ||
            coll.gameObject.name == "Monster_6" ||
            coll.gameObject.name == "Monster_7" ||
            coll.gameObject.name == "Monster_8" ||
            coll.gameObject.name == "Monster_9")
            {
                coll.GetComponent<MonsterCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
                
            }
            else if (coll.gameObject.name == "Soldier0" ||
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
            coll.gameObject.name == "Soldier17" )
            {
                coll.GetComponent<SoldierCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
            }
            else if (coll.gameObject.name == "Turret")
            {
                coll.GetComponent<TurretCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
            }
            else if (coll.gameObject.name == "SuperTurret")
            {
                coll.GetComponent<SuperTurretCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
            }
            else if (coll.gameObject.name == "FinalBoss")
            {
                coll.GetComponent<FinalBossCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
                
            }
            else if (coll.gameObject.name == "Dragon")
            {
                coll.GetComponent<DragonCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
            }
            else if (coll.gameObject.name == "ChukanBoss")
            {
                coll.GetComponent<ChukanBossCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
            }


            Destroy(SuperFireBall);
        }
        yield return null;
    }
    IEnumerator SizenExplosion()
    {
        yield return new WaitForSeconds(0.75f);//0.75秒後爆発
        Instantiate(expEffect, tr.position, Quaternion.identity);
        Collider[] colls = Physics.OverlapSphere(tr.position, 5.0f); //ポジションを中心に5.0fの範囲内にいるColliderオブジェクトの抽出

        foreach (Collider coll in colls)
        {
            if (coll.gameObject.name == "Monster_0" ||
            coll.gameObject.name == "Monster_1" ||
            coll.gameObject.name == "Monster_2" ||
            coll.gameObject.name == "Monster_3" ||
            coll.gameObject.name == "Monster_4" ||
            coll.gameObject.name == "Monster_5" ||
            coll.gameObject.name == "Monster_6" ||
            coll.gameObject.name == "Monster_7" ||
            coll.gameObject.name == "Monster_8" ||
            coll.gameObject.name == "Monster_9")
            {
                coll.GetComponent<MonsterCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
                if (coll.GetComponent<MonsterCtrl>().hp <= 0)
                {

                }
            }
            if (coll.gameObject.name == "Soldier0" ||
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
            coll.gameObject.name == "Soldier17")
            {
                coll.GetComponent<SoldierCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
            }
            if (coll.gameObject.name == "Turret")
            {
                coll.GetComponent<TurretCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
            }
            if (coll.gameObject.name == "SuperTurret")
            {
                coll.GetComponent<SuperTurretCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
            }
            if (coll.gameObject.name == "FinalBoss")
            {
                coll.GetComponent<FinalBossCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
            }
            if (coll.gameObject.name == "Dragon")
            {
                coll.GetComponent<DragonCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;
            }
            if (coll.gameObject.name == "ChukanBoss")
            {
                coll.GetComponent<ChukanBossCtrl>().hp -= Damage.GetComponent<PlayerCtrl>().Level * 10;

            }

        }
        Destroy(SuperFireBall);
        yield return null;
    }
}
