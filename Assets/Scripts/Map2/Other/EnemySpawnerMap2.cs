using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerMap2 : MonoBehaviour
{
    [Header("Spawn")]
    public float minDistanceFromPlayer = 12f;
    public int spawnAttempts = 10;

    [Header("Cleanup")]
    public float despawnDistance = 40f;
    public int checkPerFrame = 5;

    [Header("Waves")]
    public List<WaveInfo> waves;

    private readonly List<GameObject> spawnedEnemies = new();
    [SerializeField] private GameObject target;
    private Transform targetTranform;

    private float spawnCounter;
    private float waveCounter;

    private int currentWave;
    private int enemyToCheck;

    private void Start()
    {
        targetTranform = target.transform;

        currentWave = -1;

        GoToNextWave();
    }

    private void Update()
    {
        if (!target.activeSelf)
            return;

        UpdateWave();
        CleanupEnemies();
    }

    private void UpdateWave()
    {
        if (currentWave >= waves.Count)
            return;

        waveCounter -= Time.deltaTime;

        if (waveCounter <= 0)
            GoToNextWave();

        spawnCounter -= Time.deltaTime;

        if (spawnCounter <= 0)
        {
            spawnCounter = waves[currentWave].timeBetweenSpawns;
            
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (!TryGetSpawnPoint(out Vector3 spawnPos))
            return;

        GameObject newEnemy = Instantiate(
            waves[currentWave].enemyToSpawn,
            spawnPos,
            Quaternion.identity
        );
        // Set the new enemy to target the player
        EnemyController enemy = newEnemy.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.SetTarget(target);
        }
        spawnedEnemies.Add(newEnemy);
    }

    private bool TryGetSpawnPoint(out Vector3 result)
    {
        for (int i = 0; i < spawnAttempts; i++)
        {
            Vector3 point = SpawnPoint.Instance.GetRandomSpawnPoint();

            float dist = Vector3.Distance(targetTranform.position, point);

            if (dist < minDistanceFromPlayer)
                continue;

            result = point;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void CleanupEnemies()
    {
        int checkTarget = enemyToCheck + checkPerFrame;

        while (enemyToCheck < checkTarget)
        {
            if (enemyToCheck >= spawnedEnemies.Count)
            {
                enemyToCheck = 0;
                return;
            }

            GameObject enemy = spawnedEnemies[enemyToCheck];

            if (enemy == null)
            {
                spawnedEnemies.RemoveAt(enemyToCheck);
                checkTarget--;
                continue;
            }

            float dist = Vector3.Distance(
                targetTranform.position,
                enemy.transform.position
            );

            if (dist > despawnDistance)
            {
                Destroy(enemy);

                spawnedEnemies.RemoveAt(enemyToCheck);

                checkTarget--;
                continue;
            }

            enemyToCheck++;
        }
    }

    public void GoToNextWave()
    {
        currentWave++;

        if (currentWave >= waves.Count)
            currentWave = waves.Count - 1;

        waveCounter = waves[currentWave].waveLength;
        spawnCounter = waves[currentWave].timeBetweenSpawns;
    }
}