using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
  Vector3 dir;
  public float speed = 5f;
  public GameObject explosionFactory;

  // Start is called before the first frame update
  void Start()
  {

  }

  void OnEnable()
  {
    int randValue = UnityEngine.Random.Range(0, 10);

    if (randValue < 3)
    {
      GameObject target = GameObject.Find("Player");
      dir = target.transform.position - transform.position;
      dir.Normalize();
    }
    else
    {
      dir = Vector3.down;
    }
  }

  // Update is called once per frame
  void Update()
  {
    //Vector3 dir = Vector3.down;
    transform.position += dir * speed * Time.deltaTime;
  }

  private void OnCollisionEnter(Collision collision)
  {
    // ���� ���� ǥ��
    /*
    GameObject smObject = GameObject.Find("ScoreManager");
    ScoreManager sm = smObject.GetComponent<ScoreManager>();
    sm.SetScore(sm.GetScore() + 1);
    */
    // �̱��� ��ü Ȱ��
    ScoreManager.Instance.Score++;


    // ���� ȿ�� ����
    GameObject explosion = Instantiate(explosionFactory);
    explosion.transform.position = transform.position;
    explosion.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    // �浹�ϴ� ���ӿ�����Ʈ �Ҹ�
    //Destroy(collision.gameObject); 
    // ������Ʈ Ǯ ����(�Ѿ�)
    if (collision.gameObject.name.Contains("Bullet"))
    {
      collision.gameObject.SetActive(false);
      PlayerFire player = GameObject.Find("Player").GetComponent<PlayerFire>();
      player.bulletObjectPool.Add(collision.gameObject);
    }
    else
    {
      Destroy(collision.gameObject);
    }

    //Destroy(gameObject);
    gameObject.SetActive(false);
    EnemyManager enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    enemyManager.enemyObjectPool.Add(gameObject);
  }
}
