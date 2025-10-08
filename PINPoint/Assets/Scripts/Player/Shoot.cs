using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

/// <summary>
/// Makes the player shoot while aiming
/// </summary>
public class Shoot : MonoBehaviour
{
    [Header("Shooting Variables")]
    //Variables
    private Transform FirePos;
    private Camera cam;
    private bool canShoot = true;

    [SerializeField]
    private float shootCooldown;
    [SerializeField]
    private PlayerController player;

    [Header("Aiming Variables")]
    public float camZoom;
    public float deZoom;
    public bool isAiming;
    public float zoomSpeed;

    private void Awake()
    {
        //Get Camera
        cam = Camera.main;
        FirePos = Camera.main.transform;

        //Get Camera FoV
        deZoom = Mathf.Round(cam.fieldOfView);
    }

    private void Update()
    {
        if (Pause.isPaused) return;

        //Zoom In
        if (isAiming)
        {
            CameraMoveEffect(camZoom);

            GaMaControl.Instance.reticle.SetActive(true);
        }

        //If Shoot button is pressed while fully zoomed in, take the shot!
        if (Mathf.Round(cam.fieldOfView) == camZoom && player.input.Movement.Shoot.IsPressed())
        {
            //Shoot
            Shooting();
        }

        //Return to Walking state 
        if (!player.input.Movement.Aim.IsPressed())
        {
            isAiming = false;
            GaMaControl.Instance.reticle.SetActive(false);

            CameraMoveEffect(deZoom);
        }
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

    /// <summary>
    /// Make the camera move to desired position
    /// </summary>
    /// <param name="targetView">The view of the camera</param>
    void CameraMoveEffect(float targetView)
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetView, Time.deltaTime * zoomSpeed);
    }

    IEnumerator ShootingCooldown(float timer)
    {
        yield return new WaitForSeconds(timer);

        canShoot = true;
    }
}
