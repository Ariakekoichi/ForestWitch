using UnityEngine;
using System.Collections;

public class TurretBulletCtrl : MonoBehaviour
{
  
    public int damage = 1;
    public float speed = 1000.0f;
    public GameObject bullet;
    public Vector3 firePos = Vector3.zero;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        firePos = transform.position;
        Object.Destroy(bullet, 0.75f);
    }


}
