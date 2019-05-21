using UnityEngine;
using System.Collections;

public class MyGizmo : MonoBehaviour {
	public Color _color  = Color.red;
	public float _radius = 1.0f;
	
	void OnDrawGizmos (){
		//기즈모 색상 설정
		Gizmos.color = _color;
		//구체 모양의 기즈모 생성. 인수는 (생성위치, 반지름)
		Gizmos.DrawSphere ( transform.position, _radius );
	}
}