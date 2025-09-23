using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [Serializable]
    public class Walking : State
    {
        public float speed;

        public override void OnUpdate(PlayerController player)
        {
            //Put Walking movement related code here

            //Move in faced direction
            Vector3 moveDirection = player.GetDirection();

            //Apply movement to avatar
            player.rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

            //Prevent the player from going too fast
            player.SpeedLimit(speed);

            //Slow player down when on the ground
            player.rb.drag = player.groundDrag;

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
