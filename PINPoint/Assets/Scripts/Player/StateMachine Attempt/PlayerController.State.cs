using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public class State
    {
        public virtual void OnEnter(PlayerController player) { }
        public virtual void OnFixedUpdate(PlayerController player) { }
        public virtual void OnUpdate(PlayerController player) { }
        public virtual void OnExit(PlayerController player) { }
    }
}
