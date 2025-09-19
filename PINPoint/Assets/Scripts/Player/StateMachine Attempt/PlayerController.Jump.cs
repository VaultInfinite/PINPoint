using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public class Jump : State
    {
        public override void OnUpdate(PlayerController player)
        {
            //Put Jumping movement related code here

            //Prevent double jumping
            player.readyToJump = false;

            //Makes player jump the same height
            player.rb.velocity = new Vector3(player.rb.velocity.x, 0f, player.rb.velocity.z);

            //Jump
            player.rb.AddForce(player.transform.up * player.jumpForce, ForceMode.Impulse);

            if (player.input.Movement.Jump.IsPressed() & player.doublejump)
            {
                //RERUN JUMP FUNCTIONALITY
            }

            if (player.rb.velocity.y <= 0f)
            {
                player.SetState<Air>();
            }
        }
    }
}
