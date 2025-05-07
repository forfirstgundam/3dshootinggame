using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// �ΰ����� : ������ ������ �ൿ�ϴ� �˰���
// ������ / ��ȹ�� -> ��Ģ ��� �ΰ�����(�������� ���) 
//                 -> ��� ���

public class ChargeEnemy : BaseEnemy
{
    public int AttackCount = 0;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
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

    public void ChangeToChargeSpeed()
    {
        _agent.speed = 15f;
    }

    public void ChangeToNormalSpeed()
    {
        _agent.speed = Stat.MoveSpeed;
    }
}