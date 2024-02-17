using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    private float walkSpeed = 5f;
    private float jumpSpeed = 10f;
    private float crouchMultiplier = 2f;
    private bool isGrounded;
    private bool isCrouching;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
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
        if (InputHandler.Instance.IsJumpPressed()) 
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

        rb.velocity = new Vector2(rb.velocity.x, movementSpeed);
    }
}
