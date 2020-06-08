using UnityEngine;
using System.Collections;

public class FinalBossFireCtrl : MonoBehaviour
{
    public GameObject bullet;//FinalBossの一般攻撃のプレハブ
    public GameObject SuperFireBall;//FinalBossの必殺技のプレハブ
    public Transform firePos;//発射座標
    public AudioClip fireSfx;
    public MeshRenderer _renderer;
    public GameObject IsFire;

    void Start()
    {
       
    }


    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);//Rayを視覚的に使う


        if (IsFire.GetComponent<FinalBossCtrl>().fire == 1)
        {
            Fire();
        }
        if (IsFire.GetComponent<FinalBossCtrl>().SuperFire == 5)//SuperFireが5の値になったら0にする。
        {
            IsFire.GetComponent<FinalBossCtrl>().SuperFire = 0;
            Invoke("SuperFire", 0.3f);
        }
    }

    void Fire()
    {
        StartCoroutine(this.CreateBullet());
    }
    void SuperFire()
    {
        StartCoroutine(this.CreateSuperFireBallBullet());
    }

    IEnumerator ShowMuzzleFlash()
    {
        _renderer.enabled = true;
       
        float _scale = Random.Range(1.0f, 2.0f);//MuzzleFlashのスケールを不規則的に変更
        _renderer.transform.localScale = Vector3.one * _scale;
       Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));//MuzzleFlashをｚ軸基準で不規則的に回転
        _renderer.transform.localRotation = rot;
       yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));

        _renderer.enabled = false;
    }
    IEnumerator CreateBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);//Bulletプレハブを動的に生成
        yield return null;
    }
    IEnumerator CreateSuperFireBallBullet()
    {
        Instantiate(SuperFireBall, firePos.position, firePos.rotation);//Bulletプレハブを動的に生成
        yield return null;
    }
}
