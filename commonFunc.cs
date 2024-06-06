public float speed = 5f;
public float jumpForce = 5f; // ���� ��
private bool isGrounded; // ĳ���Ͱ� ���� ����ִ��� ����
private Rigidbody rb;

void MovePlayer2DTransform()
{
  float h = Input.GetAxis("Horizontal");
  float v = Input.GetAxis("Vertical");

  Vector3 movement = Vector3.right * h + Vector3.up * v;
  // Vector3 movement = new Vector3(h, v, 0);
  // transform.Translate(movement * speed * Time.deltaTime); <- ������
  transform.position += dir * speed * Time.deltaTime;
}

void MovePlayer3DTransform()
{
  float h = Input.GetAxis("Horizontal");
  float v = Input.GetAxis("Vertical");

  Vector3 movement = new Vector3(h, 0, v);
  //Vector3 movement = Vector3.right * h + Vector3.forward * v;
  //transform.Translate(movement * speed * Time.deltaTime);
  transform.position += dir * speed * Time.deltaTime;

  Jump();
}

void MovePlayer3DRigidBody()
{
  float h = Input.GetAxis("Horizontal");
  float v = Input.GetAxis("Vertical");

  Vector3 movement = new Vector3(h, 0, v);

  // Rigidbody�� ����� �̵�
  rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);

  Jump();
}

// ���� �Է�
void Jump()
{
  if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
  {
    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    isGrounded = false;
  }
}

// ĳ���Ͱ� ���� ����� �� isGrounded�� true�� ����
void isGroundCheck()
{
  if (collision.gameObject.CompareTag("Ground"))
  {
    isGrounded = true;
  }
}
