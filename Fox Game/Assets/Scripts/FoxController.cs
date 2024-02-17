using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    private float walkSpeed = 20f;
    private float jumpSpeed = 80f;
    private float crouchMultiplier = 2f;
    private bool isGrounded = true;
    private bool isCrouching;
    private Rigidbody2D rb;
    private Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleWalk();
        HandleJump();
        HandleCrouch();
    }

    public void HandleJump() 
    {
        CheckGrounded();
        if (InputHandler.Instance.IsJumpPressed() && isGrounded) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed); 
        }
    }

    public void HandleCrouch() 
    {
        if (InputHandler.Instance.IsCrouchPressed())
        {
            isCrouching = true;
            rb.velocity = new Vector2(rb.velocity.x * crouchMultiplier, rb.velocity.y);
        }
        else 
        {
            isCrouching = false;
        }
    }

    public void HandleWalk() 
    {
        float movementSpeed = InputHandler.Instance.IsWalkPressed() * walkSpeed;

        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
    }
    private void CheckGrounded() 
    {
        isGrounded = collider.IsTouchingLayers(LayerMask.GetMask("Floor"));
    }
}
