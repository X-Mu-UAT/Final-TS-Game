using UnityEngine;

/// <summary>
/// PAWN/CONTROLLER PATTERN: This script acts as the Pawn (the body).
/// It handles raw physics operations, forces, and damage execution.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyPawn2D : MonoBehaviour
{
    [Header("Movement (Designer Configurable)")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float movementLinearDrag = 2f;

    [Header("Combat Output")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private Transform attackHitboxCenter;
    [SerializeField] private float attackHitboxRadius = 0.5f;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Audio (UAT Requirement)")]
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] private AudioClip attackAudioClip;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // UAT REQUIREMENT: Physical objects must use drag coefficients to slow movement forces
        rb.linearDamping = movementLinearDrag;
        rb.gravityScale = 1f; // Sets 2D physics gravity calculations to normal force scales
    }

    /// <summary>
    /// Executes movement updates by calculating and applying acceleration forces to the Rigidbody.
    /// </summary>
    public void Move(float horizontalDirection)
    {
        if (rb == null) return;

        // UAT REQUIREMENT: Move objects by adding a physics force vector
        Vector2 targetVelocity = new Vector2(horizontalDirection * movementSpeed, rb.linearVelocity.y);
        rb.linearVelocity = targetVelocity;

        // Visual handling: flip sprite based on directional values
        if (horizontalDirection > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (horizontalDirection < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    /// <summary>
    /// Swings a virtual trigger overlap boundary loop to detect and hit the player pawn.
    /// </summary>
    public void Attack()
    {
        Debug.Log($"{gameObject.name} performs an attack animation frame draw!");

        // UAT REQUIREMENT: Uses AudioSource.PlayOneShot to trigger clean feedback sound loops
        if (enemyAudioSource != null && attackAudioClip != null)
        {
            enemyAudioSource.PlayOneShot(attackAudioClip);
        }

        // Use a non-allocated overlap sphere acting as an instant trigger sensor check
        Collider2D playerHit = Physics2D.OverlapCircle(attackHitboxCenter.position, attackHitboxRadius, playerLayerMask);
        if (playerHit != null)
        {
            Health playerHealth = playerHit.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackHitboxCenter != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(attackHitboxCenter.position, attackHitboxRadius);
        }
    }
}
