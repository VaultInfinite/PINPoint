using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    public GameObject playerContainer;
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject)
        {
            Destroy(playerContainer);
            //I will switch this to play the respawn code once I have made that
        }
    }
}
