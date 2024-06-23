using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
  public GameObject firePosition;
  public GameObject bombFactory;

  // ≈ı√¥ ∫§≈Õ ≈©±‚
  public float throwPower = 15f;

  // √—æÀ ««∞› ¿Ã∆Â∆Æ
  public GameObject bulletEffect;
  // ««∞› ¿Ã∆Â∆Æ ∆ƒ∆º≈¨ Ω√Ω∫≈€
  ParticleSystem ps;

  public int waeponPower = 5;

  private void Start()
  {
    ps = bulletEffect.GetComponent<ParticleSystem>();
  }

  private void Update()
  {
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
