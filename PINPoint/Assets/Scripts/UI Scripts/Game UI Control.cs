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
    private TextMeshProUGUI payDisplay, timeDisplay, weaponDisplay, chargeDisplay, targetPrice;

    //Timer Variables
    private float  elapsedTime;

    public string timer;
    #endregion

    private PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        targetPrice.text = "$" + LevelManager.Instance.levelMoney.ToString("00,000,000");
    }


    private void Update()
    {
        //If the target has been hit, stop time
        if (GameManager.Instance.targetHit || GameManager.Instance.levelFailed) return;

        //Timer
        
        elapsedTime += Time.deltaTime;

        int min = Mathf.FloorToInt(elapsedTime/60);
        int sec = Mathf.FloorToInt(elapsedTime%60);
        int mSec = Mathf.FloorToInt((elapsedTime%1f) * 60);

        //Display UI
        timer = min.ToString("00") + ":" + sec.ToString("00") + ":" + mSec.ToString("00");

        timeDisplay.text = timer;
        payDisplay.text = "$" + LevelManager.Instance.levelMoney.ToString("00,000,000");


        //TEMP WEAPON DISPLAY IMPLEMENTATION
        if (player != null)
        {
            if (player.gameObject.GetComponent<Shoot>().isActiveAndEnabled)
            {
                weaponDisplay.text = "Sniper";
                chargeDisplay.text = " ";
            }
            if (player.gameObject.GetComponent<Grappling>().isActiveAndEnabled)
            {
                weaponDisplay.text = "Grappling Hook";
                chargeDisplay.text = "Charges: " + player.grapple.chargeCount;
            }
            if (player.ledge.CanLedgeGrab(player))
            {
                weaponDisplay.text = "Ledge";
                chargeDisplay.text = " ";
            }
        }
    }

    /// <summary>
    /// Reset time for new playthrough
    /// </summary>
    public void ResetTime()
    {
        elapsedTime = 0;

        //This is gross but it's gotta go here
        player = FindObjectOfType<PlayerController>();
    }
}
