using UnityEngine;
using System.Collections;

public class SoldierSpawn3 : MonoBehaviour
{

    public Transform[] points; //몬스터가 출현할 위치를 담을 배열
    public GameObject monsterPrefab;//몬스터 프리팹을 할당할 변수
    public GameObject[] monsterPool;//몬스터를 미리 생성해 저장할 배열

    public float createTime = 5.0f;//몬스터를 발생시킬 주기
    public int maxMonster = 5;//몬스터 최대 발생개수
    public bool isGameOver = false;//게임종료 변수여부


    void Start()
    {
        points = GameObject.Find("SoldierSpawnPoint3").GetComponentsInChildren<Transform>();//SpawnPoint4를 찾아 하위에 있는 모든 트랜스폼 컴포넌트를 찾아옴
        monsterPool = new GameObject[maxMonster];//몬스터 풀로 사용할 배열을 할당한다.
        for (int i = 0; i < maxMonster; i++)
        {
            monsterPool[i] = (GameObject)Instantiate(monsterPrefab);//몬스터 프리팹을 생성해 배열에 저장
            monsterPool[i].name = "Soldier" + i.ToString();//생성한 몬스터의 이름 설정
            monsterPool[i].SetActive(false);//생성한 몬스터를 비활성화
        }

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateMonster());//몬스터 생성 코루틴 함수 호출
        }


    }
    IEnumerator CreateMonster()//몬스터 생성 코루틴 함수
    {
        while (!isGameOver)//게임 종료 시까지 무한 루프
        {
            yield return new WaitForSeconds(createTime);//몬스터의 생성주기 시간만큼 대기
            foreach (GameObject obj in monsterPool)//오브젝트 풀배열의 처음부터 끝까지 순차적으로 순회
            {
                if (obj.activeSelf == false)
                {
                    int idx = Random.Range(1, points.Length);//몬스터를 나타낼 위치를 산출
                    obj.transform.position = points[idx].position += new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));//몬스터 활성화
                    obj.SetActive(true);//오브젝트 풀에서 몬스터 프리팹 하나를 활성화한 후 for 루프를 빠져나감

                    break;
                }
            }

        }
    }
}
