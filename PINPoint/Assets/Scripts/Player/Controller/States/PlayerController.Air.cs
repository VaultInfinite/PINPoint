using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's behavior in the air
/// </summary>
public partial class PlayerController
{
    [Serializable]
    public class Air : State
    {

        public float maxSpeed;
        public float acceleration;
        public float maxFallSpeed;
        public float gravity;

        //Accessed in WallRunning to allow detaching
        [HideInInspector]
        public bool doubleJumped;
        private bool wallRan;

        public override void OnFixedUpdate(PlayerController player)
        {
            //Apply movement in faced direction
            Vector3 moveDirection = player.GetDirection();
            player.Accelerate(moveDirection, maxSpeed, acceleration);

            //Move quickly in air
            player.rb.drag = 0;

            //Get player velocity
            Vector3 vel = player.rb.velocity;

            //Apply gravity
            vel.y -= gravity * Time.fixedDeltaTime;
            player.rb.velocity = vel;

            //Prevents player from falling too fast
            Vector3 flatVel = new Vector3(0f, player.rb.velocity.y, 0f);

            //Maintain downward velocity
            if (player.rb.velocity.y < -maxFallSpeed)
            {
                    Vector3 limitVel = flatVel.normalized * maxFallSpeed;

                    player.rb.velocity = new Vector3(player.rb.velocity.x, limitVel.y, player.rb.velocity.z);
            }
        }

        public override void OnUpdate(PlayerController player)
        {
            //If player is in the air and holds the jump key, player will glide
            if (player.input.Movement.Gliding.WasPressedThisFrame() && player.gliding.CanGlide())
            {
                player.SetState<Gliding>();
            }

            //If player is in the air and jumps, double jump if applicable
            if (player.input.Movement.Jump.WasPressedThisFrame() && !doubleJumped && (!Pause.isPaused))
            {
                doubleJumped = true;
                player.SetState<Jump>();
            }

            //If player is on the ground, change state to walking
            if (player.grounded)
            {
                //Reset aerial movement bools
                player.gliding.ResetGlide();
                doubleJumped = false;
                wallRan = false;

                player.SetState<Walking>();
            }

            //Wallrunning check here
            if (player.wall.CanWallRun(player) && !wallRan)
            {
                player.SetState<WallRunning>();
            }
        }
    }
}
