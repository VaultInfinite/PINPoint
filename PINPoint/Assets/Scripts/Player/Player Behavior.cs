using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState
{
    IDLE,
    MOVE,
    JUMP,
    AIR,
    CROUCH,
    LEDGE
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
    public MovementState playerState;
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

        //Initiate Player State
        playerState = MovementState.IDLE;
    }

    public void Update()
    {
        StateManager();

        //Check if the player is touching the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //Makes the player enter the AIR state
        if (!grounded)
        {
            playerState = MovementState.AIR;
        }
    }

    /// <summary>
    /// Manages the player's state
    /// </summary>
    private void StateManager()
    {
        switch (playerState)
        {
            case MovementState.IDLE:
                canMove = true;
                break;

            case MovementState.MOVE:
                
                GetDirection();

                //Slow player down when on the ground
                rb.drag = groundDrag;

                //Check if player is sprinting
                if (sprinting)
                {
                    moveSpeed = runSpeed;
                }
                else if (!sprinting)
                {
                    moveSpeed = tempSpeed;
                }

                //Apply movement to avatar
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

                

                //Prevent the player from going too fast
                SpeedLimit();

                break;

            case MovementState.JUMP:
                LedgeGrab();

                if (playerState == MovementState.AIR) return;

                if (readyToJump && grounded && !crouching)
                {

                    readyToJump = false;

                    //Makes player jump the same height
                    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                    //Jump
                    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

                    //Fall down logic


                    Invoke(nameof(ResetJump), jumpCD);

                }

                break;

            case MovementState.AIR:
                LedgeGrab();

                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMulti, ForceMode.Force);


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

                
                if (grounded) playerState = MovementState.MOVE;
                break;

            case MovementState.CROUCH:
                rb.AddForce(moveDirection.normalized * crouchSpeed * 10f, ForceMode.Force);

                //Shrinks Player
                transform.localScale = new Vector3(transform.localScale.x, crouchScaleY, transform.localScale.z);

                //Pushes player down so they dont float in the air when crouching
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                crouching = true;
                sprinting = false;

                break;


        }
    }

    /// <summary>
    /// Moves the player via the given inputs
    /// </summary>
    public void OnMovement(InputAction.CallbackContext value)
    {
        
        if (!canMove) return;
        playerState = MovementState.MOVE;

        moveInput = new Vector3(value.ReadValue<Vector2>().x, 0, value.ReadValue<Vector2>().y);

        if (value.canceled) playerState = MovementState.IDLE;
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
        if (grounded && !crouching)
        {
            sprinting = true;
        }
        
        if (value.canceled)
        {
            sprinting = false;
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
        //Prevents player from entering jumping state while in the air
        if (playerState == MovementState.AIR) return;
        playerState = MovementState.JUMP;
    }

    /// <summary>
    /// Allows the player to jump again
    /// </summary>
    private void ResetJump()
    {
        playerState = MovementState.AIR;
        readyToJump = true;
    }
    
    //need code for "if (restricted) return;" so that if the player is in the return state, they cannot move with their keys



    public void OnCrouch(InputAction.CallbackContext value)
    {
        playerState = MovementState.CROUCH;
        if (value.canceled)
        {
            playerState = MovementState.IDLE;

            //if player lets go of the crouch key, they will go back to normal
            transform.localScale = new Vector3(transform.localScale.x, startScaleY, transform.localScale.z);
            crouching = false;
        }
    }

    bool hanging;

    void LedgeGrab()
    {
        if (rb.velocity.y < 0 && !hanging)
        {
            RaycastHit downHit;
            Vector3 lineDownStart = (transform.position + Vector3.up*1.5f) + transform.forward *1;
            Vector3 LineDownEnd = (transform.position + Vector3.up * 0.8f) + transform.forward * 1;
            Physics.Linecast(lineDownStart, LineDownEnd, out downHit, LayerMask.GetMask("whatIsGround"));
            Debug.DrawLine(lineDownStart, LineDownEnd);

            if (downHit.collider != null)
            {
                RaycastHit fwdHit;
                Vector3 lineFwdStart = new Vector3(transform.position.x,downHit.point.y-0.1f,transform.position.z);
                Vector3 LineFwdEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward;
                Physics.Linecast(lineDownStart, LineDownEnd, out fwdHit, LayerMask.GetMask("whatIsGround"));
                Debug.DrawLine(lineDownStart, LineDownEnd);

                if (fwdHit.collider != null)
                {
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;

                    hanging = true;
                    //possible animation here

                    Vector3 hangPos = new Vector3(fwdHit.point.x,downHit.point.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -1f;
                    hangPos += offset;
                    transform.position = hangPos;
                    transform.forward = -fwdHit.normal;
                }
            }
        }
    }
}
