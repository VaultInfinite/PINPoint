using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private MeshRenderer mr;
    private Rigidbody rb;

    //Movement
    Vector3 moveInput;
    Vector3 moveDirection;
    public Transform orientation;

    [Header("Ground Check")]
    public float playerHeight;
    bool grounded;
    public LayerMask whatIsGround;
    public float jumpForce;
    public float jumpCD;
    public float airMulti;
    bool readyToJump = true;
   
    State playerState;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        //Turn of the renderer so that the player can't see the model
        mr.enabled = false;
        rb.freezeRotation = true;
    }

    public void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        GetDirection();
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        SpeedLimit();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else rb.drag = 0;
    }


    /// <summary>
    /// Moves the player via the given inputs
    /// </summary>
    public void OnMovement(InputAction.CallbackContext value)
    {
        //moveInput = value.Get<Vector2>();

        moveInput = new Vector3(value.ReadValue<Vector2>().x, 0, value.ReadValue<Vector2>().y);

        
    }

    private void GetDirection()
    {
        moveDirection = orientation.forward * moveInput.z + orientation.right * moveInput.x;
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("Im Running!");
            tempSpeed = moveSpeed;
            moveSpeed = runSpeed;
        }else if (value.canceled)
        {
            Debug.Log("Im walkin here!");
            moveSpeed = tempSpeed;
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
        if ((value.performed) && readyToJump && grounded)
        {
            readyToJump = false;

            //Makes player jump the same height
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Jump
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCD);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }


}
