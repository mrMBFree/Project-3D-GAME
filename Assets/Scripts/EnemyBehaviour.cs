using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public float patrolRadius = 10f;
    public float patrolTime = 5f;
    public float chaseRadius = 15f;
    public float damageAmount = 10f;
    public float pushBackForce = 5f;
    public LayerMask safeZoneMask;
    public LayerMask playerMask;
    public Transform player;

    // Health and other parameters
    public float health = 100f; // Initial health
    private float maxHealth;
    private NavMeshAgent agent;
    private float timer;
    private bool isChasingPlayer;
    private bool isDying;
    private Animator animator;
    private PlayerHP_PlayerDeath playerHealthScript;

    // Footstep sound parameters
    public AudioSource audioSource; // AudioSource dodany do wroga
    public AudioClip[] footstepSounds; // Tablica z dŸwiêkami kroków
    public float stepInterval = 0.5f; // Czas miêdzy krokami
    private float nextStepTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerHealthScript = FindObjectOfType<PlayerHP_PlayerDeath>(); // Find the player's health script
        maxHealth = health; // Set maxHealth to initial health value
        timer = patrolTime;
        isChasingPlayer = false;
        isDying = false;
    }

    void Update()
    {
        if (isDying)
        {
            return;
        }

        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        UpdateAnimator();
        HandleFootstepSounds();
    }

    void Patrol()
    {
        timer += Time.deltaTime;

        if (timer >= patrolTime)
        {
            Vector3 newPos = GetRandomPatrolPosition();
            agent.SetDestination(newPos);
            timer = 0;
        }

        if (Vector3.Distance(transform.position, player.position) <= chaseRadius)
        {
            isChasingPlayer = true;
        }
    }

    void ChasePlayer()
    {
        if (!IsInSafeZone(player.position))
        {
            agent.SetDestination(player.position);
        }
        else
        {
            isChasingPlayer = false;
            timer = patrolTime;
        }

        if (Vector3.Distance(transform.position, player.position) > chaseRadius)
        {
            isChasingPlayer = false;
            timer = patrolTime;
        }
    }

    Vector3 GetRandomPatrolPosition()
    {
        Vector3 randomPos;
        do
        {
            randomPos = RandomNavSphere(transform.position, patrolRadius, -1);
        } while (IsInSafeZone(randomPos));

        return randomPos;
    }

    bool IsInSafeZone(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1f, safeZoneMask);
        return colliders.Length > 0;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
        return navHit.position;
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            bool isWalking = agent.velocity.magnitude > 0.1f && !isChasingPlayer;
            bool isRunning = agent.velocity.magnitude > 0.1f && isChasingPlayer;

            animator.SetBool("IsWalking", isWalking);
            animator.SetBool("IsRunning", isRunning);
        }
    }

    void HandleFootstepSounds()
    {
        if (agent.velocity.magnitude > 0.1f && Time.time >= nextStepTime)
        {
            PlayFootstepSound();
            nextStepTime = Time.time + stepInterval;
        }
    }

    void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            int index = Random.Range(0, footstepSounds.Length);
            audioSource.clip = footstepSounds[index];
            audioSource.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDying)
            return;

        if (other.CompareTag("SafeZone"))
        {
            Vector3 newPos = GetRandomPatrolPosition();
            agent.SetDestination(newPos);
        }
        else if (other.CompareTag("Player"))
        {
            // Apply push force using CharacterController.Move
            CharacterController characterController = other.GetComponent<CharacterController>();
            if (characterController != null)
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                pushDirection.y = 0; // Ensure push is only horizontal
                Vector3 pushBackVector = pushDirection * pushBackForce;

                // Apply the push force to move the player
                characterController.Move(pushBackVector);

                // Damage the player
                if (playerHealthScript != null)
                {
                    playerHealthScript.TakeDamage(damageAmount);
                }
            }
        }
    }

    // Method to take damage
    public void TakeDamage(float amount)
    {
        if (isDying)
            return;

        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDying = true;
        animator.SetTrigger("Die"); // Assumes there's a "Die" trigger in the animator
        agent.speed = 0; // Set speed to 0
        agent.isStopped = true; // Stop the agent
        GetComponent<Collider>().enabled = false; // Disable the collider
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(2f); // Adjust the wait time to match the death animation duration
        Destroy(gameObject);
    }
}