using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class GameUIControl : MonoBehaviour
{
    /// <summary>
    /// Handles the Timer function of the Player's UI
    /// </summary>
    #region Timer Variables
    [SerializeField]
    private TextMeshProUGUI payDisplay, timeDisplay, weaponDisplay, targetPrice;

    //Timer Variables
    private float min, sec, mSec, elapsedTime;

    public string timer;
    #endregion

    private PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        targetPrice.text = "$" + GaMaControl.Instance.levelMoney.ToString("00,000,000");
    }


    private void Update()
    {
        //If the target has been hit, stop time
        if (GaMaControl.Instance.targetHit || GaMaControl.Instance.levelFailed) return;

        //Timer
        
        elapsedTime += Time.deltaTime;
        min = Mathf.FloorToInt(elapsedTime/60);
        sec = Mathf.FloorToInt(elapsedTime%60);
        mSec = Mathf.FloorToInt((elapsedTime%1f) * 60);

        //Display UI
        timer = min.ToString("00") + ":" + sec.ToString("00") + ":" + mSec.ToString("00");

        timeDisplay.text = timer;
        payDisplay.text = "$" + GaMaControl.Instance.levelMoney.ToString("00,000,000");


        //TEMP WEAPON DISPLAY IMPLEMENTATION
        if (player != null)
        {
            if (player.gameObject.GetComponent<Shoot>().isActiveAndEnabled)
            {
                weaponDisplay.text = "Sniper";
            }
            if (player.gameObject.GetComponent<Grappling>().isActiveAndEnabled)
            {
                weaponDisplay.text = "Grappling Hook";
            }
            if (player.ledge.CanLedgeGrab(player))
            {
                weaponDisplay.text = "Ledge";
            }
        }
    }

    private void FixedUpdate()
    {
        MoneyInterval();
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

    /// <summary>
    /// Called in FixedUpdate; removes a specific amount of money per second (interest is amount lost per second)
    /// </summary>
    private void MoneyInterval()
    {
        //Check if there is enough money
        if (GaMaControl.Instance.levelMoney <= 0)
        {
            GaMaControl.Instance.Fail();
        }
        //Decrease money
        else
        {
            float interest = GaMaControl.Instance.startMoney / GaMaControl.Instance.levelDuration;
            GaMaControl.Instance.levelMoney -= interest * Time.fixedDeltaTime;
        }
    }
}
