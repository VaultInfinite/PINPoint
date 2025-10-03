using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class PlayerController
{
    [Serializable]
    public class WallRunning : State
    {

        public float maxSpeed;
        public float acceleration;
        public float wallGravity;
        public float maxWallTime;

        private float wallTime;
        [SerializeField]
        private float bumpForce;
        private Vector3 cameraForward;

        public AnimationCurve curve;

        public override void OnEnter(PlayerController player)
        {
            //Set time to zero every time state is entered
            wallTime = 0;

            cameraForward = Camera.main.transform.forward;
        }

        public override void OnFixedUpdate(PlayerController player)
        {
            //While state is running, add to the wallTime based on time passed.
            wallTime += Time.fixedDeltaTime;

            float curveMult = curve.Evaluate(wallTime / maxWallTime);

            //Get player velocity
            Vector3 vel = player.rb.velocity;

            bool canWallRun = CanWallRun(player, out RaycastHit hitInfo);

            Vector3 direction = player.GetDirection();

            if (canWallRun)
            {
                Vector3 forward = Vector3.Cross(Vector3.up, hitInfo.normal);
                if (Vector3.Dot(forward, cameraForward) < 0)
                {
                    forward = -forward;
                }

                //Apply gravity
                vel.y -= wallGravity * Time.fixedDeltaTime * (1 - curveMult);
                player.rb.velocity = vel;

                //Apply force
                player.Accelerate(forward, maxSpeed, acceleration);
            }

            if (wallTime >= maxWallTime || !canWallRun || player.input.Movement.Jump.WasPressedThisFrame() && player.air.doubleJumped && CanJump())
            {
                player.SetState<Air>();
            }

            if (player.input.Movement.Jump.IsPressed() && CanJump())
            {
                player.jump.direction = hitInfo.normal;
                player.SetState<Jump>();
            }

            if (Vector3.Dot(direction, hitInfo.normal) > 0.5f)
            {
                player.rb.velocity += hitInfo.normal * bumpForce;
                player.SetState<Air>();
            }
        }

        /// <summary>
        /// Version of the WallRun check without Raycast return for basic checking purposes
        /// </summary>
        /// <param name="player">You, dummy</param>
        /// <returns>Which side of the player has returned a wall hit</returns>
        public bool CanWallRun(PlayerController player)
        {
            bool leftHit = Physics.Raycast(player.transform.position, -Camera.main.transform.right, player.playerRadius + 0.02f, player.Ground);
            bool rightHit = Physics.Raycast(player.transform.position, Camera.main.transform.right, player.playerRadius + 0.02f, player.Ground);
            return leftHit || rightHit;
        }

        public bool CanWallRun(PlayerController player, out RaycastHit hitInfo)
        {
            hitInfo = default;
            bool leftHit = Physics.Raycast(player.transform.position, -Camera.main.transform.right, out RaycastHit leftInfo, player.playerRadius + 0.02f, player.Ground);
            if (leftHit)
            {
                hitInfo = leftInfo;
            }
            //Debug.DrawRay(player.transform.position, -Camera.main.transform.right * (player.playerRadius + 0.02f), Color.red);
            bool rightHit = Physics.Raycast(player.transform.position, Camera.main.transform.right, out RaycastHit rightInfo, player.playerRadius + 0.02f, player.Ground);
            if (rightHit)
            {
                hitInfo = rightInfo;
            } 
            //Debug.DrawRay(player.transform.position, Camera.main.transform.right * (player.playerRadius + 0.02f), Color.red);
            return leftHit || rightHit;
        }

        /// <summary>
        /// Checking if player can jump to stop multiple jumps
        /// </summary>
        /// <returns></returns>
        private bool CanJump()
        {
            return wallTime >= 0.2f;
        }

        /// <summary>
        /// Checks direction of the wall the player is running on.
        /// </summary>
        /// <param name="player">Yourself</param>
        /// <returns>If true, running on a left wall, if false, running on a right wall</returns>
        public bool IsOnLeftWall(PlayerController player)
        {
            return Physics.Raycast(player.transform.position, -Camera.main.transform.right, player.playerRadius + 0.02f, player.Ground);
        }
    }
}
