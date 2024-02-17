using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
   

    private float walkSpeed = 50f;
    private float jumpSpeed = 200f;
    private float crouchMultiplier = 0.5f; // Assuming this reduces speed for crouching
    private bool isGrounded = true;
    private bool isCrouching;
    private Rigidbody2D rb;
    private Collider2D collider;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Explicitly set initial animation states
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsCrouching", false);
    }

    void Update()
    {

        HandleWalk();
        HandleJump();
        HandleCrouch();
    }

    public void HandleJump()
    {
        if (InputHandler.Instance.IsJumpPressed())

            CheckGrounded();
        if (InputHandler.Instance.IsJumpPressed() && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            animator.SetBool("IsJumping", true);
        }
        else if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    public void HandleCrouch()
    {
        bool isCrouchPressed = InputHandler.Instance.IsCrouchPressed();
        isCrouching = isCrouchPressed;
        if (isCrouchPressed)
        {
            rb.velocity = new Vector2(rb.velocity.x * crouchMultiplier, rb.velocity.y);
        }
        animator.SetBool("IsCrouching", isCrouchPressed);
    }

    public void HandleWalk()
    {
        float horizontalInput = InputHandler.Instance.IsWalkPressed();
        float targetVelocityX = horizontalInput * walkSpeed;
        float velocityChangeX = targetVelocityX - rb.velocity.x;
        float maxVelocityChange = (isGrounded ? walkSpeed : walkSpeed * 0.2f); // Limit air strafe velocity change

        if (isGrounded)
        {
            // Apply full control on the ground
            rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);
        }
        else
        {
            // In air, apply limited control based on maxVelocityChange
            // Calculate velocity change within the allowed limit
            velocityChangeX = Mathf.Clamp(velocityChangeX, -maxVelocityChange, maxVelocityChange);
            rb.velocity = new Vector2(rb.velocity.x + velocityChangeX, rb.velocity.y);
        }

        // Here we ensure that IsWalking is only true if there is significant movement.
        bool isWalking = Mathf.Abs(walkSpeed) > 0;

        animator.SetBool("IsWalking", isWalking);
    }

    private void CheckGrounded()
    {
        isGrounded = collider.IsTouchingLayers(LayerMask.GetMask("Floor"));
        // Since ground check can influence jumping animation, it's good to update it here too.
        animator.SetBool("IsJumping", !isGrounded && rb.velocity.y != 0);
    }
    private bool HandleStanding()
    {
        return false;
    }
}
