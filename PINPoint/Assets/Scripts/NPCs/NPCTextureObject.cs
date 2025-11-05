using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCTextures", menuName = "ScriptableObjects/NPCTextureScriptableObject", order = 1)]
public class NPCTextureObject : ScriptableObject
{
    public List<Material> materials;
}
