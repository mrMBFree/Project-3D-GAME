using UnityEngine;
using System;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnEnemyDestroyed;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogWarning("NavMeshAgent component not found on enemy: " + gameObject.name);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        if (agent != null)
        {
            agent.SetDestination(destination);
        }
        else
        {
            Debug.LogWarning("NavMeshAgent not found on enemy: " + gameObject.name);
        }
    }
    private void OnDestroy()
    {
        if (OnEnemyDestroyed != null)
        {
            OnEnemyDestroyed(this);
        }
    }

    // Call this method to destroy the enemy
    public void Kill()
    {
        // Perform any necessary cleanup before destruction
        Destroy(gameObject);
    }
}