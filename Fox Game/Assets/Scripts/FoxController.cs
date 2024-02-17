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
        CheckGrounded();
        HandleJump();
        HandleCrouch();
        HandleWalk();
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

    private void HandleCrouch()
    {
        bool isCrouchPressed = InputHandler.Instance.IsCrouchPressed();
        if (isCrouchPressed && Mathf.Abs(rb.velocity.x) > 0 && !isSliding)
        {
            StartSliding();
        }
        else if (!isCrouchPressed && isCrouching)
        {
            StopCrouching();
        }
    }

    private void HandleWalk()
    {
        if (isSliding) return;
        float horizontalInput = InputHandler.Instance.IsWalkPressed();
        float targetVelocityX = horizontalInput * walkSpeed;
        float velocityChangeX = targetVelocityX - rb.velocity.x;
        float maxVelocityChange = (isGrounded ? walkSpeed : walkSpeed * 0.2f); 

        if (isGrounded)
        {
            rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);
        }
        else
        {
            velocityChangeX = Mathf.Clamp(velocityChangeX, -maxVelocityChange, maxVelocityChange);
            rb.velocity = new Vector2(rb.velocity.x + velocityChangeX, rb.velocity.y);
        }
        
        bool isWalking = Mathf.Abs(rb.velocity.x) > 0;

        animator.SetBool("IsWalking", isWalking);
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
        animator.SetBool("IsSliding", true);
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
        if (!isSliding) // Only reset if not currently sliding
        {
            isCrouching = false;
            boxCollider.size = originalColliderSize;
            boxCollider.offset = originalColliderOffset;
        }
    }

    private void StopSliding()
    {
        isSliding = false;
        animator.SetBool("IsSliding", false);
        // Reset collider size and offset after sliding
        boxCollider.size = originalColliderSize;
        boxCollider.offset = originalColliderOffset;
        if (!InputHandler.Instance.IsCrouchPressed())
        {
            StopCrouching(); // Ensure crouching is stopped if crouch is not being pressed
        }
    }
}
