using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuEffects : MonoBehaviour
{
    //Variables
    public RawImage clickToPlay;
    public float fadeAnimTimer;
    public GameObject backgroundBlack;
    public GameObject buttonGroup;
    public GameObject backgroundCity;
    public GameObject invisibleButton;

    private bool hasClicked = false;

    private void Start()
    {
        if (clickToPlay == null)
        {
            Debug.LogAssertion("Error: RawImage Not Selected!");
            return;
        }

        StartCoroutine(FadeInAndOut());
    }

    public void ProceedToMainMenu()
    {
        invisibleButton.SetActive(false);
        backgroundCity.SetActive(false);

        backgroundBlack.SetActive(true);
        hasClicked = true;
        buttonGroup.SetActive(true);
    }

    private IEnumerator FadeInAndOut()
    {
        if (!hasClicked){

            yield return new WaitForSeconds(fadeAnimTimer);

            if (clickToPlay.enabled)
            {
                clickToPlay.enabled = false;
            }
            else if (!clickToPlay.enabled)
            {
                clickToPlay.enabled = true;
            }

            StartCoroutine(FadeInAndOut());
        }
        else
        {
            clickToPlay.enabled = false;
        }
    }
}
