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

    [Header("Direct Position Shifting (UAT Requirement)")]
    [SerializeField] private Transform customMovingPlatform;
    [SerializeField] private float platformShiftSpeed = 2f;

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
        horizontalInput = 0f;

        // 1. Gather horizontal movement input from Keyboard API AND Joystick/Gamepad axes
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                horizontalInput -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                horizontalInput += 1f;
        }

        // UAT REQUIREMENT: Read input from joystick axes
        if (Gamepad.current != null)
        {
            float stickInput = Gamepad.current.leftStick.x.ReadValue();
            if (Mathf.Abs(stickInput) > 0.1f) // Deadzone handling
            {
                horizontalInput = stickInput;
            }
        }

        // 2. Detect jump input using specific frame checks
        // UAT REQUIREMENT: Must use both an instant down check (wasPressedThisFrame) AND a continuous check (isPressed)
        if (Keyboard.current != null)
        {
            // GetKeyDown equivalent: fires once on initial press
            bool instantJumpPress = Keyboard.current.spaceKey.wasPressedThisFrame;

            // GetKey equivalent: true as long as the button is held down
            bool continuousJumpHold = Keyboard.current.spaceKey.isPressed;

            if (instantJumpPress && isGrounded)
            {
                jumpRequested = true;
            }
        }

        // UAT REQUIREMENT: Move at least one object by explicitly setting its position manually (Framerate Independent)
        if (customMovingPlatform != null)
        {
            // Modifying the transform position vector directly shifts the platform every frame draw smoothly
            customMovingPlatform.position += Vector3.right * (platformShiftSpeed * Time.deltaTime);

            // Simple boundary bounce code back and forth
            if (customMovingPlatform.position.x > 5f) platformShiftSpeed = -Mathf.Abs(platformShiftSpeed);
            if (customMovingPlatform.position.x < -5f) platformShiftSpeed = Mathf.Abs(platformShiftSpeed);
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

        // 5. Apply jump physics if a jump was requested in Update using physics acceleration forces
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
