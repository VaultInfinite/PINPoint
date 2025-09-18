using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the camera to move with the player
/// </summary>
public class MoveCamera : MonoBehaviour
{
    public Transform cameraPos;

    private void Update()
    {
        transform.position = cameraPos.position;
    }
}
