using UnityEngine;
using System.Collections;

public class GameMgr3 : MonoBehaviour
{

    public Transform[] points;
    public GameObject monsterPrefab;
    public GameObject[] monsterPool;

    public float createTime = 2.0f;
    public int maxMonster = 6;
    public bool isGameOver = false;
    

    void Start()
    {
        points = GameObject.Find("SpawnPoint3").GetComponentsInChildren<Transform>();
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

