using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
  public GameObject bombEffect;

  public int attackPower = 10;

  public float explosionRadius = 5f;

  private void OnCollisionEnter(Collision collision)
  {
    // 3 번째 파라미터는 레이어의 번호입니다.
    Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1<<8);

    for (int i = 0; i < cols.Length; i++)
    {
      cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
    }

    GameObject bEffect = Instantiate(bombEffect);
    bEffect.transform.position = transform.position;
    Destroy(gameObject);
  }
}
