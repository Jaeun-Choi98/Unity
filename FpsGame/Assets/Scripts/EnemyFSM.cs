using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
  // 에너미 상태 상수
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

  // 공격 가능 범위
  public float attackDistance = 2f;

  // 초기 위치
  Vector3 originPos;
  Quaternion originRot;

  // 이동 가능 범위
  public float moveDistance = 20f;
  public float moveSpeed = 5f;

  // 플레이어 발견 범위
  public float findDistance = 8f;

  // 에너미 체력 및 UI
  int maxHp = 15;
  public int hp;
  public Slider hpSlider;

  Animator anim;

  private void Start()
  {
    m_State = EnemyState.Idle;
    player = GameObject.Find("Player");
    cc = gameObject.GetComponent<CharacterController>();
    originPos = transform.position;
    hp = maxHp;
    anim = GetComponentInChildren<Animator>();
    originRot = transform.rotation;
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
    // 플레이어와의 거리가 액션 시작 범위 이내라면 Move 상태로 전이
    if (Vector3.Distance(transform.position, player.transform.position) < findDistance)
    {
      m_State = EnemyState.Move;
      anim.SetTrigger("IdleToMove");
      print("대기 상태");
    }
  }


  void Move()
  {
    if (Vector3.Distance(transform.position, originPos) > moveDistance)
    {
      m_State = EnemyState.Return;
      print("Return 상태");
    }
    else if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
    {
      Vector3 dir = (player.transform.position - transform.position).normalized;
      cc.Move(dir * moveSpeed * Time.deltaTime);
      transform.forward = dir;
    }
    else
    {
      m_State = EnemyState.Attack;
      print("공격 상태");
      // 공격 상태로 전이시 곧바로 공격
      currentTime = attackDelay;
      anim.SetTrigger("MoveToAttackDelay");
    }
  }

  // 누적 시간
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
        /* 애니메이션 이벤트에서 공격 함수가 실행
         * Enemy 하위 오브젝트(Zombie1)에서 해당 애니메이션이 실행됨으로
         * 공격 함수를 하위 오브젝트(Animator 컴포넌트를 가지고 있는 오브젝트)에서 호출*/
        // player.GetComponent<PlayerMove>().DamageAction((float)attackPower);
        currentTime = 0;
        anim.SetTrigger("StartAttack");
      }
    }
    else
    {
      m_State = EnemyState.Move;
      print("이동 상태");
      currentTime = 0;
    }
  }

  public void AttackAction()
  {
    player.GetComponent<PlayerMove>().DamageAction((float)attackPower);
  }

  void Return()
  {
    if (Vector3.Distance(transform.position, originPos) > 0.1f)
    {
      Vector3 dir = (originPos - transform.position).normalized;
      hp = maxHp;
      cc.Move(dir * moveSpeed * Time.deltaTime);
      transform.forward = originPos;
    }
    else
    {
      transform.position = originPos;
      transform.rotation = originRot;
      m_State = EnemyState.Idle;
      print("대기상태");
      anim.SetTrigger("MoveToIdle");
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
      anim.SetTrigger("Damaged");
      print("상태 이전: Any state -> Damaged");
      Damaged();
    }
    else
    {
      m_State = EnemyState.Die;
      anim.SetTrigger("Die");
      print("상태 이전: Any state -> Die");
      Die();
    }
  }
  void Damaged()
  {
    StartCoroutine(DamageProcess());
  }

  IEnumerator DamageProcess()
  {
    yield return new WaitForSeconds(1f);
    m_State = EnemyState.Move;
    print("이동상태");
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
    print("소멸");
    Destroy(gameObject);
  }


}
