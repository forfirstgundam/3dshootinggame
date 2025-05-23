using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// 인공지능 : 지능을 가지고 행동하는 알고리즘
// 반응형 / 계획형 -> 규칙 기반 인공지능(전통적인 방식) 
//                 -> 제어문 기반

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