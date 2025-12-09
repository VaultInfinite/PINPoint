using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public class GameUIControl : MonoBehaviour
{
    /// <summary>
    /// Handles the Timer function of the Player's UI
    /// </summary>
    #region Timer Variables
    [SerializeField]
    private TextMeshProUGUI payDisplay, timeDisplay, weaponDisplay, chargeDisplay, targetPrice;

    //Timer Variables
    private float elapsedTime;

    public string timer;
    #endregion

    private PlayerController player;

    private void Start()
    {
        SceneManager.activeSceneChanged += FindPlayer;
    }


    private void Update()
    {
        //If the target has been hit, stop time
        if (GameManager.Instance.targetHit || GameManager.Instance.levelFailed || player == null) return;

        
        if (LevelManager.Instance != null)
        {
            //Timer

            elapsedTime += Time.deltaTime;

            int min = Mathf.FloorToInt(elapsedTime / 60);
            int sec = Mathf.FloorToInt(elapsedTime % 60);
            int mSec = Mathf.FloorToInt((elapsedTime % 1f) * 60);

            //Display UI
            timer = min.ToString("00") + ":" + sec.ToString("00") + ":" + mSec.ToString("00");

            timeDisplay.text = timer;

            payDisplay.text = "$" + LevelManager.Instance.levelMoney.ToString("###,###,###");
        }


        //TEMP WEAPON DISPLAY IMPLEMENTATION
        if (player != null)
        {
            var weapon = player.gameObject.GetComponent<Shoot>();
            var grapple = player.gameObject.GetComponent<Grappling>();

            if (player.stunControl.isStunned)
            {
                weaponDisplay.text = "STUNNED";
            }
            if (weapon.isActiveAndEnabled && weapon.playerGun == GunType.rifle)
            {
                weaponDisplay.text = "Sniper";
                chargeDisplay.text = " ";
            }
            if (weapon.isActiveAndEnabled && weapon.playerGun == GunType.stun)
            {
                weaponDisplay.text = "Stun Gun";
                chargeDisplay.text = " ";
            }
            if (grapple.isActiveAndEnabled)
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
    }

    public void SetTargetPrice()
    {
        int priceOfTarget = 0;

        switch (GameManager.Instance.lastDifficulty)
        {
            case LevelManager.Difficulty.Easy:
                priceOfTarget = 10000;
                break;
            case LevelManager.Difficulty.Medium:
                priceOfTarget = 25000;
                break;
            case LevelManager.Difficulty.Hard:
                priceOfTarget = 100000;
                break;
        }

        targetPrice.text = "$" + priceOfTarget.ToString("###,###,###");
    }

    private void FindPlayer(Scene current, Scene next)
    {
        player = FindObjectOfType<PlayerController>();
    }

    
}
