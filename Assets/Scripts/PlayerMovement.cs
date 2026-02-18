using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private LayerMask groundLayers;
    [Range(0, .3f)][SerializeField] private float movementSmoothing = 0.1f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private string horizontalInput = "Horizontal";
    [SerializeField] private KeyCode jumpKey = KeyCode.UpArrow;
    [SerializeField] private float fallMultiplier = 4f;
    [SerializeField] private float jumpMultiplier = 4f;
    [SerializeField] private float interactRadius = 0.5f;
    [SerializeField] private float groundRadius = 0.3f;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private float coyoteTime = 0.2f;

    private float horizontalMove;
    private bool jump = false;
    private bool facingRight = true;
    private float velocityX = 0f;
    private float coyoteTimeCounter;

    private Rigidbody2D rb;
    private Animator ani;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }
    private void Update()
    {
        horizontalMove = Input.GetAxisRaw(horizontalInput) * runSpeed;
        ani.SetFloat("Speed", Mathf.Abs(horizontalMove));
        ani.SetFloat("yVelocity", rb.linearVelocity.y);
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            ani.SetBool("IsJumping", false);
        } else
        {
            coyoteTimeCounter -= Time.deltaTime;
            ani.SetBool("IsJumping", true);
        }

        if (Input.GetKeyDown(jumpKey) && coyoteTimeCounter > 0f)
        {
            jump = true;
            coyoteTimeCounter = 0f;
            ani.SetBool("IsJumping", true);
        } 
    }

    private void FixedUpdate()
    {
        float targetVelocityX = horizontalMove;
        float newVelocityX = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocityX, ref velocityX, movementSmoothing);

        rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);

        if (horizontalMove > 0 && !facingRight)
        {
            Flip();
        } else if (horizontalMove < 0 && facingRight)
        {
            Flip();
        }

        if(jump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }

        ApplyGravityMultipliers();
    }

    private void ApplyGravityMultipliers()
    {
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        } else if (rb.linearVelocity.y > 0f && !Input.GetKey(jumpKey))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (jumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundPosition.position, groundRadius, groundLayers);
    }

    private void Flip() 
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(playerPosition.position, interactRadius);
        }

        if (groundPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundPosition.position, groundRadius);
        }
    }
}
