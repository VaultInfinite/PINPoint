using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public enum TheJumpState
{
    JUMP,
    AIR,
    GROUND
}

/// <summary>
/// This is a temparary script
/// It will be placed into the Jump Function Script
/// Afterwards, this script will be deleted
/// </summary>
public class JumpTEMP : MonoBehaviour
{
    public TheJumpState playerState;


    private Rigidbody rb;

    [Header("Ground Check")]
    public float playerHeight;
    bool grounded;
    public float gravity;
    public float maxVel;
    public LayerMask Ground;
    public float jumpForce;
    public float jumpCD;
    public float airMulti;
    bool readyToJump = true;

    [Header("Double Jump")]
    public int extraJump;
    private int extraJumpNum;

    [Header("Velocity Viewer - TEMP")]
    public float xVel;
    public float yVel;
    public float zVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Start()
    {

        rb.freezeRotation = true;
        extraJumpNum = extraJump;

    }

    private void Update()
    {
        ViewVelocity();

        JumpManager();

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        if (grounded)
        {
            extraJump = extraJumpNum;
            //playerState = TheJumpState.GROUND;
        }
        else
        {
            playerState = TheJumpState.AIR;
        }

    }


    private void ViewVelocity()
    {
        xVel = rb.velocity.x;
        yVel = rb.velocity.y;
        zVel = rb.velocity.z;
    }

    private void JumpManager()
    {
        switch (playerState)
        {
            case TheJumpState.JUMP:
    

                //Prevent double jumping
                readyToJump = false;

                //Makes player jump the same height
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                //Jump
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

                //Jump Cooldown
                Invoke(nameof(ResetJump), jumpCD);
                

                break;
        }
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        //Prevents player from entering jumping state while in the air
        if (grounded && value.performed && readyToJump)
        {
            //Debug.Log("Initial Jump");
            playerState = TheJumpState.JUMP;
        }

        else if(value.performed && extraJump > 0)
        {
            extraJump--;
            //Debug.Log("Trying to double jump");
            playerState = TheJumpState.JUMP;
        }
        
    }

    /// <summary>
    /// Allows the player to jump again
    /// </summary>
    private void ResetJump()
    {
        playerState = TheJumpState.AIR;
        readyToJump = true;
    }
}
