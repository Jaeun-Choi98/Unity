using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
  public GameObject firePosition;
  public GameObject bombFactory;

  // 투척 벡터 크기
  public float throwPower = 15f;

  // 총알 피격 이펙트
  public GameObject bulletEffect;
  // 피격 이펙트 파티클 시스템
  ParticleSystem ps;

  public int waeponPower = 5;

  private void Start()
  {
    ps = bulletEffect.GetComponent<ParticleSystem>();
  }

  private void Update()
  {
    // 게임 상태가 '게임 중' 상태일 때만 조작
    if (GameManager.gm.gState != GameManager.GameState.Run)
    {
      return;
    }

    if (Input.GetMouseButtonDown(1))
    {
      GameObject bomb = Instantiate(bombFactory);
      bomb.transform.position = firePosition.transform.position;

      Rigidbody rb = bomb.GetComponent<Rigidbody>();
      rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
    }

    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
      RaycastHit hitInfo = new RaycastHit();
      if (Physics.Raycast(ray, out hitInfo))
      {
        if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
          EnemyFSM fsm = hitInfo.transform.GetComponent<EnemyFSM>();
          fsm.HitEnemy(waeponPower);
        }
        else
        {
          bulletEffect.transform.position = hitInfo.point;
          bulletEffect.transform.forward = hitInfo.normal;
          ps.Play();
        }
      }
    }
  }
}
