using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float expireTime;
    public float gizmoRadius;

    public float playerStunDuration, droneStunDuration;

    [HideInInspector]
    public Vector3 startPoint, endPoint;
    private Vector3 direction;
    [SerializeField]
    private float speed;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPoint = transform.position;

        direction = ShootDirection();
        transform.rotation = Quaternion.LookRotation(direction);
        Destroy(gameObject, expireTime);
    }

    private void FixedUpdate()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnDrawGizmos()
    {
        //Debug
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }
    
    void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.layer)
        {
            case 9: //Assuming layer 9 is enemy

                Debug.Log("Stun drone");
                collision.gameObject.GetComponent<PoliceDrone>().stunControl.Stunned();
                Destroy(gameObject);
                break;

            case 7: //Assuming layer 7 is player
                Debug.Log("Stun Player");
                collision.gameObject.GetComponent<PlayerController>().stunControl.Stunned();
                Destroy(gameObject);
                break;
            case 3: //Assuming layer 3 is ground
                Debug.Log("miss!");
                Destroy(gameObject);
                break;
        }
    }

    private Vector3 ShootDirection()
    {
        direction = -(startPoint - endPoint).normalized;
        return direction;
    }
}
