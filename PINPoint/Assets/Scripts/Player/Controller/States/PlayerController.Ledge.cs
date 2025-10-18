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

        public override void OnEnter(PlayerController player)
        {
            player.rb.velocity = Vector3.zero;
        }

        public override void OnUpdate(PlayerController player)
        {
            if (player.input.Movement.Jump.IsPressed())
            {
                player.SetState<Jump>();
            }

            if (!CanLedgeGrab(player))
            {
                player.SetState<Air>();
            }
        }

        public override void OnExit(PlayerController player)
        {
            
        }

        public bool CanLedgeGrab(PlayerController player)
        {
            bool canLedgeGrab = false;
            if (Physics.Raycast(player.transform.position + Vector3.up * 0.5f, player.transform.forward, out RaycastHit forwardHit, raycastDistance, player.Ground))
            {
                Vector3 ledgeCheckOrigin = forwardHit.point + Vector3.up * ledgeHeightOffset;
                if (Physics.Raycast(ledgeCheckOrigin, Vector3.down, out RaycastHit downwardHit, ledgeHeightOffset * 2, player.Ground))
                {
                    canLedgeGrab = (Vector3.Dot(downwardHit.normal, Vector3.up) > 0.8f);
                }
            }
            return canLedgeGrab;
        }
    }
}