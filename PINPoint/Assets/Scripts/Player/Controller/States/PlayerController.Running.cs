using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Controls the player's running behavior
/// </summary>
public partial class PlayerController
{
    [Serializable]
    public class Running : State
    {
        public float maxSpeed;
        public float acceleration;

        public override void OnFixedUpdate(PlayerController player)
        {
            //put running related code here

            //Slow player down when on the ground
            player.rb.drag = player.groundDrag;

            //Move in faced direction
            Vector3 moveDirection = player.GetDirection();
            player.Accelerate(moveDirection, maxSpeed, acceleration);
        }

        public override void OnUpdate(PlayerController player)
        {
            if (!player.input.Movement.Sprint.IsPressed())
            {
                player.SetState<Walking>();
            }

            if (player.input.Movement.Jump.WasPressedThisFrame() && (!Pause.isPaused))
            {
                player.SetState<Jump>();
            }

            if (!player.grounded)
            {
                player.SetState<Air>();
            }
        }
    }
}
