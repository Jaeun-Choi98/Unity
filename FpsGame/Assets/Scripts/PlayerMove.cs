using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
  public float moveSpeed = 7f;
  
  CharacterController cc;

  // 중력 변수
  float gravity = -20f;
  // 수직 속력 변수
  float yVelocity = 0f;

  // 점프력 변수
  public float jumpPower = 10f;
  public bool isJumping = false;

  // hp 변수
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
    // 게임 상태가 '게임 중' 상태일 때만 조작
    if(GameManager.gm.gState != GameManager.GameState.Run)
    {
      return;
    }

    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");

    Vector3 dir = new Vector3(h, 0, v);
    dir = dir.normalized;

    // 상대 좌표로 벡터 이동(메인 카메라 기준)
    dir = Camera.main.transform.TransformDirection(dir);

    // 점프
    /*CollisionFlags.Below는 캐릭터 컨트롤러의 충돌 영역 중
     아래쪽 부분에 충돌했을 때 true를 반환*/
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

    // 카메라 수직 속도에 중력 값 적용
    yVelocity += gravity * Time.deltaTime;
    dir.y = yVelocity;
    cc.Move(dir * moveSpeed * Time.deltaTime);

    // 절대 좌표로 벡터 이동
    //transform.position += dir * moveSpeed * Time.deltaTime;


    // 현재 플레이어 hp를 ui에 반영
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
