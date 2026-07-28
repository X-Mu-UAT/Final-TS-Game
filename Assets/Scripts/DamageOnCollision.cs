using UnityEngine;

public class DamageOnCollision2D : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private bool destroySelfOnImpact = false;

    [Header("Target Filtering")]
    [SerializeField] private string targetTag = "Player";

    // Handles physical, solid collisions (e.g., walking into spikes)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryApplyDamage(collision.gameObject);
    }

    // Handles trigger overlaps (e.g., passing through a laser or a projectile)
    private void OnTriggerEnter2D(Collider2D other)
    {
        TryApplyDamage(other.gameObject);
    }

    private void TryApplyDamage(GameObject hitObject)
    {
        // 1. Check if the object we hit has the correct tag
        if (hitObject.CompareTag(targetTag))
        {
            // 2. Look for a Health component on the object (or its parent)
            Health healthComponent = hitObject.GetComponentInParent<Health>();

            if (healthComponent != null)
            {
                // 3. Apply the damage
                healthComponent.TakeDamage(damageAmount);

                // 4. Optionally destroy this hazard object (great for fireballs/bullets)
                if (destroySelfOnImpact)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
