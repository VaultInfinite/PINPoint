using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public class Air : State
    {
        public override void OnUpdate(PlayerController player)
        {
            //Apply movement in faced direction
            player.rb.AddForce(player.moveDirection.normalized * player.moveSpeed * 10f * player.airMulti, ForceMode.Force);


            //Move quickly in air
            player.rb.drag = 0;

            //Get player velocity
            Vector3 vel = player.rb.velocity;

            //Apply gravity
            vel.y -= player.gravity * Time.deltaTime;
            player.rb.velocity = vel;

            //Prevents player from falling too fast
            Vector3 flatVel = new Vector3(0f, player.rb.velocity.y, 0f);

            //Limit Player X & Z Speed
            player.SpeedLimit();

            if (flatVel.magnitude > player.maxVel)
            {
                Vector3 limitVel = flatVel.normalized * player.maxVel;

                player.rb.velocity = new Vector3(player.rb.velocity.x, limitVel.y, player.rb.velocity.z);
            }

            if (player.grounded)
            {
                player.SetState<Walking>();
            }
        }
    }
}
