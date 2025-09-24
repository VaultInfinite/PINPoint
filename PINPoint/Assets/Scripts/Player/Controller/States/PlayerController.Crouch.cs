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

            //Move in faced direction
            Vector3 moveDirection = player.GetDirection();


            //Apply movement to avatar
            player.rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

            player.SpeedLimit(speed);
            if (!player.input.Movement.Crouch.IsPressed())
            {
                ReturnShape(player);
                player.SetState<Walking>();
            }

            if (!player.grounded)
            {
                ReturnShape(player);
                player.SetState<Air>();
            }

            if (player.input.Movement.Aim.IsPressed())
            {
                ReturnShape(player);
                player.SetState<Aiming>();
            }
        }

        /// <summary>
        /// Returns the player to their original shape
        /// </summary>
        void ReturnShape(PlayerController player)
        {
            player.transform.localScale = new Vector3(player.transform.localScale.x, startScaleY, player.transform.localScale.z);
        }
    }
}