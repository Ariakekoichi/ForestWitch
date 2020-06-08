using UnityEngine;
using System.Collections;

public class DragonBulletCtrl : MonoBehaviour
{
    
    public int damage = 5;//銃弾の破壊力
    public float speed = 1000.0f; //銃弾発射速度
    public GameObject bullet;
    public Vector3 firePos = Vector3.zero;//発射原点
    
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed); //銃弾が生成された位置とFirePosの位置が同じ
        firePos = transform.position;
        Object.Destroy(bullet, 0.85f);//0.85秒後銃弾削除
    }
}