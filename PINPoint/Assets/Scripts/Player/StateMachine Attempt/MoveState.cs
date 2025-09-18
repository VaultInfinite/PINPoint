using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveState : MonoBehaviour, IState
{
    private StateController stateController;

    public void OnMovement(PlayerBehaviour player)
    {
        StartCoroutine(Move(player));
    }

    IEnumerator Move(PlayerBehaviour player)
    {
        yield return null;
    }
}
