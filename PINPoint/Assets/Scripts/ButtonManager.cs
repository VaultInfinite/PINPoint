using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject mainMenu, controlsMenu, settingsMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("You quit the game.");
    }

    public void Controls()
    {
        Debug.Log("Turn off Main Menu, Enable Controls");
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    

    public void ReturnToMenu()
    {
        controlsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        controlsMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
}
