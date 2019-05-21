using UnityEngine;
using System.Collections;

public class SuperTurretFireCtrl : MonoBehaviour
{
    //총알 프리팹
    public GameObject bullet;
    //총알 발사좌표
    public Transform firePos;
    //총알 발사 사운드 
    public AudioClip fireSfx;
    //MuzzleFlash의 MeshRenderer 컴포넌트 연결변수
  //  public MeshRenderer _renderer;
    public GameObject IsFire;
    
    void Start()
    {
        //최초에 MuzzleFlash MeshRenderer를 비활성화
 //       _renderer.enabled = false;
    }


    void Update()
    {
        //Ray를 시각적으로 표시하기 위해 사용

        

        if (IsFire.GetComponent<SuperTurretCtrl>().fire == 1)
        {
            Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);
            Fire();//총알발사
            
        }
    }

    void Fire()
    {
        //병렬 처리를 위한 코루틴 함수 호출
        StartCoroutine(this.CreateBullet());
    }

    
    // 코루틴 함수
    IEnumerator CreateBullet()
    {
        //Bullet 프리팹을 동적으로 생성
        Instantiate(bullet, firePos.position, firePos.rotation);

        yield return null;

    }

}
