using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxEnemies = 2;
    public float spawnDelay = 3f;
    public float minSpawnDistance = 8f;
    public float maxSpawnDistance = 15f;

    private Queue<Enemy> enemyPool = new Queue<Enemy>();
    private List<Enemy> activeEnemies = new List<Enemy>();
    private int spawnRequests = 0;

    void Start()
    {
        int stage = GameManager.Instance.GetStage();
        int maxEnemies = GetEnemyCountByStage(stage);
        for (int i = 0; i < maxEnemies; i++)
        {
            TrySpawn();
        }
    }

    void Update()
    {
        while (activeEnemies.Count + spawnRequests < maxEnemies)
        {
            StartCoroutine(SpawnEnemyWithDelay(spawnDelay));
            spawnRequests++;
        }
    }
    int GetEnemyCountByStage(int stage)
    {
        switch (stage)
        {
            case 1: return 2;
            case 2: return 10;
            default: return 1;
        }
    }
    public void ReturnToPool(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
        enemyPool.Enqueue(enemy);
    }

    IEnumerator SpawnEnemyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (TryGetValidSpawnPosition(out Vector3 position))
        {
            SpawnEnemy(position);
        }
        else
        {
            Debug.LogWarning("적 스폰 실패");
        }

        spawnRequests--;
    }

    void TrySpawn()
    {
        if (TryGetValidSpawnPosition(out Vector3 position))
        {
            SpawnEnemy(position);
        }
        else
        {
            Debug.LogWarning("초기 스폰 실패");
        }
    }

    void SpawnEnemy(Vector3 position)
    {
        Enemy enemy;

        if (enemyPool.Count > 0)
        {
            enemy = enemyPool.Dequeue();
        }
        else
        {
            GameObject obj = Instantiate(enemyPrefab);
            enemy = obj.GetComponent<Enemy>();
        }

        enemy.Init(this, position);
        activeEnemies.Add(enemy);
    }

    bool TryGetValidSpawnPosition(out Vector3 position)
    {
        Vector3 playerPos = Player.Instance.transform.position;

        for (int i = 0; i < 5; i++)
        {
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
            float angle = Random.Range(0f, 360f);

            float offsetX = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
            float offsetZ = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

            Vector3 rawPos = new Vector3(playerPos.x + offsetX, playerPos.y + 1f, playerPos.z + offsetZ);

            if (NavMesh.SamplePosition(rawPos, out NavMeshHit hit, 20f, NavMesh.AllAreas))
            {
                position = hit.position;
                return true;
            }
        }

        position = Vector3.zero;
        return false;
    }
}
