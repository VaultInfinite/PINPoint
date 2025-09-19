using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public class Crouch : State
    {
        public override void OnUpdate(PlayerController player)
        {
            player.rb.AddForce(player.moveDirection.normalized * player.crouchSpeed * 10f, ForceMode.Force);

            //Shrinks Player
            player.transform.localScale = new Vector3(player.transform.localScale.x, player.crouchScaleY, player.transform.localScale.z);

            //Pushes player down so they dont float in the air when crouching
            player.rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            player.crouching = true;

            //Prevents sprinting while crouching
            player.sprinting = false;

            if (!player.input.Movement.Crouch.IsPressed())
            {
                player.SetState<Walking>();
            }
        }
    }
}