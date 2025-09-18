using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AirState : MonoBehaviour, IState
{
    private StateController stateController;

    public void OnMovement(PlayerBehaviour player)
    {
        StartCoroutine(Air(player));
    }

    IEnumerator Air(PlayerBehaviour player)
    {
        yield return null;
    }
}
