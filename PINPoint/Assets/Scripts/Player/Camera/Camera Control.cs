using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the player to control the camera. They can move it around without 
/// risking clicking off screen.
/// </summary>
public class CameraControl : MonoBehaviour
{
    //The variable that will be applied to the rotation
    public float sensX;
    public float sensY;

    //The variables that will be applied to the above variables when aiming
    public float aimX;
    public float aimY;

    public bool playerAiming;

    [SerializeField]
    private Transform orientation;
    private float xRotation;
    private float yRotation;


    // Start is called before the first frame update
    void Start()
    {
        //Hide Mouse and lock to screen
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if Paused
        if (Pause.isPaused)
        {
            //Lock amd hide cursor to screen
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        AimContext();
    }

    /// <summary>
    /// Determine what sensitivity to use
    /// </summary>
    private void AimContext()
    {
        if (!playerAiming)
        {
            RegularSens();
        }

        else
        {
            AimSens();
        }
    }

    //Applies the regular camera sensitivity
    private void RegularSens()
    {
        //Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yRotation += mouseX / sensX;
        xRotation -= mouseY / sensY;

        //Limits how far the player can look up and adown
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    //Applies the aiming camera sensitivity
    private void AimSens()
    {
        //Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yRotation += mouseX / aimX;
        xRotation -= mouseY / aimY;

        //Limits how far the player can look up and adown
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
