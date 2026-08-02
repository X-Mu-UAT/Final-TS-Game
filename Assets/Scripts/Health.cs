using UnityEngine;
using UnityEngine.InputSystem; // Required for using the New Input System framework

public class Health : MonoBehaviour
{
    [Header("Health Integrity Metrics")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    [Header("Frog Scale Configurations (UAT Requirement)")]
    [SerializeField] private Vector3 regularScaleDimensions = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 giantScaleDimensions = new Vector3(2f, 2f, 1f);
    [SerializeField] private Vector3 miniScaleDimensions = new Vector3(0.5f, 0.5f, 1f);

    // Public Property exposing internal health states to external scripts (like WorldSpaceHealthBar)
    public int CurrentHealth
    {
        get { return currentHealth; }
        private set { currentHealth = value; }
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    private void Update()
    {
        // FIXED FOR NEW INPUT SYSTEM: Uses Keyboard.current API instead of old Input.GetKeyDown
        if (gameObject.CompareTag("Player") && Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame) ModifyFrogScale(0); // Regular
            if (Keyboard.current.digit2Key.wasPressedThisFrame) ModifyFrogScale(1); // Giant
            if (Keyboard.current.digit3Key.wasPressedThisFrame) ModifyFrogScale(2); // Mini
        }
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage! Current health: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Changes the size dimensions of the player sprite object based on the requested scale status.
    /// </summary>
    /// <param name="scaleIndex">0 = Regular, 1 = Giant, 2 = Mini</param>
    public void ModifyFrogScale(int scaleIndex)
    {
        if (scaleIndex == 1)
        {
            transform.localScale = giantScaleDimensions;
            Debug.Log("Frog grew to GIANT size!");
        }
        else if (scaleIndex == 2)
        {
            transform.localScale = miniScaleDimensions;
            Debug.Log("Frog shrank to MINI size!");
        }
        else
        {
            transform.localScale = regularScaleDimensions;
            Debug.Log("Frog returned to REGULAR size!");
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        // Instead of instantly destroying the player, hand execution over to the GameManager
        if (gameObject.CompareTag("Player") && GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
