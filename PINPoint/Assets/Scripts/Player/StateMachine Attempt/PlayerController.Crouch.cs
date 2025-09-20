using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public class Crouch : State
    {
        public override void OnUpdate(PlayerController player)
        {
            //put crouching related code here

            player.moveSpeed = player.crouchSpeed;

            //Move in faced direction
            player.OnMovement(player.input.Movement);
            player.GetDirection();

            //Shrinks Player
            player.transform.localScale = new Vector3(player.transform.localScale.x, player.crouchScaleY, player.transform.localScale.z);

            //Pushes player down so they dont float in the air when crouching
            player.rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            player.crouching = true;

            if (!player.input.Movement.Crouch.IsPressed())
            {
                player.crouching = false;
                player.transform.localScale = new Vector3(player.transform.localScale.x, player.crouchScaleY * 2, player.transform.localScale.z);
                player.SetState<Walking>();
            }

            if (!player.grounded)
            {
                player.SetState<Air>();
            }
        }
    }
}