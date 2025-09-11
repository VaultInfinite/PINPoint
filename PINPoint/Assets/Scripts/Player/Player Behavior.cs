using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState
{
    freeze,
    unlimited,
}

/// <summary>
/// Joseph Acuna 9/3/25 11:27pm
/// 
/// This script handles the player's movement
/// </summary>
public class PlayerBehavior : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float runSpeed;
    private float tempSpeed;

    public bool canMove = true;

    public float knownVel;

    bool sprinting;

    private MeshRenderer mr;
    private Rigidbody rb;

    //Movement
    Vector3 moveInput;
    Vector3 moveDirection;
    public Transform orientation;

    [Header("Ground Check")]
    public float playerHeight;
    bool grounded;
    public float gravity;
    public float maxVel;
    public LayerMask whatIsGround;
    public float jumpForce;
    public float jumpCD;
    public float airMulti;
    bool readyToJump = true;

    //Crouching Code
    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchScaleY;
    private float startScaleY;

    private bool crouching = false;

    //Player STate
    public bool freeze;
    public bool unlimited;
    public bool restricted;

    //LedgeGrabbing
    public MovementState state;

    State playerState;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        tempSpeed = moveSpeed;
    }

    private void Start()
    {
        //Turn of the renderer so that the player can't see the model
        mr.enabled = true;
        rb.freezeRotation = true;
        //Finding Size of Player
        startScaleY = transform.localScale.y;
    }

    public void Update()
    {
        //This is just to see the player's velocity; can be deleted
        //knownVel = rb.velocity.y;
        //Mathf.Abs(knownVel);

        //Check if the player is touching the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        GetDirection();
        StateHandler();
        
        if (!canMove)
        {
            Debug.Log("look! im grabbing the ledge!");
            return;

        }
        

        if (!crouching)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }else if (crouching)
        {
            rb.AddForce(moveDirection.normalized * crouchSpeed * 10f, ForceMode.Force);
        }

            SpeedLimit();

        if (grounded)
        {
            rb.drag = groundDrag;
            if (!sprinting)
            {
                moveSpeed = tempSpeed;
            }
        }
        else
        {
            //Move quickly in air
            rb.drag = 0;

            Vector3 vel = rb.velocity;
            vel.y -= gravity * Time.deltaTime;
            rb.velocity = vel;


            Vector3 flatVel = new Vector3(0f, rb.velocity.y, 0f);
            if (flatVel.magnitude > maxVel)
            {
                Vector3 limitVel = flatVel.normalized * maxVel;

                rb.velocity = new Vector3(rb.velocity.x, limitVel.y, rb.velocity.z);
            }
        }


        }


    /// <summary>
    /// Moves the player via the given inputs
    /// </summary>
    public void OnMovement(InputAction.CallbackContext value)
    {
        //moveInput = value.Get<Vector2>();

        moveInput = new Vector3(value.ReadValue<Vector2>().x, 0, value.ReadValue<Vector2>().y);

        
    }

    /// <summary>
    /// Makes the player move towards where they are facing
    /// </summary>
    private void GetDirection()
    {
        moveDirection = orientation.forward * moveInput.z + orientation.right * moveInput.x;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMulti, ForceMode.Force);
        }
    }

    /// <summary>
    /// Makes the player move faster when holding down the sprint button
    /// </summary>
    /// <param name="value"></param>
    public void OnSprint(InputAction.CallbackContext value)
    {
        if (value.performed && grounded && !crouching)
        {
            sprinting = true;
            Debug.Log("Im Running!");
            moveSpeed = runSpeed;
        }else if (value.canceled)
        {
            sprinting = false;
            Debug.Log("Im walkin here!");
            //moveSpeed = tempSpeed;
        }

        
    }

    /// <summary>
    /// Controls how fast the player can go
    /// </summary>
    private void SpeedLimit()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            //Calculate the speed it would be
            Vector3 limitedVel = flatVel.normalized * moveSpeed;

            //Apply speed limit
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    public void OnJump(InputAction.CallbackContext value)
    {
        if ((value.performed) && readyToJump && grounded && !crouching)
        {
            readyToJump = false;

            //Makes player jump the same height
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Jump
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            //Fall down logic


            Invoke(nameof(ResetJump), jumpCD);
   
        }
    }

    private void ResetJump()
    {
        
        readyToJump = true;
    }
    
    //need code for "if (restricted) return;" so that if the player is in the return state, they cannot move with their keys

    private void StateHandler()
    {
        switch (state)
        {
            case MovementState.freeze:
                Debug.Log("im cold");
                state = MovementState.freeze;
                canMove = false;
                rb.velocity = Vector3.zero;
                break;

            case MovementState.unlimited:
                Debug.Log("im warm");
                state = MovementState.unlimited;
                canMove = true;
                //moveSpeed = 99f;
                break;
        }
    }

    public void OnCrouch(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            //Shrinks Player
            transform.localScale = new Vector3(transform.localScale.x, crouchScaleY, transform.localScale.z);
            //Pushes player down so they dont float in the air when crouching
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            crouching = true;
            sprinting = false;
        }
        if (value.canceled)
        {
            //if player lets go of the crouch key, they will go back to normal
            transform.localScale = new Vector3(transform.localScale.x, startScaleY, transform.localScale.z);
            crouching = false;
        }
    }
}
