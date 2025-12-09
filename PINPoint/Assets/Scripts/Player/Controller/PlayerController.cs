using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public partial class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    public float groundDrag;

    [SerializeField]
    private float coyoteTime;
    private float lastGroundedTime;
    private bool coyoteJumped;

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
    private bool grounded;

    [Header("Objects")]
    public GameObject sniperOBJ, grappleOBJ, shockOBJ;
    public GameObject ledgeHands;
    public GameObject shockOBJLight;

    //Charge Regen for the Grappling Hook; time passed
    private float chargeRegenTime;


    #endregion

    private static PlayerController instance;

    public static PlayerController Instance { get { return instance; } }

    //Dictionary containing all the states the player can be in // STATES MUST BE CALLED AS THEY ARE BELOW, AS WELL AS ADDED IN AWAKE TO BE CALLED
    public Walking walking;
    public Running running;
    public Jump jump;
    public Ledge ledge;
    public Crouch crouch;
    public Air air;
    public WallRunning wall;
    public Gliding gliding;
    private readonly Dictionary<Type, State> _states = new();

    [HideInInspector]
    public Shoot shooting;
    [HideInInspector]
    public Grappling grapple;
    
    //The Input system
    public PlayerControllerInput input;


    //The key of the current state, default to walking
    [HideInInspector]
    public Type _state = typeof(Walking);

    //Player Ability
    private bool CanDoubleJump => ItemManager.Instance.rocketBoots.enabled;
    private bool CanGlide => ItemManager.Instance.glider.enabled;
    private bool CanGrapple => ItemManager.Instance.grapple.enabled;
    private bool CanShock => ItemManager.Instance.shockGun.enabled;

    //Allow stun control script to handle logic
    [HideInInspector]
    public StunControl stunControl;

    private void Awake()
    {
        input = new();
        input.Enable();
        instance = this;

        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();

        _states.Add(typeof(Walking), walking);
        _states.Add(typeof(Running), running);
        _states.Add(typeof(Jump), jump);
        _states.Add(typeof(Crouch), crouch);
        _states.Add(typeof(Air), air);
        _states.Add(typeof(WallRunning), wall);
        _states.Add(typeof(Gliding), gliding);
        _states.Add(typeof(Ledge), ledge);

        shooting = gameObject.GetComponent<Shoot>();
        grapple = gameObject.GetComponent<Grappling>();
        stunControl = gameObject.GetComponent<StunControl>();
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
        if (!stunControl.isStunned)
        {
            state.OnFixedUpdate(this);
        }
        //Debug.Log(_state);

        //Check if the player is touching the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.01f, Ground);

        if (grounded && _state != typeof(Air))
        {
            lastGroundedTime = Time.time;
            coyoteJumped = false;
        }
    }

    private void Update()
    {
        var state = _states[_state];

        if (input.Movement.SelectSniper.IsPressed() && state != ledge)
        {
            GrappleSetActive(false);
            ShockSetActive(false);
            RifleSetActive(true);
        }
        if (input.Movement.SelectGrapple.IsPressed() && state != ledge && CanGrapple)
        {
            RifleSetActive(false);
            ShockSetActive(false);
            GrappleSetActive(true);
        }
        if (input.Movement.SelectShock.IsPressed() && state != ledge && CanShock)
        {
            RifleSetActive(false);
            GrappleSetActive(false);
            ShockSetActive(true);
        }
        
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
        if (!stunControl.isStunned)
        {
            state.OnUpdate(this);
        }

        //Limitation on Grappling Hook Charges
        if (grapple.chargeCount < grapple.chargeLimit)
        {
            chargeRegenTime += Time.deltaTime;

            if (chargeRegenTime >= grapple.chargeRegenSpeed)
            {
                grapple.chargeCount++;
                chargeRegenTime -= grapple.chargeRegenSpeed;
            }
        }
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
    }

    private void OnEnable()
    {
        input.Enable();
        input.Movement.Enable();
    }

    private void OnDisable()
    {
        input.Movement.Disable();
        input.Disable();
    }

    private bool TryJump()
    {
        if (Time.time - lastGroundedTime <= coyoteTime)
        {
            return true;
        }

        return false;
    }

    private void RifleSetActive(bool toggle)
    {
        if (sniperOBJ)
        {
            if (toggle)
            {
                shooting.enabled = true;
                shooting.playerGun = GunType.rifle;
                sniperOBJ.SetActive(true);
            }
            else
            {
                shooting.enabled = false;
                sniperOBJ.SetActive(false);
            }
        }
    }

    private void GrappleSetActive(bool toggle)
    {
        if (grappleOBJ != null)
        {
            if (toggle)
            {
                grapple.enabled = true;
                grappleOBJ.SetActive(true);
            }
            else
            {
                grapple.enabled = false;
                grappleOBJ.SetActive(false);
                grapple.point.SetActive(false);
                grapple.lineRenderer.enabled = false;
            }
        }
        
    }

    private void ShockSetActive(bool toggle)
    {
        if (shockOBJ != null)
        {
            if (toggle)
            {
                shooting.enabled = true;
                shooting.playerGun = GunType.stun;
                shockOBJ.SetActive(true);
            }
            else
            {
                shooting.enabled = false;
                shockOBJ.SetActive(false);
            }
        }
        
    }

    /// <summary>
    /// Implemented to stop the player from grabbing ledge again too quickly.
    /// </summary>
    /// <returns></returns>
    public IEnumerator LedgePause()
    {
        yield return new WaitForSeconds(0.5f);
        air.ledgeGrabbed = false;
    }
}
