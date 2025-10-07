using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitByPolice : MonoBehaviour
{
   //this script will be attached to the projectile prefab for the police drone

    private void OnTriggerEnter(Collider other)
    {
        //if the projectile hits the player
        if(other.gameObject.tag == "Player")
        {
            //PUT SCENE NUMBER FOR LEVEL FAIL IN PARENTHESIS
            //SceneManager.LoadScene();
            Debug.Log("The player has failed.");

            //get rid of the projectile that hit the player
            Destroy(this.gameObject);
        }
    }

    //Note to self: please make sure all items, such as trigger boxes, are properly set up and checked in the inspector windows
}
