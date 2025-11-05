using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    private static ShopManager instance;
    public static ShopManager Instance { get { return instance; } }

    #region Variables
    public Button jumpBootsSlot;
    public Button gliderSlot;
    public Button grappleSlot;
    #endregion

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
    private void Start()
    {
        RefreshShop();
    }

    /// <summary>
    /// Check if the item can be bought
    /// </summary>
    public void RefreshShop()
    {
        if (!CheckButton(jumpBootsSlot) || !CheckButton(gliderSlot) || !CheckButton(grappleSlot)) { return; }

        if (ItemManager.Instance.jumpBootsCost > GameManager.Instance.playerMoney)
        {
            jumpBootsSlot.interactable = false;
        }
        else
        {
            jumpBootsSlot.interactable = true;
        }

        if (ItemManager.Instance.gliderCost > GameManager.Instance.playerMoney)
        {
            gliderSlot.interactable = false;
        }
        else
        {
            gliderSlot.interactable = true;
        }

        if (ItemManager.Instance.grappleCost > GameManager.Instance.playerMoney)
        {
            grappleSlot.interactable = false;
        }
        else
        {
            grappleSlot.interactable = true;
        }
    }

    /// <summary>
    /// Buys item from shop
    /// </summary>
    /// <param name="item"></param>
    public void BuyItem(int item)
    {
        if (!CheckButton(jumpBootsSlot) || !CheckButton(gliderSlot) || !CheckButton(grappleSlot)) { return; }


        switch (item)
        {
            // Jump Boots
            case 0:

                if (ItemManager.Instance.jumpBootsCost <= GameManager.Instance.playerMoney)
                {
                    GameManager.Instance.playerMoney -= ItemManager.Instance.jumpBootsCost;
                    ItemManager.Instance.jumpBootsBought = true;
                }

                break;

            // Glider
            case 1:

                if (ItemManager.Instance.gliderCost <= GameManager.Instance.playerMoney)
                {
                    GameManager.Instance.playerMoney -= ItemManager.Instance.gliderCost;
                    ItemManager.Instance.gliderBought = true;
                }


                break;

            // Grapple
            case 2:

                if (ItemManager.Instance.grappleCost <= GameManager.Instance.playerMoney)
                {
                    GameManager.Instance.playerMoney -= ItemManager.Instance.grappleCost;
                    ItemManager.Instance.grappleBought = true;
                }


                break;
        }

        RefreshShop();
    }

    #region Debug Methods

    /// <summary>
    /// Check if button is assigned to anything
    /// </summary>
    /// <param name="button"></param>
    /// <returns>Bool</returns>
    private bool CheckButton(Button button)
    {
        if (button == null)
        {
            Debug.Log("ERROR: Button is not assigned!");
            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion
}
