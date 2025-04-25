using UnityEngine;
using UnityEngine.AI;

public class FollowEnemy : BaseEnemy
{
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
        _returnPosition = transform.position;
        Health = Stat.MaxHealth;

        this.ChangeEnemyState(new TraceState());
    }

    private void Update()
    {
        _currentState?.Execute(this);
    }
}
