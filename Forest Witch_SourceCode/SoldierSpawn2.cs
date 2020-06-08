using UnityEngine;
using System.Collections;

public class SoldierSpawn2 : MonoBehaviour
{

    public Transform[] points; //モンスターが出現する位置情報を持ってる配列
    public GameObject monsterPrefab;//モンスタープレハブを割り当てる変数
    public GameObject[] monsterPool;//モンスターを予め生成する配列

    public float createTime = 5.0f;//モンスターが出没する周期
    public int maxMonster = 5;//モンスターの最大発生個数
    public bool isGameOver = false;//ゲーム終了してるかしてないか


    void Start()
    {
        points = GameObject.Find("SoldierSpawnPoint2").GetComponentsInChildren<Transform>();//SoldierSpawnPoint2を探し、下位にあるすべての<Transform>コンポーネントを探す。
        monsterPool = new GameObject[maxMonster];//モンスターをフルで使用する配列を割り当てる
        for (int i = 0; i < maxMonster; i++)
        {
            monsterPool[i] = (GameObject)Instantiate(monsterPrefab);//モンスタープレハブを生成し配列にセーブ
            monsterPool[i].name = "Soldier" + i.ToString();//生成したモンスターの名前設定
            monsterPool[i].SetActive(false);//生成したモンスターの非活性化
        }

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateMonster());//モンスター生成コルーチン関数号出
        }


    }
    IEnumerator CreateMonster()//モンスター生成コルーチン関数
    {
        while (!isGameOver)//ゲーム終了時まで無限ループ
        {
            yield return new WaitForSeconds(createTime);//モンスターの生成周期時間の間は待機
            foreach (GameObject obj in monsterPool)//オブジェクトの配列を最初から最後まで順次的に巡回
            {
                if (obj.activeSelf == false)
                {
                    int idx = Random.Range(1, points.Length);//モンスターが生成される位置
                    obj.transform.position = points[idx].position += new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));//モンスターが生成される位置を一定の範囲内でランダムで
                    obj.SetActive(true);//モンスタープレハブを活性化した後、forループを抜ける

                    break;
                }
            }

        }
    }
}
