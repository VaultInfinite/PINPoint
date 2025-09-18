using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrouchState : MonoBehaviour, IState
{
    private StateController stateController;

    public void OnMovement(PlayerBehaviour player)
    {
        StartCoroutine(Crouch(player));
    }

    IEnumerator Crouch(PlayerBehaviour player)
    {
        yield return null;
    }
}
