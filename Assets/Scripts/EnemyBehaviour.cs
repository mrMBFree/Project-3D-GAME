using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public float patrolRadius = 10f;
    public float patrolTime = 5f;
    public float chaseRadius = 15f;
    public float damageAmount = 10f; // Add this line
    public LayerMask safeZoneMask;
    public LayerMask playerMask;
    public Transform player;

    private NavMeshAgent agent;
    private float timer;
    private bool isChasingPlayer;
    private Animator animator;
    private EnemyHP_EnemyDeath enemyDeathScript;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyDeathScript = GetComponent<EnemyHP_EnemyDeath>();
        timer = patrolTime;
        isChasingPlayer = false;
    }

    void Update()
    {
        if (enemyDeathScript != null && enemyDeathScript.isDead)
        {
            agent.enabled = false;
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
        animator.SetBool("IsWalking", agent.velocity.magnitude > 0.1f && !isChasingPlayer);
        animator.SetBool("IsRunning", agent.velocity.magnitude > 0.1f && isChasingPlayer);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            Vector3 newPos = GetRandomPatrolPosition();
            agent.SetDestination(newPos);
        }
        else if (other.CompareTag("Player")) // Add this block
        {
            PlayerHP_PlayerDeath playerHealth = other.GetComponent<PlayerHP_PlayerDeath>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}