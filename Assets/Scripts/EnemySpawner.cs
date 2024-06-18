using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public LayerMask safeZoneMask;
    public LayerMask groundLayerMask; // Add a layer mask for the ground
    public float spawnRadius = 20f;
    public float minDistanceFromPlayer = 10f;
    public int initialEnemyCount = 3;
    public int minEnemyCount = 1; // Minimalna liczba przeciwnik�w
    public int spawnIncrement = 2; // O ile zwi�ksza� liczb� przeciwnik�w za ka�dym razem
    public float minDistanceBetweenEnemies = 5f; // Minimal distance between enemies

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

            if (!IsInSafeZone(spawnPosition) && !IsTooCloseToOtherEnemies(spawnPosition))
            {
                // Adjust spawn position to ground height
                spawnPosition = AdjustToGroundHeight(spawnPosition);

                GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.OnEnemyDestroyed += HandleEnemyDestroyed;
                }
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

    private Vector3 AdjustToGroundHeight(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 50, Vector3.down, out hit, 100f, groundLayerMask))
        {
            position.y = hit.point.y;
        }
        else
        {
            position.y = player.position.y; // Fallback to player's height if ground not found
        }
        return position;
    }

    private bool IsInSafeZone(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1f, safeZoneMask);
        return colliders.Length > 0;
    }

    private bool IsTooCloseToOtherEnemies(Vector3 position)
    {
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(position, enemy.transform.position) < minDistanceBetweenEnemies)
            {
                return true;
            }
        }
        return false;
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        enemies.Remove(enemy.gameObject);
        minEnemyCount -= spawnIncrement; // Decrease minEnemyCount if necessary
    }
}