using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShotMissed : MonoBehaviour
{
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
    }
}
