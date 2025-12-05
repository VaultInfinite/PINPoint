using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject mainMenu, tutorialMenu, settingsMenu, title;

    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("You quit the game.");
    }

    public void Tutorial()
    {
        Debug.Log("Turn off Main Menu, Enable Controls");
        title.SetActive(false);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        tutorialMenu.SetActive(true);
    }

    public void ReturnToMenu()
    {
        title.SetActive(true);
        tutorialMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
}
