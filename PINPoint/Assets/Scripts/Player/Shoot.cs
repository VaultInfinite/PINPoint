using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

/// <summary>
/// What type of gun the player has.
/// Currently used for cooldowns
/// </summary>
enum GUNTYPE
{
    stun,
    rifle
}

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
    [SerializeField]
    private GUNTYPE playerGun;

    [Header("Stun Gun")]
    [SerializeField]
    private float stunGunCD = 7f;
    private StunShoot stunShootScr;

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
        stunShootScr = GetComponent<StunShoot>();
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

        //Set Shoot to false
        canShoot = false;


        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit))
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.GetComponent<NPC>() && playerGun == GUNTYPE.rifle)
            {
                switch (hitObject.GetComponent<NPC>().isTarget)
                {
                    case true:
                        Debug.Log("KILL!");

                        //The Target has been hit
                        GaMaControl.Instance.targetHit = true;

                        //Pull up win screen
                        GaMaControl.Instance.CashOut();

                        break;
                    case false:

                        GaMaControl.Instance.Fail();

                        break;
                }
            }
            if (hitObject.GetComponent<PoliceDrone>() && playerGun == GUNTYPE.stun)
            {
                stunShootScr.ShootStun();
            }
            else
            {

                GaMaControl.Instance.Fail();
            }
        }
        else
        {

            GaMaControl.Instance.Fail();
        }

        

        //Check Gun Type
        GetGunCooldown();

        //Call timer
        StartCoroutine(ShootingCooldown(shootCooldown));
    }

    /// <summary>
    /// Check what kind of gun the player has and adjust shooting cooldown
    /// </summary>
    /// <param name="gun">type of gun the player has</param>
    private void GetGunCooldown()
    {
        switch (playerGun)
        {
            case GUNTYPE.stun:

                shootCooldown = stunGunCD;

                break;

            default:

                Debug.LogError("There is no Gun Type!");

                break;
        }
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
