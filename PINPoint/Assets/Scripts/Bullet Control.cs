using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float expireTime;
    public float rad;

    private void Start()
    {
        StartCoroutine(DespawnTime(expireTime));
    }

    private void OnDrawGizmos()
    {
        //Debug
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, rad);
    }

    private void Update()
    {

        RaycastHit hit;

        if (Physics.SphereCast(transform.position, rad, transform.forward, out hit, 0.2f))
        {
            Debug.Log("I hit...");
            switch (hit.transform.gameObject.tag)
            {
                case "Target":
                    Debug.Log("the Target!");
                    Destroy(gameObject);
                    break;

                case "Ground":
                    Debug.Log("the ground...");
                    Destroy(gameObject);
                    break;

                case "Enemy":

                    //Stun Enemy
                    Debug.Log("AN ENEMY!");
                    Destroy(gameObject);
                    gameObject.SetActive(false);
                    hit.transform.gameObject.transform.GetComponent<StunControl>().Stunned();
                    
                    break;

                default:
                    //Do Nothing
                    break;
            }
        }
    }
  

    /*
    void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Target":

                Debug.Log("KILL");
                Destroy(gameObject);
                break;

            case "Ground":
                Debug.Log("miss!");
                Destroy(gameObject);
                break;
        }
    }
    */

    IEnumerator DespawnTime(float timer)
    {
        yield return new WaitForSeconds(timer);

        //KYS
        Destroy(gameObject);
    }
}
