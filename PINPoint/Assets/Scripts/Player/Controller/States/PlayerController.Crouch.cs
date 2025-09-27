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

        public override void OnFixedUpdate(PlayerController player)
        {
            //put crouching related code here

            //Shrinks Player
            player.transform.localScale = new Vector3(player.transform.localScale.x, crouchScaleY, player.transform.localScale.z);

            //Move in faced direction
            Vector3 moveDirection = player.GetDirection();


            //Apply movement to avatar
            player.rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

            player.SpeedLimit(speed);
        }

        public override void OnUpdate(PlayerController player)
        {
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