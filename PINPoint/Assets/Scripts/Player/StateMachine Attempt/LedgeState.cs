using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LedgeState : MonoBehaviour, IState
{
    private StateController stateController;

    public void OnMovement(PlayerBehaviour player)
    {
        StartCoroutine(Movement(player));
    }

    IEnumerator Movement(PlayerBehaviour player)
    {
        yield return null;
    }
}
