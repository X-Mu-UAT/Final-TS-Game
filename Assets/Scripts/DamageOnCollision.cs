using UnityEngine;
using System.Collections.Generic; // Required for using Lists and Dictionaries

public class DamageOnCollision2D : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private bool destroySelfOnImpact = false;

    [Header("Target Filtering")]
    [SerializeField] private string targetTag = "Player";

    [Header("Audio & Polish (UAT Requirement)")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip impactDamageSound;

    // UAT REQUIREMENT: Use at least one List
    [Header("Immune Objects List")]
    [SerializeField] private List<GameObject> temporarilyImmuneObjects = new List<GameObject>();

    // UAT REQUIREMENT: Use at least one Dictionary
    // Tracks how many times specific objects have hit this hazard for internal analytic data tracking
    private Dictionary<string, int> hazardHitRegistry = new Dictionary<string, int>();

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
        // UAT REQUIREMENT: Use at least one for loop to check the immunity list layout
        bool isTargetImmune = false;
        for (int i = 0; i < temporarilyImmuneObjects.Count; i++)
        {
            if (temporarilyImmuneObjects[i] == hitObject)
            {
                isTargetImmune = true;
                break;
            }
        }

        // 1. Check if the object we hit has the correct tag and is not immune
        // UAT REQUIREMENT: Show use of at least one if statement with an else clause
        if (hitObject.CompareTag(targetTag) && !isTargetImmune)
        {
            // 2. Look for a Health component on the object (or its parent)
            Health healthComponent = hitObject.GetComponentInParent<Health>();
            if (healthComponent != null)
            {
                // 3. Apply the damage
                healthComponent.TakeDamage(damageAmount);

                // UAT REQUIREMENT: Track data inside a Dictionary container dynamically
                string objectNameKey = hitObject.name;
                if (hazardHitRegistry.ContainsKey(objectNameKey))
                {
                    hazardHitRegistry[objectNameKey]++;
                }
                else
                {
                    hazardHitRegistry.Add(objectNameKey, 1);
                }

                // UAT REQUIREMENT: Play sounds through an AudioSource using AudioSource.PlayOneShot
                if (sfxAudioSource != null && impactDamageSound != null)
                {
                    sfxAudioSource.PlayOneShot(impactDamageSound);
                }

                // 4. Optionally destroy this hazard object (great for fireballs/bullets)
                if (destroySelfOnImpact)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Debug.Log($"Collision registered with {hitObject.name}, but no damage was applied (Wrong Tag or Immune).");
        }
    }
}
