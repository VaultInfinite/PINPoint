using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public partial class PlayerController
{
    public class Walking : State
    {
        public override void OnUpdate(PlayerController player)
        {
            //Put Walking movement related code here

            player.moveSpeed = player.tempSpeed;

            //Move in faced direction

            player.OnMovement(player.input.Movement);
            player.GetDirection();

            //Apply movement to avatar
            player.rb.AddForce(player.moveDirection.normalized * player.moveSpeed * 10f, ForceMode.Force);

            //Prevent the player from going too fast
            player.SpeedLimit();

            //Slow player down when on the ground
            player.rb.drag = player.groundDrag;

            if (player.input.Movement.Sprint.IsPressed())
            {
                player.SetState<Running>();
            }

            if (player.input.Movement.Jump.IsPressed())
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
