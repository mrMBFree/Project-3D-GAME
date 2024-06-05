using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public LayerMask safeZoneMask;
    public float spawnRadius = 20f;
    public float minDistanceFromPlayer = 10f;
    public int initialEnemyCount = 3;
    public int minEnemyCount = 1; // Minimalna liczba przeciwników
    public int spawnIncrement = 2; // O ile zwiêkszaæ liczbê przeciwników za ka¿dym razem

    public List<GameObject> enemies = new List<GameObject>();
    public Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Start spawning enemies using InvokeRepeating
        InvokeRepeating("SpawnNewEnemy", 2.0f, 5.0f); // Start after 2 seconds and repeat every 5 seconds
    }

    private void SpawnNewEnemy()
    {
        if (enemies.Count < minEnemyCount)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (!IsInSafeZone(spawnPosition))
            {
                // Get player's height
                float playerHeight = player.position.y;

                // Set spawn position with player's height
                spawnPosition.y = playerHeight;

                GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                enemies.Add(newEnemy);

                // Increase minEnemyCount if enemies count is below minEnemyCount
                minEnemyCount += spawnIncrement;
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position;
        randomDirection.y = 0;

        while (Vector3.Distance(randomDirection, player.position) < minDistanceFromPlayer)
        {
            randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection += transform.position;
            randomDirection.y = 0;
        }

        return randomDirection;
    }

    private bool IsInSafeZone(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1f, safeZoneMask);
        return colliders.Length > 0;
    }
}