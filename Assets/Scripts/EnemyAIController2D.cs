using UnityEngine;

/// <summary>
/// PAWN/CONTROLLER PATTERN: This script acts as the AI Controller (the brain).
/// It evaluates the player's position and commands the separate EnemyPawn script.
/// </summary>
public class EnemyAIController2D : MonoBehaviour
{
    [Header("Target Tracking")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 2f;

    private Transform playerTarget;
    private EnemyPawn2D enemyPawn;
    private float nextAttackTime = 0f;

    private void Start()
    {
        // Cache the attached physical pawn component
        enemyPawn = GetComponent<EnemyPawn2D>();

        // Safely locate the player character in the scene layout
        GameObject playerObj = GameObject.FindWithTag(playerTag);
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }
    }

    private void Update()
    {
        if (playerTarget == null || enemyPawn == null) return;

        // Calculate the distance to the player character
        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        // State Machine Decision: Move toward player if detected, or attack if in range
        if (distanceToPlayer <= detectionRadius)
        {
            // Determine the direction vector (-1 for left, 1 for right)
            float direction = (playerTarget.position.x > transform.position.x) ? 1f : -1f;

            if (distanceToPlayer <= attackRange)
            {
                // Stop horizontal movement when within attack range
                enemyPawn.Move(0f);

                // Framerate independent cooldown check using Time.time calculations
                if (Time.time >= nextAttackTime)
                {
                    enemyPawn.Attack();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
            else
            {
                // Command the pawn to march toward the target location
                enemyPawn.Move(direction);
            }
        }
        else
        {
            // Stop moving if the player escapes the detection zone boundary
            enemyPawn.Move(0f);
        }
    }

    // Optional: Visualizes detection ranges directly within the Unity Editor Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
