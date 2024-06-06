using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
  float currentTime;

  float createTime;

  public float minTime = 1f;

  public float maxTime = 10f;

  public GameObject enemyFactory;

  // 에너미 오브젝트 풀 사용
  public List<GameObject> enemyObjectPool;
  public int poolSize = 10;
  public GameObject[] spawnPoints;


  // Start is called before the first frame update
  void Start()
  {
    createTime = UnityEngine.Random.Range(minTime, maxTime);

    // 에너미 오브젝트 풀
    enemyObjectPool = new List<GameObject>();
    for (int i = 0; i < poolSize; i++)
    {
      GameObject enemy = Instantiate(enemyFactory);
      enemyObjectPool.Add(enemy);
      enemy.SetActive(false);
    }

  }

  // Update is called once per frame
  void Update()
  {
    currentTime += Time.deltaTime;
    if (currentTime > createTime)
    {
      /*GameObject enemy = Instantiate(enemyFactory);
      enemy.transform.position = transform.position;*/
      // 에너미 오브젝트 풀 사용
      if (enemyObjectPool.Count > 0)
      {
        GameObject enemy = enemyObjectPool[0];
        int idx = Random.Range(0, spawnPoints.Length);
        enemy.transform.position = spawnPoints[idx].transform.position;
        enemy.SetActive(true);
        enemyObjectPool.Remove(enemy);
      }

      currentTime = 0;
      createTime = UnityEngine.Random.Range(minTime, maxTime);
    }
  }
}
