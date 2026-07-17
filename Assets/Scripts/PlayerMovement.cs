using UnityEngine;
// 1. Add this namespace at the very top
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    private void Awake()
    {
        //Grabs references for rigidbody and animator from game object. 
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 2. Setup temporary variables for your inputs
        float horizontalInput = 0f;
        bool jumpPressed = false;

        // 3. Read input from the new Input System
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontalInput = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontalInput = 1f;

            jumpPressed = Keyboard.current.spaceKey.isPressed;
        }

        // 4. Your physics logic using linearVelocity remains perfectly intact
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        //Flip player when facing left/right. 
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // 5. Use the new jump boolean here
        if (jumpPressed && grounded)
            Jump();

        //sets animation parameters 
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, speed);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Performance optimization: CompareTag is better than .tag ==
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }
}
