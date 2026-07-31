using UnityEngine;
using UnityEngine.InputSystem; // Required for the New Input System package

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool jumpRequested;

    private void Start()
    {
        // Cache the Rigidbody2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 1. Gather horizontal movement input using the Keyboard API cleanly
        horizontalInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                horizontalInput -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                horizontalInput += 1f;
        }


        // 2. Detect jump input using the Space key
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        // 3. Perform ground checking using an OverlapBox
        if (groundCheckPoint != null)
        {
            isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0f, groundLayer);
        }

        // 4. Apply horizontal movement physics (maintaining existing vertical velocity)
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // 5. Apply jump physics if a jump was requested in Update
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false; // Reset the flag immediately after applying force
        }
    }

    // Optional: Visualizes the ground check box in the Unity Editor Scene view
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
        }
    }

    // Call this from external scripts to manually set the grounded state
    public void SetGrounded(bool groundedState)
    {
        isGrounded = groundedState;
    }
}
