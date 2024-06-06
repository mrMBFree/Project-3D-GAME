using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP_PlayerDeath : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle player death
        Debug.Log("Player died!");
    }
}