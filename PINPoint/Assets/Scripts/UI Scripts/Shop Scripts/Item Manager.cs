using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This script allows 
/// </summary>
public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;
    public static ItemManager Instance { get { return instance; } }

    [Header("Items Bought")]
    public bool jumpBootsBought;
    public bool gliderBought;
    public bool grappleBought;

    [Header("Cost")]
    public int jumpBootsCost;
    public int gliderCost;
    public int grappleCost;

    [Header("UI")]
    public TextMeshProUGUI jumpCostUI;
    public TextMeshProUGUI glideCostUI;
    public TextMeshProUGUI grappleCostUI;

    private void Start()
    {
        if (jumpCostUI == null || glideCostUI == null || grappleCostUI == null)
        {
            Debug.Log("ERROR: Equipment Menu is missing equipment cost text(s)");
        }

        jumpCostUI.text = "$" + jumpBootsCost.ToString("0,000");
        glideCostUI.text = "$" + gliderCost.ToString("0,000");
        grappleCostUI.text = "$" + grappleCost.ToString("0,000");
    }

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
