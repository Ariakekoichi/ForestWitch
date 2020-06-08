using UnityEngine;
using System.Collections;

public class FinalBossSuperFireBallCtrl : MonoBehaviour
{
    public GameObject expEffect;//爆発効果連結変数
    public GameObject SuperFireBall;//スーパーファイアーボール連結変数
    public GameObject Damage;//Playerの攻撃力
    private Transform tr;

    public float speed = 1000.0f;
    public GameObject bullet;
    public GameObject FireBall;//一般攻撃連結変数
    public GameObject sparkEffect;//スパーク効果持ってくる
    
    public Vector3 firePos = Vector3.zero;//発射原点

    
    void Start()
    {
        tr = GetComponent<Transform>();
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        firePos = transform.position;//銃弾が生成される位置がFirePosの位置にする。
        StartCoroutine("SizenExplosion", 0);//0.5秒後爆発
        Damage = GameObject.Find("Player");
    }

    void OnCollisionEnter(Collision coll)//追突時発生する関数
    {
        if (coll.gameObject.name == "Player")
        {
            StartCoroutine(this.Explosion());
            Destroy(SuperFireBall);
            GetComponent<Rigidbody>().AddForce(transform.position);
        }
    }

    IEnumerator Explosion()//Playerに当たった時に爆発するコルーチン関数
    {
        Instantiate(expEffect, tr.position, Quaternion.identity);//爆発効果パーティクル生成
        Collider[] colls = Physics.OverlapSphere(tr.position, 4.0f);//指定した原点を中心として4.0f内に入ってる Collider オブジェクトの抽出

        foreach (Collider coll in colls)
        {
            if (coll.gameObject.name == "Player")
            {
                coll.GetComponent<PlayerCtrl>().hp -= 50;//PLAYERに50のダメージを与える
            }
        }
        Destroy(SuperFireBall);
        yield return null;
    }
    IEnumerator SizenExplosion()//Playerに直接当たらず爆発する時のコルーチン関数
    {
        yield return new WaitForSeconds(0.5f);//FINALBOSSから発車後0.5秒後爆発
        
        Instantiate(expEffect, tr.position, Quaternion.identity);//爆発効果Particle生成

        
        Collider[] colls = Physics.OverlapSphere(tr.position, 4.0f);//指定した原点を中心として4.0f内に入ってる Collider オブジェクトの抽出

        foreach (Collider coll in colls)
        {
            if (coll.gameObject.name == "Player")
            {
                coll.GetComponent<PlayerCtrl>().hp -= 50;//PLAYERに50のダメージを与える
            }
        }
        Destroy(SuperFireBall);
        yield return null;
    }
}
