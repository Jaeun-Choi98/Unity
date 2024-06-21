using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

  private void Start()
  {
    cc = GetComponent<CharacterController>();
  }

  private void Update()
  {
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
  }
}
