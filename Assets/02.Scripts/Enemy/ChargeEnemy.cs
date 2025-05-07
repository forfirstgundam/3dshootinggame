using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// 인공지능 : 지능을 가지고 행동하는 알고리즘
// 반응형 / 계획형 -> 규칙 기반 인공지능(전통적인 방식) 
//                 -> 제어문 기반

public class ChargeEnemy : BaseEnemy
{
    public int AttackCount = 0;
    public bool CanCharge = false;

    private void Awake()
    {
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
        _returnPosition = transform.position;
        Health = Stat.MaxHealth;
        Animator = gameObject.GetComponentInChildren<Animator>();

        this.ChangeEnemyState(new IdleState());
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        _currentState?.Execute(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!CanCharge) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Damage damage = new Damage
            {
                Value = Stat.AttackDamage,
                KnockValue = Stat.AttackKnockValue,
                KnockDir = (collision.transform.position - transform.position).normalized,
                From = gameObject
            };

            Player.Instance.TakeDamage(damage);
            CanCharge = false;

            // 충돌 후 상태 전환
            ChangeEnemyState(new TraceState());
        }
    }

    public void ChangeToChargeSpeed()
    {
        _agent.speed = 15f;
    }

    public void ChangeToNormalSpeed()
    {
        _agent.speed = Stat.MoveSpeed;
    }
}