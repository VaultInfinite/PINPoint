using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [Serializable]
    public class Air : State
    {

        public float speed;
        public float maxFallSpeed;
        public float gravity;

        private bool doubleJumped;
        private bool wallRan;

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

            if (player.rb.velocity.y < -maxFallSpeed)
            {
                Vector3 limitVel = flatVel.normalized * maxFallSpeed;

                player.rb.velocity = new Vector3(player.rb.velocity.x, limitVel.y, player.rb.velocity.z);
            }

            if (player.input.Movement.Jump.WasPressedThisFrame() && !doubleJumped)
            {
                doubleJumped = true;
                player.SetState<Jump>();
            }

            if (player.grounded)
            {
                doubleJumped = false;
                wallRan = false;
                player.SetState<Walking>();
            }

            //Wallrunning check here
            if (player.wall.CanWallRun(player) && !wallRan)
            {
                wallRan = true;
                player.SetState<WallRunning>();
            }
        }
    }
}
