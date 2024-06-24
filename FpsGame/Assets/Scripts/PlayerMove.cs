using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
  public float moveSpeed = 7f;
  
  CharacterController cc;

  // �߷� ����
  float gravity = -20f;
  // ���� �ӷ� ����
  float yVelocity = 0f;

  // ������ ����
  public float jumpPower = 10f;
  public bool isJumping = false;

  // hp ����
  public float hp = 20f;
  float maxHp = 20f;
  public Slider hpSlider;

  public GameObject hitEffect;

  private void Start()
  {
    cc = GetComponent<CharacterController>();
  }

  private void Update()
  {
    // ���� ���°� '���� ��' ������ ���� ����
    if(GameManager.gm.gState != GameManager.GameState.Run)
    {
      return;
    }

    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");

    Vector3 dir = new Vector3(h, 0, v);
    dir = dir.normalized;

    // ��� ��ǥ�� ���� �̵�(���� ī�޶� ����)
    dir = Camera.main.transform.TransformDirection(dir);

    // ����
    /*CollisionFlags.Below�� ĳ���� ��Ʈ�ѷ��� �浹 ���� ��
     �Ʒ��� �κп� �浹���� �� true�� ��ȯ*/
    if (cc.collisionFlags == CollisionFlags.Below)
    {
      if (isJumping)
      {
        isJumping = false;
      }
      yVelocity = 0f;
    }

    if (Input.GetButtonDown("Jump") && !isJumping)
    {
      yVelocity = jumpPower;
      isJumping = true;
    }

    // ī�޶� ���� �ӵ��� �߷� �� ����
    yVelocity += gravity * Time.deltaTime;
    dir.y = yVelocity;
    cc.Move(dir * moveSpeed * Time.deltaTime);

    // ���� ��ǥ�� ���� �̵�
    //transform.position += dir * moveSpeed * Time.deltaTime;


    // ���� �÷��̾� hp�� ui�� �ݿ�
    hpSlider.value = hp / maxHp;
  }

  
  public void DamageAction(float damage)
  {
    hp -= damage;
    if (hp > 0)
    {
      StartCoroutine(PlayHitEffect());
    }
  }

  IEnumerator PlayHitEffect()
  {
    hitEffect.SetActive(true);
    yield return new WaitForSeconds(0.3f);
    hitEffect.SetActive(false);
  }
}
