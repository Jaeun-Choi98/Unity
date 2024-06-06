using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
  // 총알을 생산하는 공장
  public GameObject bulletFactory;

  // 오브젝트 풀(총알) 이용
  public int poolSize = 10;
  public List<GameObject> bulletObjectPool;

  // 총알이 나가는 위치
  public GameObject firePosition;

  // Start is called before the first frame update
  void Start()
  {
    bulletObjectPool = new List<GameObject>();
    for (int i = 0; i < poolSize; i++)
    {
      GameObject bullet = Instantiate(bulletFactory);
      bulletObjectPool.Add(bullet);
      bullet.SetActive(false);
    }

#if UNITY_ANDROID
    GameObject.Find("Joystick canvas XYBZ").SetActive(true);
#elif UNITY_EDITOR || UNITY_STANDALONE
    GameObject.Find("Joystick canvas XYBZ").SetActive(false);
#endif
  }

  // Update is called once per frame
  void Update()
  {
#if UNITY_EDITOR || UNITY_STANDALONE
    if (Input.GetButtonDown("Fire1"))
    {
      Fire();
    }
#endif
  }

  public void Fire()
  {
    //GameObject bullet = Instantiate(bulletFactory);
    if (bulletObjectPool.Count > 0)
    {
      GameObject bullet = bulletObjectPool[0];
      bullet.SetActive(true);
      bullet.transform.position = firePosition.transform.position;
      bulletObjectPool.Remove(bullet);
    }
  }
}
