using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public TMP_Text nameText, costText;
    public Button button;

    private Item item;
    public Item Item 
    { 
        get => item;
        set
        {
            item = value;
            nameText.text = value.displayName;
            costText.text = value.cost.ToString("$#,#", CultureInfo.InvariantCulture);
        } 
    }

    public UnityEvent<Item> onSelected;

    public void OnPressed()
    {
        onSelected.Invoke(item);
    }

    private void Update()
    {
        if (item.bought)
        {
            costText.text = "Bought";
        }
    }
}
