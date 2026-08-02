using UnityEngine;

/// <summary>
/// Interaction system logic triggering upward bounce forces on collision.
/// </summary>
public class MushroomBounce : MonoBehaviour
{
    [Header("Mushroom Mechanics Configuration")]
    [SerializeField] private float bounceVelocityOutput = 16f;
    [SerializeField] private AudioClip bounceAudioClip;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Search for the player component using your updated player class name
        PlayerController2D player = collision.gameObject.GetComponent<PlayerController2D>();

        if (player != null)
        {
            // Safely fetch the Rigidbody2D directly from the player object
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Reset vertical linear velocity and apply an upward physics bounce force
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
                playerRb.AddForce(Vector2.up * bounceVelocityOutput, ForceMode2D.Impulse);

                // Force a temporary grounded state if necessary to refresh jump tracking flags
                player.SetGrounded(false);
            }

            // Audio execution using specified global positional vectors
            if (bounceAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(bounceAudioClip, transform.position);
            }
        }
    }
}
