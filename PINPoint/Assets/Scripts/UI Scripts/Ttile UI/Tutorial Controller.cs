using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Joseph Acuna - 12/4/25
/// 
/// This script controls the button effects 
/// and which Tutorial UI is activated at any time
/// </summary>
public class TutorialController : MonoBehaviour
{
    #region Buttons
    [Header("Buttons")]
    public GameObject hideables;
    public GameObject buttonGroup;
    public GameObject quitThisTutorialButton;
    public GameObject exitTutorialMenuButton;
    #endregion

    #region UI Groups

    // Basic Tutorial Section
    [Header("Basic Tutorials")]
    public GameObject goalTutorialGroup;
    public GameObject controlTutorialGroup;
    public GameObject hideoutTutorialGroup;

    // Advanced Tutorial Section
    [Header("Advanced Tutorials")]
    public GameObject wallRunTutorialGroup;
    public GameObject ledgeGrabTutorialGroup;
    public GameObject shockGunTutorialGroup;
    public GameObject glidingTutorialGroup;
    public GameObject grappleGunTutorialGroup;
    #endregion

    #region Functions

    /// <summary>
    /// Shows and Hides Hideable UI
    /// </summary>
    public void ToggleHidables()
    {
        if (hideables == null)
        {
            Debug.LogErrorFormat("ERROR: Hideable not set");
            return;
        }

        if (hideables.activeSelf)
        {
            hideables.SetActive(false);
        }
        else
        {
            hideables.SetActive(true);
        }
    }

    /// <summary>
    /// Shows and Hides button groups UI
    /// </summary>
    public void ToggleButtonGroups()
    {
        if (buttonGroup == null)
        {
            Debug.LogErrorFormat("ERROR: buttonGroup not set");
            return;
        }

        if (buttonGroup.activeSelf)
        {
            buttonGroup.SetActive(false);
        }
        else
        {
            buttonGroup.SetActive(true);
        }
    }

    /// <summary>
    /// Turns off the present UI and returns back to the Tutorial Menu
    /// </summary>
    public void ToTutorialMenu()
    {
        goalTutorialGroup.SetActive(false);
        controlTutorialGroup.SetActive(false);
        hideoutTutorialGroup.SetActive(false);
        wallRunTutorialGroup.SetActive(false);
        ledgeGrabTutorialGroup.SetActive(false);
        shockGunTutorialGroup.SetActive(false);
        glidingTutorialGroup.SetActive(false);
        grappleGunTutorialGroup.SetActive(false);

        hideables.SetActive(true);
        buttonGroup.SetActive(true);
        quitThisTutorialButton.SetActive(false);
        exitTutorialMenuButton.SetActive(true);

    }

    /// <summary>
    /// Changes what active UI Elements are active
    /// </summary>
    private void ChangeTutorialUI()
    {
        quitThisTutorialButton.SetActive(true);
        exitTutorialMenuButton.SetActive(false);

        hideables.SetActive(false);
        buttonGroup.SetActive(false);
    }

    #region Basic Tutorial

    /// <summary>
    /// Shows and Hides the goal tutorial
    /// </summary>
    public void ToggleGoalTutorial()
    {
        if (goalTutorialGroup == null)
        {
            Debug.LogErrorFormat("ERROR: goalTutorialGroup not set");
            return;
        }

        ChangeTutorialUI();

        if (goalTutorialGroup.activeSelf)
        {
            goalTutorialGroup.SetActive(false);
        }
        else
        {
            goalTutorialGroup.SetActive(true);
        }
    }

    /// <summary>
    /// Shows and Hides the controls tutorial
    /// </summary>
    public void ToggleControlTutorial()
    {
        if (controlTutorialGroup == null)
        {
            Debug.LogErrorFormat("ERROR: controlTutorialGroup not set");
            return;
        }

        ChangeTutorialUI();

        if (controlTutorialGroup.activeSelf)
        {
            controlTutorialGroup.SetActive(false);
        }
        else
        {
            controlTutorialGroup.SetActive(true);
        }
    }

    /// <summary>
    /// Shows and Hides the controls tutorial
    /// </summary>
    public void ToggleHideoutTutorial()
    {
        if (hideoutTutorialGroup == null)
        {
            Debug.LogErrorFormat("ERROR: hideoutTutorialGroup not set");
            return;
        }

        ChangeTutorialUI();

        if (hideoutTutorialGroup.activeSelf)
        {
            hideoutTutorialGroup.SetActive(false);
        }
        else
        {
            hideoutTutorialGroup.SetActive(true);
        }
    }

    #endregion

    #region Advanced Tutorial

    /// <summary>
    /// Shows and Hides the Wall Run tutorial
    /// </summary>
    public void ToggleWallRunTutorial()
    {
        if (wallRunTutorialGroup == null)
        {
            Debug.LogErrorFormat("ERROR: wallRunTutorialGroup not set");
            return;
        }

        ChangeTutorialUI();

        if (wallRunTutorialGroup.activeSelf)
        {
            wallRunTutorialGroup.SetActive(false);
        }
        else
        {
            wallRunTutorialGroup.SetActive(true);
        }
    }

    /// <summary>
    /// Shows and Hides the ledge grab tutorial
    /// </summary>
    public void ToggleLedgeGrabTutorial()
    {
        if (ledgeGrabTutorialGroup == null)
        {
            Debug.LogErrorFormat("ERROR: ledgeGrabTutorialGroup not set");
            return;
        }

        ChangeTutorialUI();

        if (ledgeGrabTutorialGroup.activeSelf)
        {
            ledgeGrabTutorialGroup.SetActive(false);
        }
        else
        {
            ledgeGrabTutorialGroup.SetActive(true);
        }
    }

    /// <summary>
    /// Shows and Hides the shock gun tutorial
    /// </summary>
    public void ToggleShockGunTutorial()
    {
        if (shockGunTutorialGroup == null)
        {
            Debug.LogErrorFormat("ERROR: shockGunTutorialGroup not set");
            return;
        }

        ChangeTutorialUI();

        if (shockGunTutorialGroup.activeSelf)
        {
            shockGunTutorialGroup.SetActive(false);
        }
        else
        {
            shockGunTutorialGroup.SetActive(true);
        }
    }

    /// <summary>
    /// Shows and Hides the glide tutorial
    /// </summary>
    public void ToggleGlideTutorial()
    {
        if (glidingTutorialGroup == null)
        {
            Debug.LogErrorFormat("ERROR: glidingTutorialGroup not set");
            return;
        }

        ChangeTutorialUI();

        if (glidingTutorialGroup.activeSelf)
        {
            glidingTutorialGroup.SetActive(false);
        }
        else
        {
            glidingTutorialGroup.SetActive(true);
        }
    }

    /// <summary>
    /// Shows and Hides the grapple tutorial
    /// </summary>
    public void ToggleGrappleTutorial()
    {
        if (grappleGunTutorialGroup == null)
        {
            Debug.LogErrorFormat("ERROR: grappleGunTutorialGroup not set");
            return;
        }

        ChangeTutorialUI();

        if (grappleGunTutorialGroup.activeSelf)
        {
            grappleGunTutorialGroup.SetActive(false);
        }
        else
        {
            grappleGunTutorialGroup.SetActive(true);
        }
    }

    #endregion

    #endregion
}
