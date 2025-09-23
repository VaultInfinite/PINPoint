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
    public float groundDrag;

    private MeshRenderer mr;
    private Rigidbody rb;

    [Header("Direction")]
    public Transform orientation;

    [Header("Ground Check")]
    public float playerHeight;
    public float playerRadius;
    public LayerMask Ground;
    bool grounded;

    [Header("Speed Limit")]
    public AnimationCurve speedCurve;
    public float maxSpeedLimit;
    public float seeSoftLimit;
    #endregion

    //Dictionary containing all the states the player can be in // STATES MUST BE CALLED AS THEY ARE BELOW, AS WELL AS ADDED IN AWAKE TO BE CALLED
    public Walking walking;
    public Running running;
    public Jump jump;
    public Ledge ledge;
    public Crouch crouch;
    public Air air;
    public WallRunning wall;
    public Gliding gliding;
    public Aiming aiming;
    private readonly Dictionary<Type, State> _states = new();
    
    //The Input system
    public PlayerControllerInput input;

    //The key of the current state, default to walking
    private Type _state = typeof(Walking);

    //For player shooting
    [SerializeField] Gun gun;

    private void Awake()
    {
        input = new();
        input.Enable();

        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();

        _states.Add(typeof(Walking), walking);
        _states.Add(typeof(Running), running);
        _states.Add(typeof(Jump), jump);
        _states.Add(typeof(Ledge), ledge);
        _states.Add(typeof(Crouch), crouch);
        _states.Add(typeof(Air), air);
        _states.Add(typeof(WallRunning), wall);
        _states.Add(typeof(Gliding), gliding);
        _states.Add(typeof(Aiming), aiming);
    }

    private void Start()
    {
        //Turn of the renderer so that the player can't see the model
        mr.enabled = true;
        rb.freezeRotation = true;
    }

    //Unity builtin fixed update
    private void FixedUpdate()
    {
        var state = _states[_state];
        state.OnFixedUpdate(this);
        Debug.Log(_state);

        //for player shooting
        if(input.Movement.Shoot.IsPressed())
        {
            
        }
    }

    private void Update()
    {
        var state = _states[_state];

        //Check if the player is touching the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.01f, Ground);

        state.OnUpdate(this);
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
    public Vector3 GetMovement()
    {
        //Get inputs
        return new Vector3(input.Movement.Movement.ReadValue<Vector2>().x, 0, input.Movement.Movement.ReadValue<Vector2>().y);
    }

    /// <summary>
    /// Makes the player move towards where they are facing
    /// </summary>
    private Vector3 GetDirection()
    {
        Vector3 moveInput = GetMovement();
        //Get direction where the player was facing
        return orientation.forward * moveInput.z + orientation.right * moveInput.x;

        //Apply movement in that direction
        //rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }

    /// <summary>
    /// Controls how fast the player can go
    /// </summary>
    private void SpeedLimit(float speed)
    {
        //Get velocity from RB
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //float curveMult = speedCurve.Evaluate(speed / maxSpeedLimit);

        float curveMult = speedCurve.Evaluate(flatVel.magnitude / speed);


        

        //Limit velocity if needed
        if (flatVel.magnitude > speed)
        {
            //Calculate the speed it would be
            Vector3 limitedVel = flatVel.normalized * curveMult;

            //Apply speed limit
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
