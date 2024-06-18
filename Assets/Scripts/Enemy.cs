using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnEnemyDestroyed;

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