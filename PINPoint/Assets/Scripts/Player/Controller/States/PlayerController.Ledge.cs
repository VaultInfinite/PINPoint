using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [Serializable]
    public class Ledge : State
    {
        [SerializeField]
        private float raycastDistance;
        [SerializeField]
        private float ledgeHeightOffset = 0.5f;

        //Present to reequip certain weapons.
        private bool equipSniper;
        private bool equipGrapple;
        private bool equipShock;

        private Vector3 ledgeHandsLocation;
        private GameObject hands;

        public override void OnEnter(PlayerController player)
        {
            player.rb.velocity = Vector3.zero;

            if (player.ledgeHands != null)
            {
                hands = Instantiate(player.ledgeHands, ledgeHandsLocation, Quaternion.LookRotation(-player.orientation.forward));
            }
            if (player.shooting.enabled && player.shooting.playerGun == GunType.rifle)
            {
                player.shooting.enabled = false;
                player.sniperOBJ.SetActive(false);
                equipSniper = true;
            }
            if (player.shooting.enabled && player.shooting.playerGun == GunType.stun)
            {
                player.shooting.enabled = false;
                player.shockOBJ.SetActive(false);
                equipShock = true;
            }
            if (player.grapple.enabled)
            {
                player.grapple.enabled = false;
                player.grappleOBJ.SetActive(false);
                player.grapple.point.SetActive(false);
                player.grapple.gunHook.SetActive(true);
                player.grapple.lineRenderer.enabled = false;
                equipGrapple = true;
            }
        }

        public override void OnUpdate(PlayerController player)
        {
            if (player.input.Movement.Jump.IsPressed())
            {
                player.SetState<Jump>();
            }
        }

        public override void OnExit(PlayerController player)
        {
            Destroy(hands);
            player.air.ledgeGrabbed = true;
            if (equipSniper)
            {
                player.shooting.enabled = true;
                player.sniperOBJ.SetActive(true);
                equipSniper = false;
            }
            if (equipShock)
            {
                player.shooting.enabled = true;
                player.shockOBJ.SetActive(true);
                equipShock = false;
            }
            if (equipGrapple)
            {
                player.grapple.enabled = true;
                player.grappleOBJ.SetActive(true);
                equipGrapple = false;
            }
            
        }

        public bool CanLedgeGrab(PlayerController player)
        {
            bool canLedgeGrab = false;
            if (Physics.Raycast(player.transform.position, player.orientation.transform.forward, out RaycastHit forwardHit, raycastDistance, player.Ground))
            {
                Vector3 ledgeCheckOrigin = forwardHit.point + Vector3.up * ledgeHeightOffset;
                ledgeHandsLocation = ledgeCheckOrigin;
                ledgeHandsLocation.y = ledgeHandsLocation.y - 0.97f;
                
                if (Physics.Raycast(ledgeCheckOrigin, Vector3.down, out RaycastHit downwardHit, ledgeHeightOffset * 2, player.Ground))
                {
                    //ledgeHandsLocation.y = downwardHit.transform.position.y + 0.81f;
                    canLedgeGrab = (Vector3.Dot(downwardHit.normal, Vector3.up) > 0.8f);
                }
            }
            return canLedgeGrab;
        }

        public RaycastHit CanLedgeGrabRaycast(PlayerController player, out RaycastHit hit)
        {
            hit = default;
            if (Physics.Raycast(player.transform.position, player.orientation.transform.forward, out RaycastHit forwardHit, raycastDistance, player.Ground))
            {
                hit = forwardHit;
                return hit;
            }
            else
            {
                return hit;
            }
        }
    }
}