using UnityEngine;
using System.Collections;

public class FireCtrl : MonoBehaviour {
	public GameObject bullet;
	public Transform firePos;
	public AudioClip fireSfx;
	public MeshRenderer _renderer;
    public GameObject SuperFireBall;
    public GameObject Player;

    void Start()
    {
        _renderer.enabled = false;
        Player = GameObject.Find("Player");//Damageの値持ってくる
    }


    void Update () {
		Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);

		//マウス左クリックで Fire 関数発動
		if ( Input.GetMouseButtonDown(0)){//一般攻撃関連処理
            if (Player.GetComponent<PlayerCtrl>().mp >= 1)//MPが残り1以上あってからMPを1使って攻撃可能。
            {
                Player.GetComponent<PlayerCtrl>().mp -= 1;
                Player.GetComponent<PlayerCtrl>().MPkaifukutime = 0;
                Fire();
            }
                
        }
        //マウス右クリックで SuperFireBallFire 関数発動
        if (Input.GetMouseButtonDown(1))//必殺技関連処理
        {
                 if (Player.GetComponent<PlayerCtrl>().mp >= 10 && Player.GetComponent<PlayerCtrl>().Level >= 5)//レベル5以上、残りのMPが10以上あってからMPを10使って必殺技が発射可能
                   {
                Player.GetComponent<PlayerCtrl>().mp -= 10;
                Player.GetComponent<PlayerCtrl>().MPkaifukutime = 0;
                SuperFireBallFire();
            }
            
        }
    }
	
	void Fire(){
        StartCoroutine(this.ShowMuzzleFlash());
        StartCoroutine(this.CreateBullet());
    }

    void SuperFireBallFire()
    {
        StartCoroutine(this.CreateSuperFireBall());
    }

    IEnumerator ShowMuzzleFlash()
    {
        _renderer.enabled = true;
        float _scale = Random.Range(1.0f, 2.0f);
        _renderer.transform.localScale = Vector3.one * _scale;
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        _renderer.transform.localRotation = rot;
        yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
        
        _renderer.enabled = false;
    }
    IEnumerator CreateBullet(){
		Instantiate (bullet, firePos.position, firePos.rotation);
        yield return null;
        
	}
    IEnumerator CreateSuperFireBall()
    {
        Instantiate(SuperFireBall, firePos.position, firePos.rotation);
        yield return null;

    }

}
