using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the enemy's behavior
/// </summary>
public class StunControl : MonoBehaviour
{
    //Variables
    public float stunTimer;         //How long the enemy stays stunned
    public bool stunned = false;   //Determines behavior

    //Stun enemy
    public void Stunned()
    {
        Debug.Log("Im STUNNED");
        stunned = true;
        StartCoroutine(StunLength());
    }

    //After 'X' seconds, unstun enemy
    IEnumerator StunLength()
    {
        yield return new WaitForSeconds(stunTimer);

        Debug.Log("No Longer Stunned");

        stunned = false;
    }
}
