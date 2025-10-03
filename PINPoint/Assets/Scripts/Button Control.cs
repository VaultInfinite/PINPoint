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

        loseUI.SetActive(false);

        SceneManager.SetActiveScene(restartScene);
    }

}
