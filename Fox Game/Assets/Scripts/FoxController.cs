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
    private BoxCollider2D collider;
    private Animator animator;
    
    private float slideSpeed = 400f; // Adjust this value as needed
    private bool isSliding = false;
    private float slideDuration = 0.25f; // Duration of the slide in seconds
    private float slideTimer;

    private Vector2 originalColliderSize;
    private Vector2 crouchedColliderSize;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        // Explicitly set initial animation states
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsCrouching", false);
       
        originalColliderSize = collider.size;
        crouchedColliderSize = new Vector2(collider.size.x, collider.size.y * 0.5f);
    }

    void Update()
    {
        HandleWalk();
        HandleJump();
        HandleCrouch();
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                isSliding = false;
                //animator.SetBool("IsSliding", false); // Turn off sliding animation
                                                      // Optionally, reduce the character's speed gradually instead of stopping abruptly
            }
        }
    }

    public void HandleJump()
    {
        CheckGrounded();
        if (InputHandler.Instance.IsJumpPressed() && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsIdle", false);

        }
        else if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
            //animator.SetBool("IsIdle", true);
        }
    }

    public void HandleCrouch()
    {

        bool isCrouchPressed = InputHandler.Instance.IsCrouchPressed();
        isCrouching = isCrouchPressed;
        // Initiate sliding if the character is moving and crouch is pressed
        if (isCrouchPressed && Mathf.Abs(rb.velocity.x) > 0 && !isSliding)
        {
            collider.size = crouchedColliderSize;
            isSliding = true;
            slideTimer = slideDuration; // Reset the slide timer
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * slideSpeed, rb.velocity.y);
           
        }
        else if (!isCrouchPressed)
        {
            isCrouching = false;
            collider.size = originalColliderSize;

        }
        //animator.SetBool("IsIdle", !isCrouchPressed);
    }

    public void HandleWalk()
    {
        if (isSliding) return;
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
        bool isWalking = Mathf.Abs(rb.velocity.x) > 0;
        //if (isWalking)
        //{
        //    animator.SetFloat("WalkSpeed", rb.velocity.x < 0 ? -1.0f : 1.0f);
        //}
        
        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsIdle", !isWalking);
    }

    private void CheckGrounded()
    {
        isGrounded = collider.IsTouchingLayers(LayerMask.GetMask("Floor"));
        //// Since ground check can influence jumping animation, it's good to update it here too.
        animator.SetBool("IsJumping", !isGrounded && rb.velocity.y != 0);
    }
    public bool HandleStanding()
    {
        return false;
    }
}
