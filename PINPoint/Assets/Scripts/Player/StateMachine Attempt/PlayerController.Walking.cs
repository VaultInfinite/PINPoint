using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public class Walking : State
    {
        public override void OnUpdate(PlayerController player)
        {
            //Put Walking movement related code here

            if (player.input.Movement.Sprint.IsPressed())
            {
                player.SetState<Running>();
            }
        }
    }
}
