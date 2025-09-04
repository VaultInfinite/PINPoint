using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float moveSpeed;
    public float jumpHeight;
    public float runSpeed;

    private Rigidbody rb;

    //Movement
    Vector3 moveInput;
    bool hasJumped = false;
    bool jumpCheck = false;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //Hide Mouse and lock to screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        rb.AddForce(moveInput * moveSpeed);

        if (!jumpCheck) return;
        CheckGround();
    }

    /// <summary>
    /// Allows the player to control the camera
    /// </summary>
    public void CameraControl()
    {
        
    }

    /// <summary>
    /// Moves the player via the given inputs
    /// </summary>
    void OnMovement(InputValue value)
    {
        //moveInput = value.Get<Vector2>();

        moveInput = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    void OnJump(InputValue value)
    {
        if ((value.isPressed) && (!hasJumped))
        {
            hasJumped = true;
            Debug.Log("Jump!");
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.y);
            StartCoroutine(JumpRefresh(1f));
        }
    }

    /// <summary>
    /// Checks to see if the player is on the ground
    /// </summary>
    private void CheckGround()
    {
        RaycastHit groundCheck;
        float checkDist = 2f;
        if (Physics.Raycast(transform.position, -Vector3.up, out groundCheck, checkDist))
        {
            if (groundCheck.collider.tag == "Ground")
            {
                hasJumped = false;
                jumpCheck = false;
            }

        }
    }

    /// <summary>
    /// The amount of time to check for the ground
    /// </summary>
    private IEnumerator JumpRefresh(float jumpTimer)
    {
        yield return new WaitForSeconds(jumpTimer);

        jumpCheck = true;
    }
  
}
