using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpState : MonoBehaviour, IState
{
    private StateController stateController;

    public void OnMovement(PlayerBehaviour player)
    {
        StartCoroutine(Jump(player));
    }

    IEnumerator Jump(PlayerBehaviour player)
    {
        yield return null;
    }
}
