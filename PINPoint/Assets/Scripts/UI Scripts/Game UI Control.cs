using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameUIControl : MonoBehaviour
{
    /// <summary>
    /// Handles the Timer function of the Player's UI
    /// </summary>
    #region Timer Variables
    [SerializeField]
    private TextMeshProUGUI payDisplay, timeDisplay, weaponDisplay;

    //Timer Variables
    private float min, sec, mSec, elapsedTime;

    //For the money
    float actionTime;

    public string timer;

    //Money Variables

    //Note - The Money Variable is in the Game Control.

    [SerializeField]
    private int interest;
    #endregion

  
    private void Update()
    {
        //If the target has been hit, stop time
        if (GaMaControl.Instance.targetHit || GaMaControl.Instance.levelFailed) return;

        //Timer
        elapsedTime += Time.deltaTime;
        min = Mathf.FloorToInt(elapsedTime/60);
        sec = Mathf.FloorToInt(elapsedTime%60);
        mSec = Mathf.FloorToInt((elapsedTime%1f) * 60);

        //Decrease Money
        MoneyInterval();

        //Display UI
        timer = min.ToString("00") + ":" + sec.ToString("00") + ":" + mSec.ToString("00");

        timeDisplay.text = timer;
        payDisplay.text = "$" + GaMaControl.Instance.levelMoney.ToString("0,000,000");
    }

    void MoneyInterval()
    {
        //Check if there is enough money
        if (GaMaControl.Instance.levelMoney <= 0)
        {
            GaMaControl.Instance.Fail();
        }
        //Decrease money
        else
        {
            if (Time.deltaTime > actionTime)
            {
                GaMaControl.Instance.levelMoney -= interest;
            }
        }
    }

    /// <summary>
    /// Reset time for new playthrough
    /// </summary>
    public void ResetTime()
    {
        min = 0;
        sec = 0;
        mSec = 0;
        elapsedTime = 0;
    }
}
