using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	public Transform target;
	public float dist = 5.0f;//카메라와의 일정거리
	public float height = 2.0f;//카메라의 높이설정
	public float dampRotate = 5.0f; //부드러운 회전을 위한 변수
    public int CameraKey = 0;

	public Transform tr;//카메라자신의 트랜스폼 변수

		void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				Application.Quit();
			}
            if (Input.GetKeyDown(KeyCode.C))
            {
                CameraMoving();
            }
    }
	// Use this for initialization
		void Start () {
			tr = GetComponent<Transform>();
		}
	
	// Update is called once per frame
		void LateUpdate () {
			float currYAngle = Mathf.LerpAngle (tr.eulerAngles.y, target.eulerAngles.y, dampRotate * Time.deltaTime);
			Quaternion rot = Quaternion.Euler(0,currYAngle,0);
            //dist만큼 뒤쪽으로배치하고 높이만큼 위로올림
            tr.position = target.position - (rot * Vector3.forward * dist) + (Vector3.up * height);
            //카메라가 타겟 오브젝트를 바라보게 설정
			tr.LookAt(target);
        
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
