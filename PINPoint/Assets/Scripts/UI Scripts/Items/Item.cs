using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class Item : ScriptableObject
{
    public int cost;

    
    public bool bought, enabled;

    public string description, displayName;
}
