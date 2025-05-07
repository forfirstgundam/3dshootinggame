using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// �ΰ����� : ������ ������ �ൿ�ϴ� �˰���
// ������ / ��ȹ�� -> ��Ģ ��� �ΰ�����(�������� ���) 
//                 -> ��� ���

public class MimicEnemy : BaseEnemy
{
    public GameObject Canvas;

    private void Awake()
    {
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
        _returnPosition = transform.position;
        Health = Stat.MaxHealth;
        Animator = gameObject.GetComponentInChildren<Animator>();

        this.ChangeEnemyState(new MimicIdleState());
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        _currentState?.Execute(this);
    }
}