using UnityEngine;

public enum EnemyState
{
    Idle,
    Trace,
    Return,
    Attack,
    Hit,
    Die
}

public class Enemy : MonoBehaviour
{
    public EnemyState CurrentState = EnemyState.Idle;

    private void Update()
    {
        switch (CurrentState)
        {
            case (EnemyState.Idle):
                {
                    Idle();
                    break;
                }
            case (EnemyState.Trace):
                {
                    Trace();
                    break;
                }
            case (EnemyState.Return):
                {
                    Return();
                    break;
                }
            case (EnemyState.Attack):
                {
                    Attack();
                    break;
                }
            case (EnemyState.Hit):
                {
                    Hit();
                    break;
                }
            case (EnemyState.Die):
                {
                    Die();
                    break;
                }
        }
    }

    private void Idle()
    {
        // 가만히 있기
    }

    private void Trace()
    {

    }

    private void Return()
    {

    }

    private void Attack()
    {

    }

    private void Hit()
    {

    }

    private void Die()
    {

    }
}
