using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [Serializable]
    public class Walking : State
    {
        public float maxSpeed;
        public float acceleration;

        public override void OnFixedUpdate(PlayerController player)
        {
            //Put Walking movement related code here

            //Move in faced direction
            Vector3 moveDirection = player.GetDirection();
            player.Accelerate(moveDirection, maxSpeed, acceleration);

            //Apply movement to avatar
            //player.rb.AddForce(moveDirection * additionalSpeed, ForceMode.Force);

            //Slow player down when on the ground
            player.rb.drag = player.groundDrag;
        }

        public override void OnUpdate(PlayerController player)
        {
            if (player.input.Movement.Sprint.IsPressed())
            {
                player.SetState<Running>();
            }

            if (player.input.Movement.Jump.WasPressedThisFrame())
            {
                player.SetState<Jump>();
            }

            if (player.input.Movement.Crouch.IsPressed())
            {
                player.SetState<Crouch>();
            }

            if (!player.grounded)
            {
                player.SetState<Air>();
            }
        }
    }
}
