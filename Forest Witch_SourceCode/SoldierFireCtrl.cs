using UnityEngine;
using System.Collections;

public class SoldierFireCtrl : MonoBehaviour
{
    public GameObject bullet;//銃弾プリフェップ
    public Transform firePos;//銃弾発車座標
    public AudioClip fireSfx;//銃弾発射サウンド
    public MeshRenderer _renderer;//MuzzleFlashの MeshRendererコンポーネント連結変数
    public GameObject IsFire;

    void Start()
    {
        _renderer.enabled = false;// ずっと光るバグが有って非活性化しています。
    }


    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);//Rayを視覚的に表示するように使用


        if (IsFire.GetComponent<SoldierCtrl>().fire == 1)
        {
            
            Fire();


        }
    }

    void Fire()
    {
        StartCoroutine(this.ShowMuzzleFlash());
        StartCoroutine(this.CreateBullet());
    }

    IEnumerator ShowMuzzleFlash()
    {
        _renderer.enabled = true;
        float _scale = Random.Range(1.0f, 2.0f);//MuzzleFlashの大きさを不規則的に変更
        _renderer.transform.localScale = Vector3.one * _scale;
       Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));//MuzzleFlashをZ軸基準で不規則的に回転
        _renderer.transform.localRotation = rot;
       yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));

        _renderer.enabled = false;
    }
    IEnumerator CreateBullet()
    {
    
        Instantiate(bullet, firePos.position, firePos.rotation);

        yield return null;

    }

}
