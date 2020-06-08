using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    GameObject Mycamera;
   
    int i = 1;
    void Start()
    {
        Mycamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()// Update is called once per frame
    {
        if (Input.GetKeyDown(KeyCode.Tab))//Tabを押したら
        {
            changeCameraMode();//カメラが変わる
        }
        
    }
    private void changeCameraMode()//カメラが変わる
    {
        if (i == 1)//最初の設定
        {
            Mycamera.transform.localPosition = new Vector3(0, 1.5f, -4);//カメラの位置を後退
            i = 0;
        }
        else
        {                                           
            Mycamera.transform.localPosition = new Vector3(0, 1, 0);
            i = 1;//カメラの位置を元通り
        }
    }
    
}
    
    
