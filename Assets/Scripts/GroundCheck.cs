using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private PlayerController2D player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            player.SetGrounded(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            player.SetGrounded(false);
    }
}