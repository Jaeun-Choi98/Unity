using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
  // ���ʹ� ���� ���
  enum EnemyState
  {
    Idle,
    Move,
    Attack,
    Return,
    Damaged,
    Die
  }

  EnemyState m_State;

  GameObject player;
  CharacterController cc;

  // ���� ���� ����
  public float attackDistance = 2f;

  // �ʱ� ��ġ
  Vector3 originPos;

  // �̵� ���� ����
  public float moveDistance = 20f;
  public float moveSpeed = 5f;

  // �÷��̾� �߰� ����
  public float findDistance = 8f;

  // ���ʹ� ü�� �� UI
  int maxHp = 15;
  public int hp;
  public Slider hpSlider;

  private void Start()
  {
    m_State = EnemyState.Idle;
    player = GameObject.Find("Player");
    cc = gameObject.GetComponent<CharacterController>();
    originPos = transform.position;
    hp = maxHp;
  }

  private void Update()
  {
    switch (m_State)
    {
      case EnemyState.Idle:
        Idle();
        break;
      case EnemyState.Move:
        Move();
        break;
      case EnemyState.Attack:
        Attack();
        break;
      case EnemyState.Return:
        Return();
        break;
      case EnemyState.Damaged:
        break;
      case EnemyState.Die:
        break;
    }
    hpSlider.value = (float)hp / (float)maxHp;
  }


  void Idle()
  {
    // �÷��̾���� �Ÿ��� �׼� ���� ���� �̳���� Move ���·� ����
    if (Vector3.Distance(transform.position, player.transform.position) < findDistance)
    {
      m_State = EnemyState.Move;
      print("��� ����");
    }
  }


  void Move()
  {
    if (Vector3.Distance(transform.position, originPos) > moveDistance)
    {
      m_State = EnemyState.Return;
      print("Return ����");
    }
    else if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
    {
      Vector3 dir = (player.transform.position - transform.position).normalized;
      cc.Move(dir * moveSpeed * Time.deltaTime);
    }
    else
    {
      m_State = EnemyState.Attack;
      print("���� ����");
      // ���� ���·� ���̽� ��ٷ� ����
      currentTime = attackDelay;
    }
  }

  // ���� �ð�
  float currentTime = 0;
  float attackDelay = 2f;
  public int attackPower = 3;

  void Attack()
  {
    if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
    {
      currentTime += Time.deltaTime;
      if (currentTime > attackDelay)
      {
        player.GetComponent<PlayerMove>().DamageAction((float)attackPower);
        currentTime = 0;
      }
    }
    else
    {
      m_State = EnemyState.Move;
      print("�̵� ����");
      currentTime = 0;
    }
  }

  void Return()
  {
    if (Vector3.Distance(transform.position, originPos) > 0.1f)
    {
      Vector3 dir = (originPos - transform.position).normalized;
      hp = maxHp;
      cc.Move(dir * moveSpeed * Time.deltaTime);
    }
    else
    {
      transform.position = originPos;
      m_State = EnemyState.Idle;
      print("������");
    }
  }

  public void HitEnemy(int hitPower)
  {
    if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
    {
      return;
    }
    hp -= hitPower;
    if (hp > 0)
    {
      m_State = EnemyState.Damaged;
      print("���� ����: Any state -> Damaged");
      Damaged();
    }
    else
    {
      m_State = EnemyState.Die;
      print("���� ����: Any state -> Die");
      Die();
    }
  }
  void Damaged()
  {
    StartCoroutine(DamageProcess());
  }

  IEnumerator DamageProcess()
  {
    yield return new WaitForSeconds(0.5f);
    m_State = EnemyState.Move;
    print("�̵�����");
  }

  void Die()
  {
    StopAllCoroutines();
    StartCoroutine(DieProcess());
  }

  IEnumerator DieProcess()
  {
    cc.enabled = false;
    yield return new WaitForSeconds(2f);
    print("�Ҹ�");
    Destroy(gameObject);
  }


}