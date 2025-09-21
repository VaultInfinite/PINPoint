using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Controls the player's behavior in the air
/// </summary>
public partial class PlayerController
{
    [Serializable]
    public class Air : State
    {

        public float speed;
        public float maxFallSpeed;
        public float gravity;

        private bool doubleJumped;

        public float glideSpeed;
        private bool gliding;
        private bool canGlide;
        public float glideTimer;
        public float glideTimeRefill;


        public override void OnUpdate(PlayerController player)
        {
            //Apply movement in faced direction
            Vector3 moveDirection = player.GetDirection();

            player.rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

            //Move quickly in air
            player.rb.drag = 0;

            //Get player velocity
            Vector3 vel = player.rb.velocity;

            //Apply gravity
            vel.y -= gravity * Time.deltaTime;
            player.rb.velocity = vel;

            //Prevents player from falling too fast
            Vector3 flatVel = new Vector3(0f, player.rb.velocity.y, 0f);

            //Limit Player X & Z Speed
            player.SpeedLimit(speed);

            //if spacebar is pressed, glide
            if (player.input.Movement.Jump.IsPressed())
            {
                gliding = true;
            }

            else
            {
                gliding = false;
            }


            //If the player is holding down the gliding button
            if (gliding && canGlide && player.rb.velocity.y < -glideSpeed)
            {
                if (glideTimer <= 0)
                {
                    gliding = false;
                    canGlide = false;
                    return;
                }

                Vector3 limitGlideVel = flatVel.normalized * glideSpeed;

                player.rb.velocity = new Vector3(player.rb.velocity.x, limitGlideVel.y, player.rb.velocity.z);

                glideTimer = glideTimer - Time.deltaTime;
            }

            //Maintain downward velocity
            else if (player.rb.velocity.y < -maxFallSpeed)
            {
                
                    Vector3 limitVel = flatVel.normalized * maxFallSpeed;

                    player.rb.velocity = new Vector3(player.rb.velocity.x, limitVel.y, player.rb.velocity.z);
                
            }

            //If player is in the air and jumps, double jump if applicable
            if (player.input.Movement.Jump.WasPressedThisFrame() && !doubleJumped)
            {
                doubleJumped = true;
                player.SetState<Jump>();
            }

            //If player is on the ground, change state to walking
            if (player.grounded)
            {
                //Reset Glide
                canGlide = true;
                glideTimer = glideTimeRefill;

                doubleJumped = false;
                player.SetState<Walking>();
            }
        }


    }
}
