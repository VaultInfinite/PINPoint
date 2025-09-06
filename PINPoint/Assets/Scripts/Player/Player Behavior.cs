using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

enum State
{
    IDLE,
    WALK,
    RUN,
    JUMP,
    AIRMOVE
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

    private Rigidbody rb;

    //Movement
    Vector3 moveInput;

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
    }


    public void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);


        rb.AddForce(moveInput * moveSpeed, ForceMode.Force);
    }


    /// <summary>
    /// Moves the player via the given inputs
    /// </summary>
    void OnMovement(InputValue value)
    {
        //moveInput = value.Get<Vector2>();

        moveInput = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);

  
        playerState = State.WALK;
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    void OnJump(InputValue value)
    {
        if ((value.isPressed) && readyToJump && grounded)
        {
            readyToJump = false;

            playerState = State.JUMP;

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
