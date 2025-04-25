using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float SpawnCoolTime = 5f;
    private float _timer = 0f;

    public int Offset = 5;
    public Vector3[] PatrolPositions;

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > SpawnCoolTime)
        {
            Debug.Log("spawning enemy");

            Vector3 PositionOffset = new Vector3(Random.Range(-Offset, Offset + 1), Random.Range(-Offset, Offset + 1), Random.Range(-Offset, Offset + 1));

            Vector3 spawnpoint = transform.position + PositionOffset;
            GameObject newthing = Pools.Instance.Create(1, transform.position);
            BaseEnemy enemy = newthing.GetComponent<BaseEnemy>();

            enemy.Initialize();
            enemy.PatrolPositions = new Vector3[PatrolPositions.Length];
            for (int i =0;i<PatrolPositions.Length; i++)
            {
                enemy.PatrolPositions[i] = PatrolPositions[i] + PositionOffset;
            }
            _timer = 0f;
        }
    }
}
