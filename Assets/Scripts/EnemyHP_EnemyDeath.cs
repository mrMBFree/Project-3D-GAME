using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP_EnemyDeath : MonoBehaviour
{
    [Tooltip("Maximum health of the enemy")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public bool isDead = false;
    public int pointsForHeadshot = 250;
    public int pointsForBodyshot = 100;
    public int pointsForLimbshot = 150;

    private float health;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator component not found on the enemy.");
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        else
        {
            Destroy(gameObject);
        }

        // Optionally, destroy the enemy after the death animation has played
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Assuming the death animation length is 2 seconds. Adjust according to your animation length.
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}