using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    private IState
        moveState, airState, 

    public MoveState moveState = new MoveState();
    public AirState airState = new AirState();
    public CrouchState crouchState = new CrouchState();
    public JumpState jumpState = new JumpState();
    public LedgeState ledgeState = new LedgeState();

    private MoveStateContext moveStateContext;

    private void Start()
    {
        //moveStateContext = new MoveStateContext(this);

        //moveState = gameObject.AddComponent<MoveState>();
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void ChangeState(IState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}

public interface IState
{
    public void OnEnter();

    public void UpdateState();

    public void OnExit();
}