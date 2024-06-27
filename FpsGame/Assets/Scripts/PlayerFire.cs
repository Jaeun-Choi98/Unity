using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{

  enum WeaponMode
  {
    Normal,
    Sniper
  }

  WeaponMode wMode;

  public int waeponPower = 5;

  bool ZoomMode = false;

  public GameObject firePosition;
  public GameObject bombFactory;

  // 투척 벡터 크기
  public float throwPower = 15f;

  // 총알 피격 이펙트
  public GameObject bulletEffect;
  // 피격 이펙트 파티클 시스템
  ParticleSystem ps;

  Animator anim;

  public Text textWeaponMode;

  public GameObject[] effFlash;

  private void Start()
  {
    ps = bulletEffect.GetComponent<ParticleSystem>();
    anim = GetComponentInChildren<Animator>();
    wMode = WeaponMode.Normal;
  }

  private void Update()
  {
    if(wMode == WeaponMode.Normal)
    {
      textWeaponMode.text = "Normal Mode";
    }
    else
    {
      textWeaponMode.text = "Sniper Mode";
    }

    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      wMode = WeaponMode.Normal;
      Camera.main.fieldOfView = 60f;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      wMode = WeaponMode.Sniper;
    }

    // 게임 상태가 '게임 중' 상태일 때만 조작
    if (GameManager.gm.gState != GameManager.GameState.Run)
    {
      return;
    }

    if (Input.GetMouseButtonDown(1))
    {
      switch (wMode)
      {
        case WeaponMode.Normal:
          GameObject bomb = Instantiate(bombFactory);
          bomb.transform.position = firePosition.transform.position;

          Rigidbody rb = bomb.GetComponent<Rigidbody>();
          rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
          break;
        case WeaponMode.Sniper:
          if (!ZoomMode)
          {
            Camera.main.fieldOfView = 15f;
          }
          else
          {
            Camera.main.fieldOfView = 60f;
          }
          ZoomMode = !ZoomMode;
          break;
      }


    }

    if (Input.GetMouseButtonDown(0))
    {
      if (anim.GetFloat("MoveMotion") == 0)
      {
        anim.SetTrigger("Attack");
      }
      StartCoroutine(ShootEffectOn(0.05f));
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

  IEnumerator ShootEffectOn(float duration)
  {
    int num = Random.Range(0, effFlash.Length);
    effFlash[num].SetActive(true);
    yield return new WaitForSeconds(duration);
    effFlash[num].SetActive(false);
  }
}
