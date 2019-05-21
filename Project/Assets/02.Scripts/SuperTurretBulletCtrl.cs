﻿using UnityEngine;
using System.Collections;

public class SuperTurretBulletCtrl : MonoBehaviour
{
    //총알의 파괴력
    public int damage = 20;
    //총알 발사 속도
    public float speed = 1000.0f;
    public GameObject bullet;

    //발사 원점
    public Vector3 firePos = Vector3.zero;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        //총알이 생성된 위치가 FirePos 위치와 같음
        firePos = transform.position;
        Object.Destroy(bullet, 0.85f);//0.85초후 총알삭제
    }


}
