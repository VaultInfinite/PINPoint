using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public partial class PlayerController : MonoBehaviour
{
    
    #region Variables

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float runSpeed;
    private float tempSpeed;

    public bool canMove = true;

    public bool doublejump;

    private MeshRenderer mr;
    private Rigidbody rb;

    [Header("Direction")]
    public Transform orientation;
    Vector3 moveInput;
    Vector3 moveDirection;


    [Header("Ground Check")]
    public float playerHeight;
    bool grounded;
    public float gravity;
    public float maxVel;
    public LayerMask Ground;
    public float jumpForce;
    public float jumpCD;
    public float airMulti;
    private bool jumping;

    //Crouching Code
    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchScaleY;
    private float startScaleY;

    private bool crouching = false;

    //Player State
    public bool freeze;
    public bool unlimited;
    public bool restricted;
    #endregion

    //Dictionary containing all the states the player can be in
    private readonly Dictionary<Type, State> _states = new()
    {
        {typeof(Walking), new Walking() },
        {typeof(Running), new Running() },
        {typeof(Jump), new Jump() },
        {typeof(Ledge), new Ledge() },
        {typeof(Crouch), new Crouch() },
        {typeof(Air), new Air() },
    };
    
    //The Input system
    public PlayerControllerInput input;

    //The key of the current state, default to walking
    private Type _state = typeof(Walking);

    private void Awake()
    {
        input = new();
        input.Enable();

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

    //Unity builtin fixed update
    private void FixedUpdate()
    {
        var state = _states[_state];
        state.OnFixedUpdate(this);
        Debug.Log(_state);
    }

    private void Update()
    {
        var state = _states[_state];
        state.OnUpdate(this);

        //Check if the player is touching the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);
    }

    //Sets the current state calling OnEnter on new state and OnExit on old state
    public void SetState<S>()
    where
        S : State
    {
        //Get old state
        var oldState = _states[_state];
        oldState.OnExit(this);
        //Get key of new state
        var key = typeof(S);
        //Set State
        _state = key;
        //Tell new state it has been entered
        var newState = _states[_state];
        newState.OnEnter(this);
    }

    /// <summary>
    /// Moves the player via the given inputs
    /// </summary>
    public void OnMovement(PlayerControllerInput.MovementActions input)
    {
        //Get inputs
        moveInput = new Vector3(input.Movement.ReadValue<Vector2>().x, 0, input.Movement.ReadValue<Vector2>().y);
    }

    /// <summary>
    /// Makes the player move towards where they are facing
    /// </summary>
    private void GetDirection()
    {
        //Get direction where the player was facing
        moveDirection = orientation.forward * moveInput.z + orientation.right * moveInput.x;

        //Apply movement in that direction
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    public void OnJump(InputAction.CallbackContext value)
    {

    }

    /// <summary>
    /// Controls how fast the player can go
    /// </summary>
    private void SpeedLimit()
    {
        //Get velocity from RB
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
}
