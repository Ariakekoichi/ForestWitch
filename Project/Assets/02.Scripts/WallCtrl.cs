using UnityEngine;
using System.Collections;

public class WallCtrl : MonoBehaviour {
	//스파크 파티클 프리팹 연결할 변수
	public GameObject sparkEffect;
    public GameObject sparkEffect1;
    public GameObject sparkEffect2;

    //충돌 시작할때 발생하는 이벤트
    void OnCollisionEnter ( Collision coll )
	{
		//충돌한 게임오브젝트의 태그값 비교
		if (coll.collider.tag == "BULLET")
		{
			//스파크 파티클 동적으로 생성 후 변수에 할당
			Object obj = Instantiate ( sparkEffect
			                          , coll.transform.position
			                          , Quaternion.identity );
			//2초 후에 스파크 파티클 삭제
			Destroy ( obj, 2.0f );


			//충돌한 게임오브젝트 삭제
			Destroy ( coll.gameObject );
		}
        if (coll.collider.tag == "FireBall")
        {
            /* Object obj = Instantiate(sparkEffect
                                        , coll.transform.position
                                        , Quaternion.identity);
             Destroy(coll.gameObject, 2.0f);*/
        }
        if (coll.collider.tag == "SuperFireBall")
        {
           

            //충돌한 게임오브젝트 삭제
          //  Destroy(coll.gameObject);
        }

    }
}

