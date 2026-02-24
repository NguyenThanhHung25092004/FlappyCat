using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private LayerMask groundLayers;
    [Range(0, .3f)][SerializeField] private float movementSmoothing = 0.1f;
    [SerializeField] private KeyCode jumpKey = KeyCode.UpArrow;
    [SerializeField] private float fallMultiplier = 4f;
    [SerializeField] private float interactRadius = 0.5f;
    [SerializeField] private float groundRadius = 0.3f;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private float coyoteTime = 0.2f;

    // Chỉnh thời gian giữ nút, Jump Force khi giữ phím nhảy
    [Header("Jump Settings")]
    [SerializeField] private float holdJumpTimer = 0.0f;
    [SerializeField] private float baseMaxHoldTime = 0.4f;
    [SerializeField] private float dynamicMaxHoldTime = 0.4f;
    [SerializeField] private float speedHoldMultiplier = 0.02f;
    [SerializeField] private float sustainedJumpForce = 15f;

    [Header("Run Settings")]
    [SerializeField] private float initialSpeed = 5f;
    [SerializeField] private float maxSpeed = 25f;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float maxAcceleration = 1f;
    private float currentRunSpeed;

    private bool isHoldingJump = false;
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

    private void Start()
    {
        currentRunSpeed = initialSpeed;
    }
    private void Update()
    {
        ani.SetFloat("Speed", Mathf.Abs(currentRunSpeed));
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
            isHoldingJump = true;
            holdJumpTimer = 0.0f;
            coyoteTimeCounter = 0f;
            ani.SetBool("IsJumping", true);
        }

        if (Input.GetKeyUp(jumpKey))
        {
            isHoldingJump = false;
        }
    }

    private void FixedUpdate()
    {
        if(IsGrounded())
        {
            if (currentRunSpeed < maxSpeed)
            {
                currentRunSpeed += acceleration * Time.fixedDeltaTime;
            }
        }

        float targetVelocityX = currentRunSpeed;
        float newVelocityX = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocityX, ref velocityX, movementSmoothing);

        rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);

        if(jump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }

        if(isHoldingJump)
        {
            float velocityRatio = currentRunSpeed / maxSpeed;
            acceleration = maxAcceleration * (1 - velocityRatio);
            dynamicMaxHoldTime = baseMaxHoldTime * velocityRatio;
            if (holdJumpTimer < dynamicMaxHoldTime)
            {
                rb.AddForce(Vector2.up * sustainedJumpForce, ForceMode2D.Force);
                holdJumpTimer += Time.fixedDeltaTime;
            }
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
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundPosition.position, groundRadius, groundLayers);
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
