using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// �ΰ����� : ������ ������ �ൿ�ϴ� �˰���
// ������ / ��ȹ�� -> ��Ģ ��� �ΰ�����(�������� ���) 
//                 -> ��� ���

public class Enemy : BaseEnemy
{
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
        _returnPosition = transform.position;
        Health = Stat.MaxHealth;

        this.ChangeEnemyState(new IdleState());
    }

    private void Update()
    {
        _currentState?.Execute(this);
    }
}