using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public partial class PlayerController : MonoBehaviour
{
    
    #region Variables

    [Header("Movement")]
    public float groundDrag;

    private MeshRenderer mr;
    [HideInInspector]
    public Rigidbody rb;

    [Header("Direction")]
    public Transform orientation;
    public float wallCameraAngle;
    private float roll;

    [Header("Ground Check")]
    public float playerHeight;
    public float playerRadius;
    public LayerMask Ground;
    bool grounded;

    [Header("Shooting")]
    public Shoot shootScr;


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
    //[SerializeField] Gun gun;

    //bool to eventually stun player if the police drone shoots them
    public bool stun = false;


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

        //aiming.reticle.SetActive(false);
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
    }

    private void Update()
    {
        var state = _states[_state];

        //Check if the player is touching the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.01f, Ground);

        if (state is WallRunning)
        {
            if (wall.IsOnLeftWall(this))
            {
                roll = Mathf.Lerp(roll, -wallCameraAngle, Time.deltaTime * 3);
            }
            else
            {
                roll = Mathf.Lerp(roll, wallCameraAngle, Time.deltaTime * 3);
            }
            Camera.main.transform.localEulerAngles = new Vector3(0, 0, roll);
        }
        else
        {
            roll = Mathf.Lerp(roll, 0, Time.deltaTime * 3);
            Camera.main.transform.localEulerAngles = new Vector3(0, 0, roll);
        }
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
        //if the player is not stunned, then they can move
        /*if(stun == false)
        {
            //Get inputs
            return new Vector3(input.Movement.Movement.ReadValue<Vector2>().x, 0, input.Movement.Movement.ReadValue<Vector2>().y);
        }*/
        //Get inputs
        return new Vector3(input.Movement.Movement.ReadValue<Vector2>().x, 0, input.Movement.Movement.ReadValue<Vector2>().y);
    }

    /// <summary>
    /// Makes the player move towards where they are facing
    /// </summary>
    private Vector3 GetDirection()
    {
        Vector3 moveInput = GetMovement().normalized;
        //Get direction where the player was facing
        return orientation.forward * moveInput.z + orientation.right * moveInput.x;
    }

    /// <summary>
    /// Controls how fast the player can go
    /// </summary>
    private void Accelerate(Vector3 moveDirection, float maxSpeed, float acceleration)
    {
        //if the player isn't stunned, then they can move
        if(stun == false)
        {
            Vector3 velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            float product = Vector3.Dot(moveDirection, velocity);
            float accel = acceleration * Time.fixedDeltaTime;
            if (product + accel > maxSpeed)
            {
                accel = maxSpeed - product;
            }
            Vector3 newVelocity = velocity + moveDirection * accel;

            //Debug.Log(newVelocity.magnitude);

            newVelocity.y = rb.velocity.y;
            rb.velocity = newVelocity;
        }
        
        /*Vector3 velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float product = Vector3.Dot(moveDirection, velocity);
        float accel = acceleration * Time.fixedDeltaTime;
        if (product + accel > maxSpeed)
        {
            accel = maxSpeed - product;
        }
        Vector3 newVelocity = velocity + moveDirection * accel;

        //Debug.Log(newVelocity.magnitude);

        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<DeathFloor>())
        {
            DeathFloor deathfloor = other.gameObject.GetComponent<DeathFloor>();
            deathfloor.Respawn();
        }
        if (other.gameObject.tag == "Respawn")
        {
            Transform respawn = other.gameObject.transform;
            DeathFloor deathFloor = FindAnyObjectByType<DeathFloor>();
            deathFloor.Checkpoint(respawn);
        }
        if (other.gameObject.tag == "Projectile")
        {
            //stun = true;
            Debug.Log("Stun is true.");
            StartCoroutine(Stunned());
            //stun = false;
        }
    }

    //time the player will be stunned for when hit by police drone
    public IEnumerator Stunned()
    {
        stun = true;
        yield return new WaitForSeconds(3f);
        stun = false;
    }
}
