using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GaMaControl : MonoBehaviour
{
    private static GaMaControl _instance;

    public static GaMaControl Instance { get { return _instance; } }

    public GameObject pause, lose, contracts, settings, load;

    private Scene restartScene;

    public float boTimer;

    private void Awake()
    {
        //Make sure this is the only Game Manager in the Scene
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        //Keep this object even when changing scenes
        DontDestroyOnLoad(gameObject);
    }

    #region Button Functions
    public void CallContractsUI()
    {
        contracts.SetActive(true);
    }

    //Pulls up the settings UI
    public void CallSettingsUI()
    {
        settings.SetActive(true);
    }

    /// <summary>
    /// Restarts the Level
    /// </summary>
    public void RetryLevel()
    {
        restartScene = SceneManager.GetActiveScene();

        BlackOut();

        SceneManager.LoadSceneAsync(restartScene.name);

        lose.SetActive(false);

        //Rough Fix of the Pause Menu
        //Prevents it from bugging out
        if (pause.activeSelf == true)
        {
            pause.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Pause.isPaused = false;
        }

    }

    /// <summary>
    /// Applies a blackout to transition to the next scene
    /// </summary>
    private void BlackOut()
    {
        load.SetActive(true);
        StartCoroutine(BlackIn());
    }


    IEnumerator BlackIn()
    {
        yield return new WaitForSeconds(boTimer);
        load.SetActive(false);
    }
    #endregion
}
