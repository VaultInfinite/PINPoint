using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuEffects : MonoBehaviour
{
    //Variables
    public RawImage clickToPlay;

    private void Update()
    {
        
    }

    /// <summary>
    /// Makes the UI Fade in and out
    /// </summary>
    private void FadeInAndOut()
    {
        if (clickToPlay == null)
        {
            Debug.LogAssertion("Error: RawImage Not Selected!");
            return;
        }

        
    }

    /*
    private IEnumerator FadeIn()
    {
        return;
    }*/
}
