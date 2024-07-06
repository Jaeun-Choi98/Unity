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

  Vector3 dir = new Vector3(h, 0, v);
  //dir.Normalize();
  // 오브젝트의 로컬 좌표계 -> 월드 좌표계로 변환
  dir = transform.TransformDirection(dir);
  if (isJump)
  {
    rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
  }
  else
  {
    rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
  }

  //rb.AddForce(dir*playerData.speed*Time.deltaTime,ForceMode.Impulse);
  //rb.velocity = new Vector3(dir.x * playerData.speed, rb.velocity.y, dir.z * playerData.speed);
}

// 리지드 바디를 이용한 점프
public float jumpForce = 5f; // 점프 힘
bool isJump; // 캐릭터가 땅에 닿아있는지 여부
void JumpRigidBody()
{
  if (Input.GetButtonDown("Jump") && !isJump)
  {
    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
  }

  if (CheckCollisionBelow())
  {
    if (isJump)
    {
      isJump = false;
    }
  }
  else
  {
    rb.AddForce(Vector3.down * 3f, ForceMode.Acceleration);
    isJump = true;
  }
}

private bool CheckCollisionBelow()
{
  Ray ray = new Ray(transform.position, Vector3.down);
  RaycastHit hit;
  bool ret = Physics.Raycast(ray, out hit, col.bounds.extents.y + 0.1f, collisionMask);
  return ret;
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

// 화면 회전 및 플레이어 회전
float xAngle = 0f;
float yAngle = 0f;

void RotatePlayer()
{
  /*float dy = Input.GetAxis("Mouse Y");
    yAngle -= dy * rotSpeed * Time.deltaTime;
    yAngle = Mathf.Clamp(yAngle, -90f, 90f);*/

  float dx = Input.GetAxis("Mouse X");
  xAngle += dx * rotSpeed * Time.deltaTime;

  Quaternion cameraRotation = Quaternion.Euler(0f, xAngle, 0f); // X 축 회전만 적용
  transform.rotation = cameraRotation;


  //transform.Rotate(Vector3.up * x);
}

// 플레이어 기준으로 카메라 회전( 구면 좌표계를 이용한 회전)
private GameObject target;

private float r = 4f;
private float rOffset = 3f;
private float rotSpeed = 150f;
private float yAngle = 0f;
private float xAngle = 0f;

void CameraPostionAndRotation()
{
  float dy = Input.GetAxis("Mouse Y");
  yAngle -= dy * rotSpeed * Time.deltaTime;
  yAngle = Mathf.Clamp(yAngle, -90f, 90f);

  float dx = Input.GetAxis("Mouse X");
  xAngle += dx * rotSpeed * Time.deltaTime;

  // 구면 좌표계를 사용하여 카메라의 위치 계산
  Vector3 offset = new Vector3(
   -r * Mathf.Cos(yAngle * Mathf.Deg2Rad) * Mathf.Sin(xAngle * Mathf.Deg2Rad),
    r * Mathf.Sin(yAngle * Mathf.Deg2Rad) + rOffset,
   -r * Mathf.Cos(yAngle * Mathf.Deg2Rad) * Mathf.Cos(xAngle * Mathf.Deg2Rad)
  );


  // 카메라가 주변 오브젝트에 닿았을 때, 뚫고 들어가는 문제 ->
  // 카메라와 플레이어 사이의 충돌을 감지하고 충돌한 경우 카메라를 해당 오브젝트의 표면으로 이동
  Ray ray = new Ray(target.transform.position, offset.normalized);
  RaycastHit hit;

  if (Physics.Raycast(ray, out hit, offset.magnitude, collisionMask))
  {
    transform.position = target.transform.position + offset.normalized * hit.distance * 0.8f;
  }
  else
  {
    transform.position = target.transform.position + offset;
  }

  transform.LookAt(target.transform.position);
}