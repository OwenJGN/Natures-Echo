using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
   

    private float walkSpeed = 100f;
    private float jumpSpeed = 400f;
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



    private bool isRunning = false;
    private float lastTapTime = 0f;
    private float doubleTapTime = 0.25f; 
    private float runSpeedMultiplier = 2f; 

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
<<<<<<< HEAD
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
=======
        HandleDoubleTap();
        CheckGrounded();
        HandleJump();
        HandleCrouch();
        HandleWalk();
        HandleRun(); 
        UpdateSliding();
>>>>>>> 8ff721beebbb008d7a6089404cf6e9a7b3384d66
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
<<<<<<< HEAD
        // Initiate sliding if the character is moving and crouch is pressed
        if (isCrouchPressed && Mathf.Abs(rb.velocity.x) > 0 && !isSliding)
=======

        // Ensure sliding can only be initiated if the character is running
        if (isCrouchPressed && Mathf.Abs(rb.velocity.x) > 0 && !isSliding && isRunning)
>>>>>>> 8ff721beebbb008d7a6089404cf6e9a7b3384d66
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

<<<<<<< HEAD
=======

>>>>>>> 8ff721beebbb008d7a6089404cf6e9a7b3384d66
    public void HandleWalk()
    {
        if (isSliding) return; 

        float horizontalInput = InputHandler.Instance.IsWalkPressed();
<<<<<<< HEAD
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
=======

        if (Mathf.Abs(horizontalInput) == 0)
        {
            isRunning = false;
        }

        float targetVelocityX = horizontalInput * (isRunning ? walkSpeed * runSpeedMultiplier : walkSpeed);
        rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);

        bool isWalking = Mathf.Abs(rb.velocity.x) > 0;
        animator.SetBool("IsWalking", isWalking && !isRunning); 
        //animator.SetBool("IsRunning", isRunning); 
>>>>>>> 8ff721beebbb008d7a6089404cf6e9a7b3384d66
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
<<<<<<< HEAD
        return false;
=======
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
>>>>>>> 8ff721beebbb008d7a6089404cf6e9a7b3384d66
    }
}
