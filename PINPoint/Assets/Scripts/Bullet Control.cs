using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float expireTime;

    private void Start()
    {
        StartCoroutine(DespawnTime(expireTime));
    }

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

    IEnumerator DespawnTime(float timer)
    {
        yield return new WaitForSeconds(timer);

        //KYS
        Destroy(gameObject);
    }
}
