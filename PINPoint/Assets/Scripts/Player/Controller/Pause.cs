using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script handles pausing the game
/// </summary>
public class Pause : MonoBehaviour
{
    //Variables
    public static bool isPaused = false;
    public PlayerControllerInput input;

    private void Awake()
    {
        input = new();
        input.Enable();
    }

    private void Update()
    {
        if (input.Movement.Pause.WasPressedThisFrame()) HitPause();
    }

    /// <summary>
    /// Reverse pause variable and change timescale depending on bool
    /// </summary>
    public void HitPause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
