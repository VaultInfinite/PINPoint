using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages various button effects
/// </summary>
public class ButtonControl : MonoBehaviour
{
    [SerializeField]
    private GameObject contractsUI;

    [SerializeField]
    private GameObject settingsUI;

    [SerializeField]
    private GameObject loseUI;

    [SerializeField]
    private GameObject pauseUI;

    private Scene restartScene;

    //Pulls up the COntracts UI
    public void CallContractsUI()
    {
        contractsUI.SetActive(true);
    }

    //Pulls up the settings UI
    public void CallSettingsUI()
    {
        settingsUI.SetActive(true);
    }

    //Restarts the Level
    public void RetryLevel()
    {
        restartScene = SceneManager.GetActiveScene();

        SceneManager.LoadSceneAsync(restartScene.name);

        loseUI.SetActive(false);

        //Rough Fix of the Pause Menu
        //Prevents it from bugging out
        if (pauseUI.activeSelf == true)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Pause.isPaused = false;
        }
        
    }

}
