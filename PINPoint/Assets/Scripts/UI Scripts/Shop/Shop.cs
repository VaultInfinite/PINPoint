using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{

    #region Variables

    private Item selected;

    public GameObject shopButton, equipButton;
    public Transform shopButtonParent, equipButtonParent;

    public Button purchaseButton;
    
    //public Button jumpBootsSlot;
    //public Button gliderSlot;
    //public Button grappleSlot;

    //public string jumpBootsDesc = "This item allows you to jump in the air";
    //public string gliderDesc = "This item allows you to glide in the air";
    //public string grappleDesc = "This item allows you to grapple to nearby walls";

    //public GameObject jumpBootsToggle;
    //public GameObject glidersToggle;
    //public GameObject grappleToggle;

    //public PlayerController player;

    public TextMeshProUGUI itemDescUI;

    //public int currItem = 3;
    #endregion

    private void Start()
    {
        foreach (Item item in ItemManager.Instance.Items)
        {
            GameObject shopButtonInstance = Instantiate(shopButton, shopButtonParent);
            ShopButton c1 = shopButtonInstance.GetComponent<ShopButton>();
            c1.Item = item;
            c1.onSelected.AddListener(OnItemSelected);

            GameObject equipButtonInstance = Instantiate(equipButton, equipButtonParent);
            EquipButton c2 = equipButtonInstance.GetComponent<EquipButton>();
            c2.Item = item;
        }
    }

    private void OnItemSelected(Item item)
    {
        selected = item;
        itemDescUI.text = item.description;
        purchaseButton.interactable = !item.bought && GameManager.Instance.playerMoney >= item.cost;
    }

    public void OnPurchase()
    {
        GameManager.Instance.playerMoney -= selected.cost;
        selected.bought = true;
        purchaseButton.interactable = false;
    }

    /// <summary>
    /// Check if the item can be bought
    /// </summary>
    //public void RefreshShop()
    //{
    //    if (!CheckButton(jumpBootsSlot) || !CheckButton(gliderSlot) || !CheckButton(grappleSlot)) { return; }

    //    if (ItemManager.Instance.jumpBootsCost > GameManager.Instance.playerMoney)
    //    {
    //        jumpBootsSlot.interactable = false;
    //    }
    //    else
    //    {
    //        jumpBootsSlot.interactable = true;
    //    }

    //    if (ItemManager.Instance.gliderCost > GameManager.Instance.playerMoney)
    //    {
    //        gliderSlot.interactable = false;
    //    }
    //    else
    //    {
    //        gliderSlot.interactable = true;
    //    }

    //    if (ItemManager.Instance.grappleCost > GameManager.Instance.playerMoney)
    //    {
    //        grappleSlot.interactable = false;
    //    }
    //    else
    //    {
    //        grappleSlot.interactable = true;
    //    }

    //    if (jumpBootsToggle == null || glidersToggle == null || grappleToggle == null)
    //    {
    //        Debug.LogError("ERROR: Toggle Not assigned");
    //        return;
    //    }

    //    if (ItemManager.Instance.jumpBootsBought == true)
    //    {
    //        jumpBootsToggle.SetActive(true);
    //    }
    //    else
    //    {
    //        jumpBootsToggle.SetActive(false);
    //    }

    //    if (ItemManager.Instance.gliderBought == true)
    //    {
    //        glidersToggle.SetActive(true);
    //    }
    //    else
    //    {
    //        glidersToggle.SetActive(false);
    //    }

    //    if (ItemManager.Instance.grappleBought == true)
    //    {
    //        grappleToggle.SetActive(true);
    //    }
    //    else
    //    {
    //        grappleToggle.SetActive(false);
    //    }
    //}

    /// <summary>
    /// Buys item from shop
    /// </summary>
    /// <param name="item"></param>
    //public void BuyItem()
    //{
    //    if (jumpBootsToggle == null || glidersToggle == null || grappleToggle == null)
    //    {
    //        Debug.LogError("ERROR: Toggle Not assigned");
    //        return;
    //    }


    //    switch (currItem)
    //    {
    //        // Jump Boots
    //        case 0:

    //            if (ItemManager.Instance.jumpBootsCost <= GameManager.Instance.playerMoney)
    //            {
    //                GameManager.Instance.playerMoney -= ItemManager.Instance.jumpBootsCost;
    //                ItemManager.Instance.jumpBootsBought = true;
    //                player.CanDoubleJump = true;
    //                jumpBootsToggle.SetActive(true);
    //            }

    //            break;

    //        // Glider
    //        case 1:

    //            if (ItemManager.Instance.gliderCost <= GameManager.Instance.playerMoney)
    //            {
    //                GameManager.Instance.playerMoney -= ItemManager.Instance.gliderCost;
    //                ItemManager.Instance.gliderBought = true;
    //                player.CanGlide = true;
    //                glidersToggle.SetActive(true);
    //            }


    //            break;

    //        // Grapple
    //        case 2:

    //            if (ItemManager.Instance.grappleCost <= GameManager.Instance.playerMoney)
    //            {
    //                GameManager.Instance.playerMoney -= ItemManager.Instance.grappleCost;
    //                ItemManager.Instance.grappleBought = true;
    //                player.CanGrapple = true;
    //                grappleToggle.SetActive(true);
    //            }


    //            break;
    //    }

    //    RefreshShop();
    //}

    /// <summary>
    /// Makes the shop-equipment button display description if bought
    /// </summary>
    /// <param name="item"></param>
    //public void ClickItem(int item)
    //{
    //    if (!CheckButton(jumpBootsSlot) || !CheckButton(gliderSlot) || !CheckButton(grappleSlot)) { return; }

    //    switch (item)
    //    {
    //        // Jump Boots
    //        case 0:

    //            currItem = 0;
    //            itemDescUI.text = jumpBootsDesc;


    //            break;

    //        // Glider
    //        case 1:

    //            currItem = 1;
    //            itemDescUI.text = gliderDesc;

    //            break;

    //        // Grapple
    //        case 2:

    //            currItem = 2;
    //            itemDescUI.text = grappleDesc;

    //            break;

    //        default:

    //            currItem = 3;
    //            itemDescUI.text = "ERROR";

    //            break;
    //    }

    //    RefreshShop();
    //}

    //public void ToggleAbility(int item)
    //{
    //    switch (item)
    //    {
    //        // Jump Boots
    //        case 0:
    //            if (player.CanDoubleJump)
    //            {
    //                player.CanDoubleJump = false;
    //            }
    //            else
    //            {
    //                player.CanDoubleJump = true;
    //            }
                    


    //            break;

    //        // Glider
    //        case 1:
    //            if (player.CanGlide)
    //            {
    //                player.CanGlide = false;
    //            }
    //            else
    //            {
    //                player.CanGlide = true;
    //            }

    //            break;

    //        // Grapple
    //        case 2:
    //            if (player.CanGrapple)
    //            {
    //                player.CanGrapple = false;
    //            }
    //            else
    //            {
    //                player.CanGrapple = true;
    //            }

    //            break;

    //        default:

    //            Debug.LogError("ERROR: Toggle Out Of Range");

    //            break;
    //    }
    //}

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
