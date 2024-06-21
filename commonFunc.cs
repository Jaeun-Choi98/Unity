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
  Vector3 movement = new Vector3(h, 0, v);
  movement = movement.normalized;

  // Rigidbody�� ����� �̵�
  rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
}

// ������ �ٵ� �̿��� ����
public float jumpForce = 5f; // ���� ��
bool isGrounded; // ĳ���Ͱ� ���� ����ִ��� ����
void JumpRigidBody()
{
  if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
  {
    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    isGrounded = false;
  }
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


// ĳ���Ͱ� ���� ����� �� isGrounded�� true�� ����
void isGroundCheck()
{
  if (collision.gameObject.CompareTag("Ground"))
  {
    isGrounded = true;
  }
}

// ȭ�� ȸ�� �� �÷��̾� ȸ��
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

