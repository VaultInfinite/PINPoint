using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [Serializable]
    public class Gliding : State
    {

        public float glideSpeed;
        public float glideTurn;
        public float glideDecline;
        public float maxGlideTime;
        private float glideTime;

        private Vector3 direction;

        public override void OnEnter(PlayerController player)
        {

            //Set the input direction for the player velocity
            direction = player.rb.velocity;
            direction.y = 0;
            direction = direction.normalized;
        }

        public override void OnFixedUpdate(PlayerController player)
        {

            //Move in faced direction
            direction += Camera.main.transform.forward * glideTurn * Time.fixedDeltaTime;
            direction = direction.normalized;

            //Apply movement to avatar
            player.rb.AddForce(direction * glideSpeed * 10f, ForceMode.Force);

            //Add time to glideTime
            glideTime += Time.fixedDeltaTime;

            player.rb.velocity = new Vector3(player.rb.velocity.x, -glideDecline, player.rb.velocity.z);

            player.SpeedLimit(glideSpeed);

            
        }

        public override void OnUpdate(PlayerController player)
        {
            //Checks if gliding button was released, or if glideTime has surpassed maxGlideTime
            if (player.input.Movement.Gliding.WasReleasedThisFrame() || !CanGlide())
            {
                player.SetState<Air>();
            }
            //Checks if player is grounded
            if (player.grounded)
            {
                player.SetState<Walking>();
            }
        }

        public bool CanGlide()
        {
            return glideTime < maxGlideTime;
        }

        /// <summary>
        /// Called in Air State upon touching the ground, sets glide time back to zero
        /// </summary>
        public void ResetGlide()
        {
            glideTime = 0;
        }
    }
}
