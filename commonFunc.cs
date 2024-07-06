// �÷��̾� ������
float speed = 5f;
void MovePlayer2DTransform()
{
  float h = Input.GetAxis("Horizontal");
  float v = Input.GetAxis("Vertical");

  Vector3 movement = Vector3.right * h + Vector3.up * v;
  // Vector3 movement = new Vector3(h, v, 0);
  // transform.Translate(movement * speed * Time.deltaTime); <- ������
  transform.position += dir * speed * Time.deltaTime;
}

// �÷��̾� ������ ( ���� ��ǥ �̵�/ ��� ��ǥ �̵�( ĳ���� ��Ʈ�ѷ� ��� ) )
float gravity = -20f;
float yVelocity = 0f;
CharacterController cc;
void MovePlayer3DTransform()
{
  float h = Input.GetAxis("Horizontal");
  float v = Input.GetAxis("Vertical");

  Vector3 dir = new Vector3(h, 0, v);
  dir = dir.normalized;

  // ���� ��ǥ���� �̵� ����
  transform.position += dir * speed * Time.deltaTime;

  /* ��� ��ǥ�� ���� ����( ���� ī�޶� ���� ) -> ���� ������Ʈ�� �������� ���󰡴� ���� �߻� -> ĳ���� ��Ʈ�ѷ��� ����ؼ� �ذ� ���� -> 
   -> x���� 90 or -90�� ���, vector3(0,1,0) or vector3(0,-1,0) ���� �Ǵ� ���� �߻�*/
  dir = Camera.main.transform.TransformDirection(dir);

  // ĳ���� ��Ʈ�ѷ��� ����� �߷��� ����� �̵� ���( ĳ���� ��Ʈ�ѷ��� Move �Լ� ���) 
  yVelocity += gravity * Time.deltaTime;
  dir.y = yVelocity;
  cc.Move(dir * moveSpeed * Time.deltaTime);
}

// ������ �ٵ� �̿��� �÷��̾� ������
private Rigidbody rb;
void MovePlayer3DRigidBody()
{
  float h = Input.GetAxis("Horizontal");
  float v = Input.GetAxis("Vertical");

  Vector3 dir = new Vector3(h, 0, v);
  //dir.Normalize();
  // ������Ʈ�� ���� ��ǥ�� -> ���� ��ǥ��� ��ȯ
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

// ������ �ٵ� �̿��� ����
public float jumpForce = 5f; // ���� ��
bool isJump; // ĳ���Ͱ� ���� ����ִ��� ����
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

// ĳ���� ��Ʈ�ѷ��� �̿��� ����
bool isJumping = false;
void JumpAndGravityCharacterController()
{
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
  yVelocity += gravity * Time.deltaTime;
  dir.y = yVelocity;
  cc.Move(dir * moveSpeed * Time.deltaTime);
}

// ȭ�� ȸ�� �� �÷��̾� ȸ��
float xAngle = 0f;
float yAngle = 0f;

void RotatePlayer()
{
  /*float dy = Input.GetAxis("Mouse Y");
    yAngle -= dy * rotSpeed * Time.deltaTime;
    yAngle = Mathf.Clamp(yAngle, -90f, 90f);*/

  float dx = Input.GetAxis("Mouse X");
  xAngle += dx * rotSpeed * Time.deltaTime;

  Quaternion cameraRotation = Quaternion.Euler(0f, xAngle, 0f); // X �� ȸ���� ����
  transform.rotation = cameraRotation;


  //transform.Rotate(Vector3.up * x);
}

// �÷��̾� �������� ī�޶� ȸ��( ���� ��ǥ�踦 �̿��� ȸ��)
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

  // ���� ��ǥ�踦 ����Ͽ� ī�޶��� ��ġ ���
  Vector3 offset = new Vector3(
   -r * Mathf.Cos(yAngle * Mathf.Deg2Rad) * Mathf.Sin(xAngle * Mathf.Deg2Rad),
    r * Mathf.Sin(yAngle * Mathf.Deg2Rad) + rOffset,
   -r * Mathf.Cos(yAngle * Mathf.Deg2Rad) * Mathf.Cos(xAngle * Mathf.Deg2Rad)
  );


  // ī�޶� �ֺ� ������Ʈ�� ����� ��, �հ� ���� ���� ->
  // ī�޶�� �÷��̾� ������ �浹�� �����ϰ� �浹�� ��� ī�޶� �ش� ������Ʈ�� ǥ������ �̵�
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