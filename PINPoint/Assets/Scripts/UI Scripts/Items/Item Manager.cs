using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This script dictates items in both the scene & shop
/// </summary>
public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;
    public static ItemManager Instance { get { return instance; } }

    public Item glider, grapple, rocketBoots, shockGun;

    public Item[] Items => new Item[] {glider, grapple, rocketBoots};

    //private void Start()
    //{
    //    if (jumpCostUI == null || glideCostUI == null || grappleCostUI == null)
    //    {
    //        Debug.Log("ERROR: Equipment Menu is missing equipment cost text(s)");
    //    }

    //    jumpCostUI.text = "$" + jumpBootsCost.ToString("0,000");
    //    glideCostUI.text = "$" + gliderCost.ToString("0,000");
    //    grappleCostUI.text = "$" + grappleCost.ToString("0,000");
    //}

    private void Awake()
    {
        //Make sure this is the only Game Manager in the Scene
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
