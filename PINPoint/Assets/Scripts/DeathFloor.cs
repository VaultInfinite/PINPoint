using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    public GameObject player;
    public Transform respawn;

    public void Respawn()
    {
        player.transform.position = new Vector3(respawn.transform.position.x, respawn.transform.position.y + 2, respawn.transform.position.z);
    }

    public void Checkpoint(Transform spawnPoint)
    {
        respawn = spawnPoint.transform;
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
