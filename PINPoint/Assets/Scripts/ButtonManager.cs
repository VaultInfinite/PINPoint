using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject mainMenu, controlsMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Controls()
    {
        Debug.Log("Turn off Main Menu, Enable Controls");
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("You quit the game.");
    }

    public void ReturnToMenu()
    {
        controlsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
