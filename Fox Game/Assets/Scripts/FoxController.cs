using UnityEngine;

public class FoxController : MonoBehaviour
{
    private float walkSpeed = 100f;
    private float jumpSpeed = 400f;
    private float crouchMultiplier = 0.5f;
    private float slideSpeed = 400f;
    private float slideDuration = 0.5f;

    private bool isGrounded;
    private bool isCrouching;
    private bool isSliding;
    private float slideTimer;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;

    private Vector2 originalColliderSize;
    private Vector2 crouchedColliderSize;
    private Vector2 originalColliderOffset;

    private bool isRunning = false;
    private float lastTapTime = 0f;
    private float doubleTapTime = 0.25f;
    private float runSpeedMultiplier = 2f;
    private LayerMask groundLayerMask;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        groundLayerMask = LayerMask.GetMask("Floor");
        originalColliderSize = boxCollider.size;
        crouchedColliderSize = new Vector2(boxCollider.size.x, boxCollider.size.y * 0.5f);
        crouchedColliderSize = new Vector2(boxCollider.size.x, boxCollider.size.y * 0.5f);
    }

    void Update()
    {
        HandleDoubleTap();
        CheckGrounded();
        HandleJump();
        HandleCrouch();
        HandleWalk();
        HandleRun();
        UpdateSliding();

    }

    public void AlignWithSlope(Vector2 groundNormal)
    {
        float slopeAngle = Mathf.Atan2(groundNormal.y, groundNormal.x) * Mathf.Rad2Deg;

        slopeAngle -= 90; 

        transform.rotation = Quaternion.Euler(0, 0, slopeAngle);
    }

    private void HandleJump()
    {
        if (InputHandler.Instance.IsJumpPressed() && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", !isGrounded);
        }
    }

    public void HandleCrouch()
    {
        bool isCrouchPressed = InputHandler.Instance.IsCrouchPressed();
        isCrouching = isCrouchPressed;

        if (isCrouchPressed && Mathf.Abs(rb.velocity.x) > 0 && !isSliding && isRunning)
        {
            StartSliding();
        }
        else if (!isCrouchPressed && isCrouching)
        {
            StopCrouching();
        }
    }


    public void HandleWalk()
    {
        float horizontalInput = InputHandler.Instance.IsWalkPressed();

        if (isGrounded)
        {
            float targetVelocityX = horizontalInput * walkSpeed;
            rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);
        }
        
        bool isWalking = Mathf.Abs(rb.velocity.x) > 0 && isGrounded;
        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsIdle", !isWalking);
    }


    private void CheckGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 30f; 

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayerMask);
        isGrounded = hit.collider != null;
        if (isGrounded)
        {
            AlignWithSlope(hit.normal);
        }
        else
        {
            
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void StartSliding()
    {
        isSliding = true;
        isCrouching = true;
        slideTimer = slideDuration;
        rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * slideSpeed, rb.velocity.y);
        boxCollider.size = crouchedColliderSize;
        boxCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y + (crouchedColliderSize.y - originalColliderSize.y) / 2);
        //animator.SetBool("IsSliding", true);
    }


    private void UpdateSliding()
    {
        if (!isSliding) return;

        slideTimer -= Time.deltaTime;
        if (slideTimer <= 0)
        {
            StopSliding();
        }
    }

    private void StopCrouching()
    {
        if (!isSliding)
        {
            isCrouching = false;
            boxCollider.size = originalColliderSize;
            boxCollider.offset = originalColliderOffset;
        }
    }

    private void StopSliding()
    {
        isSliding = false;
        //animator.SetBool("IsSliding", false);
        boxCollider.size = originalColliderSize;
        boxCollider.offset = originalColliderOffset;
        if (!InputHandler.Instance.IsCrouchPressed())
        {
            StopCrouching();
        }
    }
    private void HandleDoubleTap()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - lastTapTime < doubleTapTime)
            {
                isRunning = true;
            }
            lastTapTime = Time.time;
        }
    }
    private void HandleRun()
    {
        // Determine if the character is running based on the velocity and the isRunning flag.
        bool isCurrentlyRunning = isRunning && Mathf.Abs(rb.velocity.x) > 0;
        Debug.Log(isCurrentlyRunning);
        // Update the Animator's IsRunning parameter to reflect the current running state.
        animator.SetBool("IsRunning", isCurrentlyRunning);
        animator.SetBool("IsWalking", !isCurrentlyRunning);

        // If the character is running, adjust the velocity for running speed.
        if (isCurrentlyRunning)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * walkSpeed * runSpeedMultiplier, rb.velocity.y);
        }

        // Reset the isRunning flag if the character stops (This might need adjustment based on your game's logic).
        if (!isCurrentlyRunning)
        {
            isRunning = false;
        }
    }

}
