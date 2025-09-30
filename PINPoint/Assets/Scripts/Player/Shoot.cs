using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the player shoot while aiming
/// </summary>
public class Shoot : MonoBehaviour
{
    //Variables
    public Transform FirePos;
    private bool canShoot = true;
    public float shootCooldown;

    

    //Shoot the bullet
    public void Shooting()
    {
        //Check if can shoot
        if (!canShoot) return;

        RaycastHit hit;
        if (Physics.Raycast(FirePos.position, FirePos.forward, out hit, 100))
        {
            //Set Shoot to false
            canShoot = false;

            switch (hit.collider.gameObject.tag)
            {
                default:
                    //Don't do anything
                    Debug.DrawRay(FirePos.position, FirePos.forward * 100, Color.yellow);
                    return;

                case "Target":
                    Debug.Log("Take the shot juju!");
                    Debug.DrawRay(FirePos.position, FirePos.forward * 100, Color.red);

                    break;
            }
            
        }

        //Call timer
        StartCoroutine(ShootingCooldown(shootCooldown));
    }

    IEnumerator ShootingCooldown(float timer)
    {
        yield return new WaitForSeconds(timer);

        canShoot = true;
    }
}
