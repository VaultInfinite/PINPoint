using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GaMaControl : MonoBehaviour
{
    private static GaMaControl _instance;

    public static GaMaControl Instance { get { return _instance; } }


    public GameObject pause, lose, contracts, settings, load, playerUI;

    //Money that is CURRENTLY in the player's posession
    public int playerMoney;

    //Money that the player can win in the level
    public int levelMoney;
    int startMoney;

    public bool targetHit = false;
    public bool levelFailed;

    private Scene restartScene;

    public float boTimer;

    /// <summary>
    /// Make sure there is one one Game Manager Instance
    /// </summary>
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

    private void Start()
    {
        startMoney = levelMoney;
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

        levelMoney = startMoney;
        playerUI.gameObject.GetComponent<GameUIControl>().ResetTime();

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

        levelFailed = false;
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

    #region Menu Calls
    public void Fail()
    {
        levelFailed = true;

        lose.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    #endregion
}
