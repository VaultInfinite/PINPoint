using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    [Header("References")]
    public PlayerBehavior pb;
    public Transform orientation;
    public Transform cam;
    public Rigidbody rb;
    [Header("Edges")]
    public float ledgeDetectionLength;
    public float ledgeSphereCastRadius;
    public LayerMask whatIsLedge;
    [Header("Grabbing")]
    public float moveToLedgeSpeed;
    public float maxLedgeGrabDistance;
    public float timeOnLedge;
    public float minTimeOnLedge;

    public bool holding;

    private Transform curLedge;
    private Transform prevLedge;
    private RaycastHit ledgeHit;
  
    private void DetectLedge()
    {
        ///find ledge, look for distance, find only what is a ledge
        bool ledgeFound = Physics.SphereCast(transform.position, ledgeSphereCastRadius, cam.forward, out ledgeHit, ledgeDetectionLength, whatIsLedge);
        ///if you didnt find ledge, go back to normal
        if (!ledgeFound) return;
        ///Found the ledge
        float distancetoLedge = Vector3.Distance(transform.position, ledgeHit.transform.position);

        if (ledgeHit.transform == prevLedge) return;

        if (distancetoLedge < maxLedgeGrabDistance && !holding) holdLedge();
    }

    private void Update()
    {
        DetectLedge();
        SubStateMachine();
    }

    private void SubStateMachine()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool anyInputKeyPressed = horizontalInput != 0 || verticalInput != 0;

        if (holding)
        {
            FreezeRigidbodyOnLedge();

            timeOnLedge = Time.deltaTime;

            if(timeOnLedge > minTimeOnLedge && anyInputKeyPressed) exitLedge();
        }
    }

    private void holdLedge()
    {
        holding = true;

        pb.unlimited = true;
        pb.restricted = true;

        curLedge = ledgeHit.transform;
        prevLedge = ledgeHit.transform;

        rb.useGravity = false;
        //remove all momentem from rigidbody
        rb.velocity = Vector3.zero;
    }

    private void FreezeRigidbodyOnLedge()
    {
        rb.useGravity = false;

        Vector3 directionToLedge = curLedge.position - transform.position;
        float distanceToLedge = Vector3.Distance(transform.position, curLedge.position);
        //New Ledge
        if (distanceToLedge > 1f)
        {
            if(rb.velocity.magnitude <moveToLedgeSpeed)
             rb.AddForce(directionToLedge.normalized * moveToLedgeSpeed * 1000f * Time.deltaTime);
        }
        else
        {
            if (!pb.freeze) pb.freeze = true;
            if (pb.unlimited) pb.unlimited = false;
        }

        if (distanceToLedge > maxLedgeGrabDistance) exitLedge();
    }

    private void exitLedge()
    {
        holding = false;
        timeOnLedge = 0f;   

        pb.restricted = false;
        pb.freeze = false;
        rb.useGravity = true;

        StopAllCoroutines();
        Invoke(nameof(resetprevLedge), 1f);

    }

    private void resetprevLedge()
    {
        prevLedge = null;
    }
}
