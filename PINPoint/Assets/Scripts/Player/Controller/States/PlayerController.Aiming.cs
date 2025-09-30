using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Puts the player in the state where the camera zooms in
/// 
/// CURRENT ISSUE: THE CAMERA DOES NOT LEAVE THE ZOOM STATE
/// </summary>
public partial class PlayerController
{
    [Serializable]
    public class Aiming : State
    {
        //Variables
        public Camera playerCam;
        public float camZoom;
        public float deZoom;
        public bool isAiming;
        public float zoomSpeed;
        public GameObject reticle;

        public override void OnEnter(PlayerController player)
        {
            //Get current FoV from camera
            deZoom = Mathf.Round(playerCam.fieldOfView);
            isAiming = true;
        }

        public override void OnUpdate(PlayerController player)
        {
            if (Pause.isPaused) return;

            //Zoom In
            if (isAiming)
            {
                CameraMoveEffect(camZoom);

                reticle.SetActive(true);
            }

            //If Shoot button is pressed while fully zoomed in, take the shot!
            if (Mathf.Round(playerCam.fieldOfView) == camZoom && player.input.Movement.Shoot.IsPressed())
            {
                //Shoot
                player.shootScr.Shooting();
            }

            //Return to Walking state 
            if (!player.input.Movement.Aim.IsPressed())
            {
                isAiming = false;
                reticle.SetActive(false);

                CameraMoveEffect(deZoom);

                //Return to 'Neutral' State
                if (Mathf.Round(playerCam.fieldOfView) == deZoom) player.SetState<Walking>();
            }
        }

        public override void OnExit(PlayerController player)
        {
            //Zoom out
            //ZoomOut();
            
        }

        /// <summary>
        /// Make the camera move to desired position
        /// </summary>
        /// <param name="targetView">The view of the camera</param>
        void CameraMoveEffect(float targetView)
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetView, Time.deltaTime * zoomSpeed);
        }
    }
}
