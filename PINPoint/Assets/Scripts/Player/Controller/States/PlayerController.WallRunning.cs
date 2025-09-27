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

        public float wallSpeed;
        public float wallGravity;
        public float maxWallTime;

        private float wallTime;
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

            if (canWallRun)
            {
                Vector3 forward = Vector3.Cross(Vector3.up, hitInfo.normal);
                if (Vector3.Dot(forward, cameraForward) < 0)
                {
                    forward = -forward;
                }

                //Apply gravity
                vel.y -= wallGravity * Time.fixedDeltaTime * (1 - curveMult);

                //Debug.Log(curveMult);

                player.rb.velocity = vel;

                player.rb.AddForce(forward * wallSpeed * curveMult * 10f, ForceMode.Force);

                player.SpeedLimit(wallSpeed);
            }

            if (wallTime >= maxWallTime || !canWallRun || player.input.Movement.Jump.WasPressedThisFrame() && player.air.doubleJumped && CanJump())
            {
                player.SetState<Air>();
            }

            if (player.input.Movement.Jump.IsPressed() && !player.air.doubleJumped && CanJump())
            {
                player.air.doubleJumped = true;
                player.SetState<Jump>();
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
        public bool CanJump()
        {
            return wallTime >= 0.2f;
        }
    }
}
