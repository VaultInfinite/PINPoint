using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public class Walking : State
    {
        public override void OnUpdate(PlayerController player)
        {
            //Put Walking movement related code here

            //Move in faced direction
            player.GetDirection();

            player.moveSpeed = player.tempSpeed;

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
                player.SetState<Air>();
            }

            if (player.input.Movement.Crouch.IsPressed())
            {
                player.SetState<Crouch>();
            }
        }
    }
}
