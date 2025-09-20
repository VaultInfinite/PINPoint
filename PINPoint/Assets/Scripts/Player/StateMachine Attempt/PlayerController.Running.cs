using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerController
{
    public class Running : State
    {
        public override void OnUpdate(PlayerController player)
        {
            //put running related code here

            player.moveSpeed = player.runSpeed;

            //Move in faced direction
            player.OnMovement(player.input.Movement);
            player.GetDirection();


            //Apply movement to avatar
            player.rb.AddForce(player.moveDirection.normalized * player.moveSpeed * 10f, ForceMode.Force);

            //Prevent the player from going too fast
            player.SpeedLimit();

            //Slow player down when on the ground
            player.rb.drag = player.groundDrag;

            if (!player.input.Movement.Sprint.IsPressed())
            {
                player.SetState<Walking>();
            }

            if (player.input.Movement.Jump.IsPressed())
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
