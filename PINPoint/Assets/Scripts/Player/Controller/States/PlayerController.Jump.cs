using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's jumping behavior
/// </summary>
public partial class PlayerController
{
    [Serializable]
    public class Jump : State
    {
        public float jumpForce;

        public override void OnFixedUpdate(PlayerController player)
        {
            //Put Jumping movement related code here

            //Makes player jump the same height
            player.rb.velocity = new Vector3(player.rb.velocity.x, 0f, player.rb.velocity.z);

            //Jump
            player.rb.AddForce(player.transform.up * jumpForce, ForceMode.Impulse);
            
            player.SetState<Air>();
        }
    }
}
