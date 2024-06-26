using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
  public GameObject firePosition;
  public GameObject bombFactory;

  // ��ô ���� ũ��
  public float throwPower = 15f;

  // �Ѿ� �ǰ� ����Ʈ
  public GameObject bulletEffect;
  // �ǰ� ����Ʈ ��ƼŬ �ý���
  ParticleSystem ps;

  public int waeponPower = 5;

  Animator anim;

  private void Start()
  {
    ps = bulletEffect.GetComponent<ParticleSystem>();
    anim = GetComponentInChildren<Animator>();
  }

  private void Update()
  {
    // ���� ���°� '���� ��' ������ ���� ����
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
      if(anim.GetFloat("MoveMotion") == 0)
      {
        anim.SetTrigger("Attack");
      }
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
