using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// �ΰ����� : ������ ������ �ൿ�ϴ� �˰���
// ������ / ��ȹ�� -> ��Ģ ��� �ΰ�����(�������� ���) 
//                 -> ��� ���

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

            // �浹 �� ���� ��ȯ
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