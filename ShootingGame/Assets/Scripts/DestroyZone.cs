using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  private void OnTriggerEnter(Collider collision)
  {
    if (collision.gameObject.name.Contains("Bullet") || collision.gameObject.name.Contains("Enemy"))
    {
      collision.gameObject.SetActive(false);
      if (collision.gameObject.name.Contains("Bullet"))
      {
        PlayerFire player = GameObject.Find("Player").GetComponent<PlayerFire>();
        player.bulletObjectPool.Add(collision.gameObject);
      }
      else if (collision.gameObject.name.Contains("Enemy"))
      {
        EnemyManager enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        enemyManager.enemyObjectPool.Add(collision.gameObject);
      }
    }
    else
    {
      Destroy(collision.gameObject);
    }
  }
}
