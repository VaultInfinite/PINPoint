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
        public float speed;

        public override void OnFixedUpdate(PlayerController player)
        {
            //put running related code here

            //Move in faced direction
            Vector3 moveDirection = player.GetDirection();


            //Apply movement to avatar
            player.rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

            //Prevent the player from going too fast
            player.SpeedLimit(speed);

            //Slow player down when on the ground
            player.rb.drag = player.groundDrag;
        }

        public override void OnUpdate(PlayerController player)
        {
            if (!player.input.Movement.Sprint.IsPressed())
            {
                player.SetState<Walking>();
            }

            if (player.input.Movement.Jump.WasPressedThisFrame())
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
