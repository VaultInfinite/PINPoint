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
    public GameObject bullet;
    private Ray ray;
    [SerializeField]
    private float shootForce;

    [SerializeField]
    private float shootCooldown;
    [SerializeField]
    private PlayerController player;

    [Header("Aiming Variables")]
    [SerializeField]
    private float camZoom;      //Desired Camera FoV while Aiming
    private float deZoom;       //Original Camera FoV 
    private bool isAiming;      //See if aiming
    [SerializeField]
    private float zoomSpeed;    //How fast the camera zooms in and out
    private bool enemyAim;      //Check if aiming at enemy
    private bool targetAim;     //Check if aiming at Target

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

        if (player.input.Movement.Aim.IsPressed())
        {
            isAiming = true;
        }
        if (!player.input.Movement.Aim.IsPressed())
        {
            isAiming = false;
        }

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

        //Get center screen
        ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));

        //Set Shoot to false
        canShoot = false;


        RaycastHit hit;
        Vector3 targetPoint = ray.GetPoint(75);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            //Check Object's Tag
            switch (hit.transform.gameObject.tag)
            {
                //If there is no tag
                default:
                    //Make Aim Variables false;
                    enemyAim = false;
                    targetAim = false;
                    break;

                //If the tag is "Enemy"
                case "Enemy":
                    targetAim = false;
                    Debug.Log("Enemy Hit!");
                    enemyAim = true;
                    targetPoint = hit.point;
                    break;
            }

            switch (hit.transform.gameObject.GetComponent<NPC>().isTarget)
            {
                case true:
                    Debug.Log("KILL!");

                    //Prevent the bullet from being created
                    targetAim = true;

                    //The Target has been hit
                    GaMaControl.Instance.targetHit = true;

                    //Pull up win screen
                    GaMaControl.Instance.CashOut();

                    break;

                default:
                    //If aiming at an enemy, Do NOT go the the fail screen
                    if (!enemyAim) GaMaControl.Instance.Fail();

                    break;
            }
        }


        //If NOT Aimed at the target but at an Enemy, create bullet
        if (!targetAim)
        {
            //Get firing direction
            Vector3 bulletDir = targetPoint - cam.transform.position;

            //Create gameObject and store it
            GameObject currentBullet = Instantiate(bullet, cam.transform.position, Quaternion.identity);

            //Rotate bullet to desired direction
            currentBullet.transform.forward = bulletDir.normalized;

            //Add force to bullet
            currentBullet.GetComponent<Rigidbody>().AddForce(bulletDir.normalized * shootForce, ForceMode.Impulse);
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
