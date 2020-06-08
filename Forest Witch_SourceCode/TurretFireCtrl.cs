using UnityEngine;
using System.Collections;

public class TurretFireCtrl : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;
    public AudioClip fireSfx;
    public MeshRenderer _renderer;
    public GameObject IsFire;
    
    void Start()
    {
  
        _renderer.enabled = false;
    
    }


    void Update()
    {
        if (IsFire.GetComponent<TurretCtrl>().fire == 1)
        {
            Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);
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
        float _scale = Random.Range(1.0f, 2.0f);
        _renderer.transform.localScale = Vector3.one * _scale;
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
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
