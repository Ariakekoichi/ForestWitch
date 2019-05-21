using UnityEngine;
using System.Collections;

public class FinalBossSuperFireBallCtrl : MonoBehaviour
{
    //폭발효과 파티클 연결변수.
    public GameObject expEffect;
    public GameObject SuperFireBall;//슈퍼파이어볼연결변수
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
        StartCoroutine("SizenExplosion", 0);//0.5초후 폭발
        Damage = GameObject.Find("Player");
    }

    //충돌시 발생하는 콜백함수(CallBack Function)
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.name == "Player")
        {
            StartCoroutine(this.Explosion());
            Destroy(SuperFireBall);
            GetComponent<Rigidbody>().AddForce(transform.position);
        }
    }

    //폭발시킬 코루틴 함수
    IEnumerator Explosion()
    {
        //폭발효과 파티클 생성
        Instantiate(expEffect, tr.position, Quaternion.identity);

        //지정한 원점을 중심으로 5.0f 반경내에 들어와있는 Collider 객체 추출
        Collider[] colls = Physics.OverlapSphere(tr.position, 4.0f);

        foreach (Collider coll in colls)
        {
            if (coll.gameObject.name == "Player")
            {
                coll.GetComponent<PlayerCtrl>().hp -= 50;
            }
        }
        Destroy(SuperFireBall);
        yield return null;
    }
    IEnumerator SizenExplosion()
    {
        yield return new WaitForSeconds(0.5f);//0.5초후 폭발
        //폭발효과 파티클 생성
        Instantiate(expEffect, tr.position, Quaternion.identity);

        //지정한 원점을 중심으로 4.0f 반경내에 들어와있는 Collider 객체 추출
        Collider[] colls = Physics.OverlapSphere(tr.position, 4.0f);

        foreach (Collider coll in colls)
        {
            if (coll.gameObject.name == "Player")
            {
                coll.GetComponent<PlayerCtrl>().hp -= 50;
            }
        }
        Destroy(SuperFireBall);
        yield return null;
    }
}
