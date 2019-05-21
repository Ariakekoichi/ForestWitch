using UnityEngine;
using System.Collections;

public class ChukanBossFireCtrl : MonoBehaviour
{
    //총알 프리팹
    public GameObject bullet;
    //총알 발사좌표
    public Transform firePos;
    //총알 발사 사운드 
    public AudioClip fireSfx;
    //MuzzleFlash의 MeshRenderer 컴포넌트 연결변수
    public MeshRenderer _renderer;
    public GameObject IsFire;

    void Start()
    {
        //최초에 MuzzleFlash MeshRenderer를 비활성화
       // _renderer.enabled = false;// 너무 버그가 많이나서 일시적으로 비활성화시킴
        //IsFire = GameObject.Find("Soldier");
    }


    void Update()
    {
        //Ray를 시각적으로 표시하기 위해 사용
        Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);


        if (IsFire.GetComponent<ChukanBossCtrl>().fire == 1)
        {

            Fire();


        }
    }

    void Fire()
    {
        //병렬 처리를 위한 코루틴 함수 호출
       // StartCoroutine(this.ShowMuzzleFlash());
        StartCoroutine(this.CreateBullet());
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

}
