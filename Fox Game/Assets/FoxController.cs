using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
<<<<<<< Updated upstream:Fox Game/Assets/FoxController.cs
    private float walkSpeed = 5f;
    private float jumpSpeed = 10f;
    private float crouchMultiplier = 2f;
    private bool isGrounded;
    private bool isCrouching;
    private Rigidbody2D rb;
=======
    private float walkSpeed = 50f;
    private float jumpSpeed = 200f;
    private float crouchMultiplier = 0.5f; // Assuming this reduces speed for crouching
    private bool isGrounded = true;
    private bool isCrouching;
    private Rigidbody2D rb;
    private Collider2D collider;
    private Animator animator;
>>>>>>> Stashed changes:Fox Game/Assets/Scripts/FoxController.cs

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
<<<<<<< Updated upstream:Fox Game/Assets/FoxController.cs
        if (InputHandler.Instance.IsJumpPressed()) 
=======
        CheckGrounded();
        if (InputHandler.Instance.IsJumpPressed() && isGrounded)
>>>>>>> Stashed changes:Fox Game/Assets/Scripts/FoxController.cs
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
        float movementSpeed = InputHandler.Instance.IsWalkPressed() * walkSpeed;
<<<<<<< Updated upstream:Fox Game/Assets/FoxController.cs

        rb.velocity = new Vector2(rb.velocity.x, movementSpeed);
=======
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);

        // Here we ensure that IsWalking is only true if there is significant movement.
        bool isWalking = Mathf.Abs(movementSpeed) > 0;
        Debug.Log
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
        if()
>>>>>>> Stashed changes:Fox Game/Assets/Scripts/FoxController.cs
    }
}
