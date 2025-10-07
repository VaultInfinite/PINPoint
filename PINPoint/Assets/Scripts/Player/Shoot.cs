using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the player shoot while aiming
/// </summary>
public class Shoot : MonoBehaviour
{
    //Variables
    private Transform FirePos;
    private Camera cam;
    private bool canShoot = true;

    [SerializeField]
    private float shootCooldown;

    private void Awake()
    {
        cam = Camera.main;
        FirePos = Camera.main.transform;
    }

    //Shoot the bullet
    public void Shooting()
    {
        //Check if can shoot
        if (!canShoot) return;

        //Set Shoot to false
        canShoot = false;


        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            switch (hit.transform.gameObject.tag)
            {
                case "Target":
                    Debug.Log("KILL!");

                    //The Target has been hit
                    GaMaControl.Instance.targetHit = true;

                    //Pull up win screen
                    GaMaControl.Instance.CashOut();

                    //Destroy the gameobject
                    //Can be altered to any other die function
                    Destroy(hit.transform.gameObject);

                    break;

                default:

                    //Do Nothing

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
