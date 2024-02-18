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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

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

        // Ensure sliding can only be initiated if the character is running
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
        if (isSliding) return; 

        float horizontalInput = InputHandler.Instance.IsWalkPressed();

        if (Mathf.Abs(horizontalInput) == 0)
        {
            isRunning = false;
        }

        float targetVelocityX = horizontalInput * (isRunning ? walkSpeed * runSpeedMultiplier : walkSpeed);
        rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);

        bool isWalking = Mathf.Abs(rb.velocity.x) > 0;
        animator.SetBool("IsWalking", isWalking && !isRunning); 
        //animator.SetBool("IsRunning", isRunning); 
        animator.SetBool("IsIdle", !isWalking);
    }

    private void CheckGrounded()
    {
        isGrounded = boxCollider.IsTouchingLayers(LayerMask.GetMask("Floor"));
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
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
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
        if (isRunning && Mathf.Abs(rb.velocity.x) > 0)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * walkSpeed * runSpeedMultiplier, rb.velocity.y);
        }
        else
        {
            isRunning = false;
        }
    }
}
