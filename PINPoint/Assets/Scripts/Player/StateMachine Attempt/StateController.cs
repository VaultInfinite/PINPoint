using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateController : MonoBehaviour
{
    private List<IState> states = new List<IState>();

    private PlayerBehaviour playerController;

    private void Start()
    {
        Object player = FindFirstObjectByType(typeof(PlayerBehaviour));
        playerController = player.GetComponent<PlayerBehaviour>();

        //moveStateContext = new MoveStateContext(this);

        states.Add(player.AddComponent<MoveState>());
        states.Add(player.AddComponent<JumpState>());
        states.Add(player.AddComponent<CrouchState>());
        states.Add(player.AddComponent<AirState>());
        states.Add(player.AddComponent<LedgeState>());
    }

    //private void Update()
    //{
    //    currentState.UpdateState();
    //}

    //public void ChangeState(IState newState)
    //{
        
    //}
}

public interface IState
{
    void OnMovement(PlayerBehaviour player);
}