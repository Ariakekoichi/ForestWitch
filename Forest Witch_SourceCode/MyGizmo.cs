using UnityEngine;
using System.Collections;

public class MyGizmo : MonoBehaviour {
	public Color _color  = Color.red;
	public float _radius = 1.0f;
	
	void OnDrawGizmos (){
		Gizmos.color = _color;//Gizmosの色設定
        Gizmos.DrawSphere ( transform.position, _radius );//球体のGizmos生成、因数は（生成位置、半径）
    }
}