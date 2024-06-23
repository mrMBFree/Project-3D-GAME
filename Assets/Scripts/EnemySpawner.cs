using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public LayerMask safeZoneMask;
    public LayerMask groundLayerMask;
    public float spawnRadius = 20f;
    public float minDistanceFromPlayer = 10f;
    public int initialEnemyCount = 3;
    public int minEnemyCount = 1;
    public int spawnIncrement = 2;
    public float minDistanceBetweenEnemies = 5f;

   
    public int increasedHealth = 20;
    public float increasedDamage = 5f;
    public float increasedChaseRadius = 5f;

    public List<GameObject> enemies = new List<GameObject>();
    public Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        
        InvokeRepeating("SpawnNewEnemy", 2.0f, 5.0f); 
    }

    private void SpawnNewEnemy()
    {
        if (enemies.Count < minEnemyCount)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (!IsInSafeZone(spawnPosition) && !IsTooCloseToOtherEnemies(spawnPosition))
            {
                spawnPosition = AdjustToGroundHeight(spawnPosition);

                
                NavMeshHit hit;
                if (NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas))
                {
                    spawnPosition = hit.position;

                    GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    EnemyBehaviour enemyBehaviour = newEnemy.GetComponent<EnemyBehaviour>();
                    if (enemyBehaviour != null)
                    {
                       
                        enemyBehaviour.player = player;
                        enemyBehaviour.health += increasedHealth;
                        enemyBehaviour.damageAmount += increasedDamage;
                        enemyBehaviour.chaseRadius += increasedChaseRadius;
                    }
                    enemies.Add(newEnemy);

                    minEnemyCount += spawnIncrement;
                }
                else
                {
                    Debug.LogWarning("Failed to find valid position on NavMesh for enemy spawn.");
                }
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
            position.y = player.position.y;
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
            if (enemy == null) continue;

            if (Vector3.Distance(position, enemy.transform.position) < minDistanceBetweenEnemies)
            {
                return true;
            }
        }
        return false;
    }
}