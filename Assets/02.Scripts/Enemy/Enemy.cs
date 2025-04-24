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


    //private void Update()
    //{
    //    switch (CurrentState)
    //    {
    //        case (EnemyState.Idle):
    //            {
    //                Idle();
    //                break;
    //            }
    //        case (EnemyState.Patrol):
    //            {
    //                Patrol();
    //                break;
    //            }
    //        case (EnemyState.Trace):
    //            {
    //                Trace();
    //                break;
    //            }
    //        case (EnemyState.Return):
    //            {
    //                Return();
    //                break;
    //            }
    //        case (EnemyState.Attack):
    //            {
    //                Attack();
    //                break;
    //            }
    //        case (EnemyState.Hit):
    //            {
    //                break;
    //            }
    //        case (EnemyState.Die):
    //            {
    //                StartCoroutine(Die());
    //                break;
    //            }
    //    }
    //}    
}