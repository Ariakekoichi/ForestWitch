using UnityEngine;
using System.Collections;

public class FinalBossBulletCtrl : MonoBehaviour
{
    public int damage = 2;//銃弾の破壊力
    public float speed = 1000.0f;//銃弾の発射速度
    public GameObject bullet;
    public Vector3 firePos = Vector3.zero; //発射原点

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        firePos = transform.position;
        Object.Destroy(bullet, 0.85f);//0.85秒後銃弾削除


    }
}