using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerController), typeof(LineRenderer))]
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
    private float yAddition;

    [SerializeField]
    private GameObject point;
    private LineRenderer lineRenderer;

    private Vector3 hookPoint;

    private void Awake()
    {
        player = gameObject.GetComponent<PlayerController>();

        lineRenderer = gameObject.GetComponent<LineRenderer>();
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
                point = Instantiate(point, hookPoint, Quaternion.identity);
                point.SetActive(true);
            }
            else
            {
                point.transform.position = hookPoint;
                point.transform.rotation = Quaternion.LookRotation(grappleHit.normal, Vector3.up);
                point.SetActive(true);
            }
            lineRenderer.SetPosition(0, hookPoint);
            lineRenderer.enabled = true;
        }
        if (player.input.Movement.Shoot.WasReleasedThisFrame())
        {
            hookPoint = Vector3.zero;
            point.SetActive(false);
            lineRenderer.enabled = false;
        }
        if (player.input.Movement.Shoot.IsPressed() && hookPoint != Vector3.zero)
        {
            lineRenderer.SetPosition(1, player.grappleOBJ.transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (player.input.Movement.Shoot.IsPressed() && hookPoint != Vector3.zero)
        {
            Grapple((hookPoint - player.transform.position).normalized, maxSpeed, acceleration);
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

        newVelocity.y += yAddition * Time.fixedDeltaTime;
        player.rb.velocity = newVelocity;
    }
}
