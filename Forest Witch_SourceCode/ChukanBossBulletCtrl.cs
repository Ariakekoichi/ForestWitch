using UnityEngine;
using System.Collections;

public class ChukanBossBulletCtrl : MonoBehaviour
{
    
    public int damage = 20;//銃弾の破壊力
    public float speed = 1000.0f;//銃弾の発射速度
    public GameObject bullet;
    public Vector3 firePos = Vector3.zero;//銃弾発射原点

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);//銃弾の速度
        firePos = transform.position;//銃弾が生成された一がFirePosの位置と同じ
        Object.Destroy(bullet, 0.75f);//0.75秒後銃弾削除


    }
}