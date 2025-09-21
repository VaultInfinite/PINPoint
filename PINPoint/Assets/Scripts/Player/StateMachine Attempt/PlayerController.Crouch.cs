using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's crouching behavior
/// </summary>
public partial class PlayerController
{
    [Serializable]
    public class Crouch : State
    {
        public float speed;
        public float crouchScaleY;
        private float startScaleY;

        public override void OnEnter(PlayerController player)
        {
            //Finding Size of Player
            startScaleY = player.transform.localScale.y;
        }

        public override void OnUpdate(PlayerController player)
        {
            //put crouching related code here

            //Shrinks Player
            player.transform.localScale = new Vector3(player.transform.localScale.x, crouchScaleY, player.transform.localScale.z);

            //Pushes player down so they dont float in the air when crouching
            player.rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            if (!player.input.Movement.Crouch.IsPressed())
            {
                player.transform.localScale = new Vector3(player.transform.localScale.x, startScaleY, player.transform.localScale.z);
                player.SetState<Walking>();
            }

            if (!player.grounded)
            {
                player.SetState<Air>();
            }
        }
    }
}