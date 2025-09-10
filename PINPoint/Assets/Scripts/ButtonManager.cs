using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Game scene needs to be connected");
        //SceneManager.LoadScene();
    }

    public void Controls()
    {
        Debug.Log("Controls panel needs to be connected");
        //SceneManager.LoadScene();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("You quit the game.");
    }

    public void ReturnToMenu()
    {
        Debug.Log("Menu needs to be connected");
        //SceneManager.LoadScene();
    }
}
