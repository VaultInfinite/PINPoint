using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grappling : MonoBehaviour
{
    private PlayerController player;
    private Camera cam;

    [SerializeField]
    private float hookRange;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float yMultiplier;

    [SerializeField]
    private GameObject point;

    private Vector3 hookPoint;

    private void Awake()
    {
        if (gameObject.GetComponent<PlayerController>())
        {
            player = gameObject.GetComponent<PlayerController>();
        }
        cam = Camera.main;
    }
    private void Update()
    {
        bool hookHit = CanGrapple(out RaycastHit grappleHit);
        if (player.input.Movement.Shoot.WasPressedThisFrame() && hookHit)
        {
            hookPoint = grappleHit.point;
            if (point.transform.position == Vector3.zero)
            {
                point = Instantiate(point, hookPoint, cam.transform.rotation);
                point.SetActive(true);
            }
            else
            {
                point.transform.position = hookPoint;
                point.SetActive(true);
            }
        }
        if (player.input.Movement.Shoot.IsPressed() && hookPoint != Vector3.zero)
        {
            Grapple((hookPoint - player.transform.position).normalized, maxSpeed, acceleration);
        }
        if (player.input.Movement.Shoot.WasReleasedThisFrame())
        {
            hookPoint = Vector3.zero;
            point.SetActive(false);
        }
    }

    private bool CanGrapple(out RaycastHit grappleHit)
    {
        bool hookHit = Physics.Raycast(cam.transform.position, cam.transform.forward, out grappleHit, hookRange, player.Ground);
        return hookHit;
    }

    private void Grapple(Vector3 moveDirection, float maxSpeed, float acceleration)
    {
        Vector3 velocity = player.rb.velocity;
        float product = Vector3.Dot(moveDirection, velocity);
        float accel = acceleration * Time.deltaTime;
        if (product + accel > maxSpeed)
        {
            accel = maxSpeed - product;
        }
        Vector3 newVelocity = velocity + moveDirection * accel;
        newVelocity = new Vector3(newVelocity.x, (newVelocity.y), newVelocity.z);

        newVelocity.y = newVelocity.y * yMultiplier;
        player.rb.velocity = newVelocity;
    }
}
