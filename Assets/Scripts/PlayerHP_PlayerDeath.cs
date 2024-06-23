using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using TMPro;

public class PlayerHP_PlayerDeath : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
    public int extraLives = 3; 

    public TMP_Text healthText;
    public TMP_Text extraLivesText;

    public List<GameObject> enemies;

    private void Start()
    {
        health = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            if (extraLives > 0)
            {
                UseExtraLife();
            }
            else
            {
                Die();
            }
        }
        UpdateUI();
    }

    void UseExtraLife()
    {
        extraLives--;
        health = maxHealth;
        KillAllEnemies();
        Debug.Log("Used extra life! Health restored, enemies killed.");
        UpdateUI();
    }

    void KillAllEnemies()
    {
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    void Die()
    {
        
        Debug.Log("Player died! Returning to main menu...");
        SceneManager.LoadScene("MainMenu");
    }

    void UpdateUI()
    {
        healthText.text =  health.ToString();
        extraLivesText.text =  extraLives.ToString();
    }
}