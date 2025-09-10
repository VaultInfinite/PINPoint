using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    public GameObject playerContainer;
    private GameObject spawnPoint;

    private void Start()
    {
        //GetComponent(spawnPoint.transform.position);
        //spawnPoint = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "playerContainer")
        {
            //Destroy(playerContainer);
            //I will switch this to play the respawn code once I have made that
            //transform.position = spawnPoint;
        }
    }
}
