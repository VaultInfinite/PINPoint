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
        [SerializeField]
        private Camera playerCam;
        public float camZoom;
        private float deZoom;
        public bool isAiming;

        
        private float exitAimTimer = 1f;
        private bool aimCD;

        public override void OnEnter(PlayerController player)
        {
            //Get current FoV from Camera
            deZoom = playerCam.fieldOfView;
            
            aimCD = false;
        }

        public override void OnUpdate(PlayerController player)
        {
            //Return to Walking state 
            if (isAiming == true && player.input.Movement.Aim.IsPressed() && aimCD)
            {
                Debug.Log("Exit Aiming");
                isAiming = false;
                player.SetState<Walking>();
            }

            if (playerCam.fieldOfView == camZoom) return;
            playerCam.fieldOfView = camZoom;
        }

        /// <summary>
        /// Timer to set before being able to return from aiming
        /// </summary>
        public IEnumerator AimTimer()
        {
            aimCD = true;
            isAiming = true;
            yield return new WaitForSeconds(exitAimTimer);
        }

        public override void OnExit(PlayerController player)
        {

            playerCam.fieldOfView = deZoom;
        }
    }
}
