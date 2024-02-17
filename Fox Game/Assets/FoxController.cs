using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    private float walkSpeed = 5f;
    private float jumpSpeed = 10f;
    private float crouchMultipler = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<RigidBody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void JumpAction() 
    { 
    
    }

    void CrouchAction() 
    {
        
    }

    void WalkAction() 
    { 
    
    }
}
