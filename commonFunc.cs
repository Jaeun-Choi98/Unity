// 플레이어 움직임
float speed = 5f;
void MovePlayer2DTransform()
{
  float h = Input.GetAxis("Horizontal");
  float v = Input.GetAxis("Vertical");

  Vector3 movement = Vector3.right * h + Vector3.up * v;
  // Vector3 movement = new Vector3(h, v, 0);
  // transform.Translate(movement * speed * Time.deltaTime); <- 종속적
  transform.position += dir * speed * Time.deltaTime;
}

// 플레이어 움직임 ( 절대 좌표 이동/ 상대 좌표 이동( 캐릭터 컨트롤러 사용 ) )
float gravity = -20f;
float yVelocity = 0f;
CharacterController cc;
void MovePlayer3DTransform()
{
  float h = Input.GetAxis("Horizontal");
  float v = Input.GetAxis("Vertical");

  Vector3 dir = new Vector3(h, 0, v);
  dir = dir.normalized;

  // 절대 좌표로의 이동 벡터
  transform.position += dir * speed * Time.deltaTime;

  /* 상대 좌표로 벡터 설정( 메인 카메라 기준 ) -> 게임 오브젝트가 공중으로 날라가는 문제 발생 -> 캐릭터 컨트롤러를 사용해서 해결 가능 -> 
   -> x축이 90 or -90일 경우, vector3(0,1,0) or vector3(0,-1,0) 으로 되는 문제 발생*/
  dir = Camera.main.transform.TransformDirection(dir);

  // 캐릭터 컨트롤러를 사용한 중력이 적용된 이동 기능( 캐릭터 컨트롤러의 Move 함수 사용) 
  yVelocity += gravity * Time.deltaTime;
  dir.y = yVelocity;
  cc.Move(dir * moveSpeed * Time.deltaTime);
}

// 리지드 바디를 이용한 플레이어 움직임
private Rigidbody rb;
void MovePlayer3DRigidBody()
{
  float h = Input.GetAxis("Horizontal");
  float v = Input.GetAxis("Vertical");
  Vector3 movement = new Vector3(h, 0, v);
  movement = movement.normalized;

  // Rigidbody를 사용한 이동
  rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
}

// 리지드 바디를 이용한 점프
public float jumpForce = 5f; // 점프 힘
bool isGrounded; // 캐릭터가 땅에 닿아있는지 여부
void JumpRigidBody()
{
  if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
  {
    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    isGrounded = false;
  }
}

// 캐릭터 컨트롤러를 이용한 점프
bool isJumping = false;
void JumpAndGravityCharacterController()
{
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
  yVelocity += gravity * Time.deltaTime;
  dir.y = yVelocity;
  cc.Move(dir * moveSpeed * Time.deltaTime);
}


// 캐릭터가 땅에 닿았을 때 isGrounded를 true로 설정
void isGroundCheck()
{
  if (collision.gameObject.CompareTag("Ground"))
  {
    isGrounded = true;
  }
}

// 화면 회전 및 플레이어 회전
float mx = 0f;
float my = 0f;

void RotatePlayer()
{
  float mouse_X = Input.GetAxis("Mouse X");
  float mouse_Y = Input.GetAxis("Mouse Y");

  mx += mouse_X * rotSpeed * Time.deltaTime;
  my += mouse_Y * rotSpeed * Time.deltaTime;
  my = Mathf.Clamp(my, -90f, 90f);
  transform.eulerAngles = new Vector3(-my, mx, 0);
}

