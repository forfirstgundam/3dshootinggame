using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
        Debug.Log("change speed to charge");
        _agent.speed = 15f;
        _agent.acceleration = 100f;
    }

    public void ChangeToNormalSpeed()
    {
        Debug.Log("change speed to normal");
        _agent.speed = Stat.MoveSpeed;
        _agent.acceleration = 8f;
    }
}