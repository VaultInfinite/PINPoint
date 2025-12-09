using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script handles pausing the game
/// </summary>
public class Pause : MonoBehaviour
{
    private static Pause instance;

    public static Pause Instance { get { return instance; } }

    //Variables
    public static bool isPaused = false;    //Global Var - See if the game is paused
    private GameObject pauseMenu;           //Pause menu UI

    private void Awake()
    {
        //Make sure this is the only Game Manager in the Scene
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        pauseMenu = GameManager.Instance.pause;

        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (PlayerController.Instance != null)
        {
            if (PlayerController.Instance.input.Movement.Pause.WasPressedThisFrame()) HitPause();
        }
    }

    /// <summary>
    /// Reverse pause variable and change timescale depending on bool
    /// </summary>
    public void HitPause()
    {
        if (GameManager.Instance.levelFailed) return;

        isPaused = !isPaused;

        //Set the Pause Menu
        pauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// For settings only
    /// Turns the Pause menu back on
    /// </summary>
    public void BackToPause()
    {
        //Set the Pause Menu
        pauseMenu.SetActive(true);
    }
}
