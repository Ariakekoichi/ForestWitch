using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	public Transform target;
	public float dist = 5.0f;//カメラとの一定距離
	public float height = 2.0f;//カメラの高さ設定
	public float dampRotate = 5.0f; //滑らかな回転のための変数
    public int CameraKey = 0;

	public Transform tr;//カメラ自身のTransformの変数

    void Update()
		{
			if (Input.GetKey(KeyCode.Escape))//ESCを押すとゲーム終了
			{
				Application.Quit();
			}
            if (Input.GetKeyDown(KeyCode.C))//Cを押すとカメラ角度変化。。坂の上の敵を見るために導入
        {
                CameraMoving();
            }
    }
	
		void Start () {
			tr = GetComponent<Transform>();
		}
	

		void LateUpdate () {
			float currYAngle = Mathf.LerpAngle (tr.eulerAngles.y, target.eulerAngles.y, dampRotate * Time.deltaTime);
			Quaternion rot = Quaternion.Euler(0,currYAngle,0);
           
            tr.position = target.position - (rot * Vector3.forward * dist) + (Vector3.up * height); //distぐらい後ろに配置し、heightぐらい上に上げる
                                                                                                   
        tr.LookAt(target); //カメラがターゲットオブジェクトを見るように設定

    }
    void CameraMoving()
    {
        if(CameraKey == 0)
        {
            dist += 2.5f;
            CameraKey = 1;
        }
        else
        {
            dist -= 2.5f;
            CameraKey = 0;
        }
    }
}
