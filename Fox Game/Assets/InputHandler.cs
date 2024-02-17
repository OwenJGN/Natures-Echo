using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// For Checking what key has been pressed
    /// </summary>
    /// <returns> 0 for false, 1 for true</returns>
    
    public float IsWalkPressed()
    {
        return Input.GetAxis("Walk");
    }

    public bool IsJumpPressed()
    {
        return Input.GetButtonDown("Jump");
    }

    public bool IsCrouchPressed() 
    {
        return Input.GetButtonDown("Crouch");
    }
    public bool IsCrouchDown() 
    {
        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
