using UnityEngine;

/// <summary>
/// Handles floating item pickups that increment the player's score profile upon contact.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class FloatingPickup2D : MonoBehaviour
{
    [Header("Score Configuration")]
    [SerializeField] private int scoreValueAmount = 100;

    [Header("Floating Movement (Framerate Independent)")]
    [SerializeField] private float floatAmplitude = 0.5f; // How high it floats
    [SerializeField] private float floatFrequency = 2f;   // How fast it moves up and down

    [Header("Audio Effects (UAT Requirement)")]
    [SerializeField] private AudioClip pickupAudioClip;

    private Vector3 initialStartingPosition;

    private void Start()
    {
        // Cache the original world coordinates layout on startup
        initialStartingPosition = transform.position;

        // Force the attached collision boundary to act as an overlap trigger sensor
        Collider2D pickupCollider = GetComponent<Collider2D>();
        if (pickupCollider != null)
        {
            pickupCollider.isTrigger = true;
        }
    }

    private void Update()
    {
        // UAT REQUIREMENT: Move an object by explicitly setting its transform position every frame draw.
        // The movement uses mathematical calculation multipliers to keep execution framerate independent.
        float currentSineWaveValue = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = initialStartingPosition + new Vector3(0f, currentSineWaveValue, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Filter incoming collisions to only trigger calculations if the target is tagged as the Player
        if (other.CompareTag("Player"))
        {
            // Update the global game state using our core tracking manager instance pattern
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValueAmount);
            }

            // UAT REQUIREMENT: Play sound effects using AudioSource.PlayClipAtPoint
            // This instantiates a temporary, self-destroying 3D sound point at the pickup's position
            if (pickupAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(pickupAudioClip, transform.position);
            }

            // Remove the item from the game board hierarchy cleanly
            Destroy(gameObject);
        }
    }
}
