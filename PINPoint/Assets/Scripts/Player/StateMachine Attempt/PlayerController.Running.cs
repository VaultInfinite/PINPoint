using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerController
{
    public class Running : State
    {
        public override void OnUpdate(PlayerController player)
        {
            //put running related code here

            if (!player.input.Movement.Sprint.IsPressed())
            {
                player.SetState<Walking>();
            }
        }
    }
}
