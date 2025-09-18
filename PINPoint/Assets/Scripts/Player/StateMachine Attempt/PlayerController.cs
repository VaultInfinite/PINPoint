using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController : MonoBehaviour
{
    //Dictionary containing all the states the player can be in
    private readonly Dictionary<Type, State> _states = new()
    {
        {typeof(Walking), new Walking() },
        {typeof(Running), new Running() },
    };
    
    //The Input system
    public PlayerControllerInput input;

    //The key of the current state, default to walking
    private Type _state = typeof(Walking);

    private void Awake()
    {
        input = new();
        input.Enable();
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
    }
}
