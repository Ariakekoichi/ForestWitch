using UnityEngine;
using System.Collections;

public class GameMgr2 : MonoBehaviour
{

    public Transform[] points; //モンスターが出現する位置情報を持ってる配列
    public GameObject monsterPrefab;//モンスタープレハブを割り当てる変数
    public GameObject[] monsterPool;//モンスターを予め生成する配列
    public float createTime = 2.0f;//モンスターが出没する周期
    public int maxMonster = 6;//モンスターの最大発生個数
    public bool isGameOver = false;//ゲーム終了してるかしてないか
    
    void Start()
    {
        points = GameObject.Find("SpawnPoint2").GetComponentsInChildren<Transform>();
        monsterPool = new GameObject[maxMonster];
        for (int i = 0; i < maxMonster; i++)
        {
            monsterPool[i] = (GameObject)Instantiate(monsterPrefab);
            monsterPool[i].name = "Monster_" + i.ToString();
            monsterPool[i].SetActive(false);
        }

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateMonster());
        }


    }
    IEnumerator CreateMonster()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(createTime);
            foreach (GameObject obj in monsterPool)
            {
                if (obj.activeSelf == false)
                {
                    int idx = Random.Range(1, points.Length);
                    obj.transform.position = points[idx].position += new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
                    obj.SetActive(true);
                    break;
                }

            }

        }
    }
}

