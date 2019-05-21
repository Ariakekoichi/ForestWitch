using UnityEngine;
using System.Collections;

public class FinalBossFireCtrl : MonoBehaviour
{
    //총알 프리팹
    public GameObject bullet;
    public GameObject SuperFireBall;
    //총알 발사좌표
    public Transform firePos;
    //총알 발사 사운드 
    public AudioClip fireSfx;
    //MuzzleFlash의 MeshRenderer 컴포넌트 연결변수
    public MeshRenderer _renderer;
    public GameObject IsFire;

    void Start()
    {
       
    }


    void Update()
    {
        //Ray를 시각적으로 표시하기 위해 사용
        Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);

        
        if (IsFire.GetComponent<FinalBossCtrl>().fire == 1)
        {
            Fire();
        }
        if (IsFire.GetComponent<FinalBossCtrl>().SuperFire == 5)
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
       //머즐플래쉬 스케일을 불규칙하게 변경
        float _scale = Random.Range(1.0f, 2.0f);
        _renderer.transform.localScale = Vector3.one * _scale;

       //MuzzleFlash를 제트축기준으로 불규칙하게 회전
       Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
     _renderer.transform.localRotation = rot;


       //불규칙적인 시간동안 Delay한 다음 MeshRenderer를 비활성화
       yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));

        _renderer.enabled = false;
    }
    // 코루틴 함수
    IEnumerator CreateBullet()
    {
        //Bullet 프리팹을 동적으로 생성
        Instantiate(bullet, firePos.position, firePos.rotation);

        yield return null;
    }
    IEnumerator CreateSuperFireBallBullet()
    {
        //Bullet 프리팹을 동적으로 생성
        Instantiate(SuperFireBall, firePos.position, firePos.rotation);
        yield return null;
    }
}
