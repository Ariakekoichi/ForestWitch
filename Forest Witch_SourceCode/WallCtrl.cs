using UnityEngine;
using System.Collections;

public class WallCtrl : MonoBehaviour {
	public GameObject sparkEffect;
    public GameObject sparkEffect1;
    public GameObject sparkEffect2;
    
    void OnCollisionEnter ( Collision coll )//衝突するときに発生するイベント
	{
		if (coll.collider.tag == "BULLET")//衝突したゲームオブジェクトのタグ確認
        {
			
			Object obj = Instantiate ( sparkEffect
			                          , coll.transform.position
			                          , Quaternion.identity );//スパークパーティクル動的に生成後変数に割り当て
            Destroy ( obj, 2.0f );//2秒後スパークパーティクル削除
            Destroy ( coll.gameObject );//衝突したゲームオブジェクト削除
        }
       

    }
}

