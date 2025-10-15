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
    
    void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":

                Debug.Log("KILL");
                collision.transform.gameObject.GetComponent<StunControl>().Stunned();
                Destroy(gameObject);
                break;

            case "Ground":
                Debug.Log("miss!");
                Destroy(gameObject);
                break;
        }
    }
    

    IEnumerator DespawnTime(float timer)
    {
        yield return new WaitForSeconds(timer);

        //KYS
        Destroy(gameObject);
    }
}
