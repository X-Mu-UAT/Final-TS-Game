using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")][SerializeField] private float moveSpeed = 8f; [SerializeField] private float jumpForce = 12f; [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer; private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool jumpRequested; private void Start()
    {
        // Cache the Rigidbody2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        // 1. Gather horizontal movement input (A/D or Left/Right arrows)
        horizontalInput = Input.GetAxisRaw("Horizontal");// 2. Detect jump input (Spacebar by default)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }
    }
}