using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShotMissed : MonoBehaviour
{
    //THIS SCRIPT WILL BE ATTACHED TO THE PROJECTILE 
    //THE THING THAT SHOOTS OUT OF THE PLAYER'S GUN

    private void OnTriggerEnter(Collider other)
    {
        //if the projectile doesn't touch the enemy
        if(other.gameObject.tag != "Enemy")
        {
            //send the player to the level fail screen
            //PUT THE SCENE NUMBER FOR LEVEL FAIL SCREEN IN THE PARENTHESIS BELOW
            //SceneManager.LoadScene();
            Debug.Log("You failed.");
        }

        //if the projectile does touch the enemy
        if(other.gameObject.tag == "Enemy")
        {
            //kill the enemy
            Destroy(other.gameObject);
            //send the player to the level complete screen (or equivalent)
            //PUT THE SCENE NUMBER FOR THE LEVEL COMPLETE SCREEN IN THE PARENTHESIS BELOW
            //SceneManager.LoadScene();
            Debug.Log("You killed the enemy.");
        }
    }
}
