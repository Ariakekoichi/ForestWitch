using UnityEngine;
using System.Collections;

public class SuperFireBallCtrl : MonoBehaviour
{
    //폭발효과 파티클 연결변수.
    public GameObject expEffect;
    public GameObject SuperFireBall;//슈퍼파이어볼연결변수
    public GameObject MonsterHigai;//몬스터에게 데미지를 주기위한
    public GameObject SoldierHigai;//솔져에게 데미지를 주기위한
    public GameObject TurretHigai;//터렛에게 데미지를 주기위한
    public GameObject Damage;//Player의 공격력
    private Transform tr;

    public float speed = 1000.0f;
    public GameObject bullet;
    public GameObject FireBall;//슈퍼파이어볼연결변수
    public GameObject sparkEffect;//스파크이펙트 불러오기

    //발사 원점
    public Vector3 firePos = Vector3.zero;

    
    void Start()
    {
        tr = GetComponent<Transform>();
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        //총알이 생성된 위치가 FirePos 위치와 같음
        firePos = transform.position;
        StartCoroutine("SizenExplosion", 0);//0.75초후 폭발
       
        
        Damage = GameObject.Find("Player");
    }

    //충돌시 발생하는 콜백함수(CallBack Function)
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
            /*
            
            */
        }
    }

    //폭발시킬 코루틴 함수
    IEnumerator Explosion()
    {
        //폭발효과 파티클 생성
        Instantiate(expEffect, tr.position, Quaternion.identity);
        
        //지정한 원점을 중심으로 5.0f 반경내에 들어와있는 Collider 객체 추출
        Collider[] colls = Physics.OverlapSphere(tr.position, 5.0f);

        
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
        yield return new WaitForSeconds(0.75f);//0.75초후 폭발
        //폭발효과 파티클 생성
        Instantiate(expEffect, tr.position, Quaternion.identity);

        //지정한 원점을 중심으로 5.0f 반경내에 들어와있는 Collider 객체 추출
        Collider[] colls = Physics.OverlapSphere(tr.position, 5.0f);

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
