using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ShopManager : MonoBehaviour
{
    #region Variables
    public GameObject jumpBootsSlot;
    public GameObject gliderSlot;
    public GameObject grappleSlot;
    #endregion



    public void BuyItem(int item)
    {
        if (jumpBootsSlot == null || gliderSlot == null || grappleSlot == null)
        {
            Debug.Log("ERROR: Equipment Menu is missing equipment slot(s)");
            return;
        }
        

        switch (item)
        {
            // Jump Boots
            case 0:

                if (ItemManager.Instance.jumpBootsCost <= GaMaControl.Instance.playerMoney)
                {
                    GaMaControl.Instance.playerMoney -= ItemManager.Instance.jumpBootsCost;
                    ItemManager.Instance.jumpBootsBought = true;
                }

                break;

            // Glider
            case 1:

                if (ItemManager.Instance.gliderCost <= GaMaControl.Instance.playerMoney)
                {
                    GaMaControl.Instance.playerMoney -= ItemManager.Instance.gliderCost;
                    ItemManager.Instance.gliderBought = true;
                }
                

                break;

            // Grapple
            case 2:

                if (ItemManager.Instance.grappleCost <= GaMaControl.Instance.playerMoney)
                {
                    GaMaControl.Instance.playerMoney -= ItemManager.Instance.grappleCost;
                    ItemManager.Instance.grappleBought = true;
                }
                

                break;
        }

        RefreshShop();
    }

    /// <summary>
    /// Check if the item can be bought
    /// </summary>
    private void RefreshShop()
    {
        if (ItemManager.Instance.jumpBootsCost > GaMaControl.Instance.playerMoney)
        {
            jumpBootsSlot.SetActive(false);
        }
        else
        {
            jumpBootsSlot.SetActive(true);
        }

        if (ItemManager.Instance.gliderCost > GaMaControl.Instance.playerMoney)
        {
            gliderSlot.SetActive(false);
        }
        else
        {
            gliderSlot.SetActive(true);
        }

        if (ItemManager.Instance.grappleCost > GaMaControl.Instance.playerMoney)
        {
            grappleSlot.SetActive(false);
        }
        else
        {
            grappleSlot.SetActive(true);
        }
    }
}
