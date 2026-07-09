using Unity.Hierarchy;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private LayerMask groundLayers;
    [Range(0, .3f)][SerializeField] private float movementSmoothing = 0.1f;
    [SerializeField] private KeyCode jumpKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode slideKey = KeyCode.DownArrow;
    [SerializeField] private float fallMultiplier = 6f;
    [SerializeField] private float interactRadius = 0.5f;
    [SerializeField] private float groundRadius = 0.3f;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private float coyoteTime = 0.1f;

    // Chỉnh thời gian giữ nút, Jump Force khi giữ phím nhảy
    [Header("Jump Settings")]
    [SerializeField] private float holdJumpTimer = 0.0f;
    [SerializeField] private float baseMaxHoldTime = 0.4f;
    [SerializeField] private float dynamicMaxHoldTime = 0.4f;
    [SerializeField] private float sustainedJumpForce = 25f;

    [Header("Run Settings")]
    [SerializeField] private float initialSpeed = 5f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float maxAcceleration = 1f;
    public float currentRunSpeed;

    [Header("Slide Settings")]
    [SerializeField] private float slideDuration = 0.8f;
    [SerializeField] private float offsetChange = 0.07f;
    private Vector2 originalOffset = Vector2.zero;
    private bool isSliding = false;
    private float slideTimer = 0f;

    private bool isHoldingJump = false;
    private bool jump = false;
    private float velocityX = 0f;
    private float coyoteTimeCounter;

    private Rigidbody2D rb;
    private Animator ani;

    private CircleCollider2D upCol;
    private CapsuleCollider2D downCol;

    public float JumpForce => jumpForce;
    public float SustainedJumpForce => sustainedJumpForce;
    public float BaseMaxHoldTime => baseMaxHoldTime;
    public float FallMultiplier => fallMultiplier;
    public float MaxSpeed => maxSpeed;
    public Rigidbody2D Rb => rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();

        upCol = GetComponent<CircleCollider2D>();
        downCol = GetComponent<CapsuleCollider2D>();

        originalOffset = downCol.offset;
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
            if (isSliding) StopSlide();

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

        if (Input.GetKeyDown(slideKey) && IsGrounded() && !isSliding)
        {
            StartSlide();
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                StopSlide();
            }
        }
    }

    private void FixedUpdate()
    {
        if(IsGrounded())
        {
            float velocityRatio = currentRunSpeed / maxSpeed;
            acceleration = maxAcceleration * (1 - velocityRatio);
            dynamicMaxHoldTime = baseMaxHoldTime * velocityRatio;
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
            if (holdJumpTimer < dynamicMaxHoldTime)
            {
                rb.AddForce(Vector2.up * sustainedJumpForce, ForceMode2D.Force);
                holdJumpTimer += Time.fixedDeltaTime;
            }
        }

        ApplyGravityMultipliers();
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        ani.SetBool("IsSliding", true);

        if (upCol != null)
        {
            upCol.enabled = false;
        }

        if (downCol != null)
        {
            downCol.offset = new Vector2(originalOffset.x, originalOffset.y + offsetChange);
        }
    }

    private void StopSlide()
    {
        isSliding = false;
        ani.SetBool("IsSliding", false);

        if (upCol != null)
        {
            upCol.enabled = true;
        }

        if (downCol != null)
        {
            downCol.offset = originalOffset;
        }
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
