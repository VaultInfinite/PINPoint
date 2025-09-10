using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    public GameObject player;
    public Transform respawn;

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player.transform.position = respawn.transform.position;
        }
    }

    /**public GameObject playerContainer;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "playerContainer")
        {
            Destroy(playerContainer);
            //I will switch this to play the respawn code once I have made that
        }
    }**/
}
