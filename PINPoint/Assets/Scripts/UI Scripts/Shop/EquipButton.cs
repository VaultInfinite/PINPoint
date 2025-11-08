using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    public TMP_Text nameText;
    public Toggle toggle;

    private Item item;
    public Item Item
    {
        get => item;
        set
        {
            item = value;
            nameText.text = value.displayName;
        }
    }

    public UnityEvent<Item, bool> onToggle;

    public void OnToggled(bool toggled)
    {
        onToggle.Invoke(item, toggled);
        item.enabled = toggled;
    }

    private void Update()
    {
        toggle.interactable = item.bought;
        toggle.isOn = item.enabled;
    }
}
