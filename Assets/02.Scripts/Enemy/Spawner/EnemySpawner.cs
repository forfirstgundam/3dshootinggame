using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float SpawnCoolTime = 5f;
    private float _timer = 0f;

    public float offset = 3f;
    public Transform[] PatrolPositions;

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > SpawnCoolTime)
        {
            Debug.Log("spawning enemy");
            Vector3 spawnpoint = new Vector3(transform.position.x + Random.Range(-offset, offset), transform.position.y + Random.Range(-offset, offset), transform.position.z + Random.Range(-offset, offset));
            GameObject newthing = Pools.Instance.Create(1, transform.position);
            BaseEnemy enemy = newthing.GetComponent<BaseEnemy>();

            enemy.Initialize();
            enemy.PatrolPositions = PatrolPositions;
            _timer = 0f;
        }
    }
}
