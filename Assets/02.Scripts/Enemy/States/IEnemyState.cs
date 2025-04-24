public interface IEnemyState
{
    void Enter(BaseEnemy enemy);
    void Execute(BaseEnemy enemy);
    void Exit(BaseEnemy enemy);
}