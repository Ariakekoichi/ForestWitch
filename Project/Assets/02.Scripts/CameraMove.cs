using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    GameObject Mycamera;
   
    int i = 1;
  
    // Use this for initialization
    void Start()
    {
        Mycamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))//탭울 누른다
        {
            changeCameraMode();//카메라를 바꾸기
        }
        
    }
    private void changeCameraMode()//카메라바꾸기
    {
        if (i == 1)//맨처음설정
        {
            Mycamera.transform.localPosition = new Vector3(0, 1.5f, -4);//카메라를 후퇴(3인칭)
            i = 0;
        }
        else
        {                                           
            Mycamera.transform.localPosition = new Vector3(0, 1, 0);
            i = 1;//카메라위치를 원래대로(1인칭)
        }
    }
    
}
    
    
