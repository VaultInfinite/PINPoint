using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject playerController;
    public static DeathFloor Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == playerController)
        {
            collision.transform.position = respawnPoint.position;
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
