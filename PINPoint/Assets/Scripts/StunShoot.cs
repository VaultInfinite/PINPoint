using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunShoot : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float shootForce;
    [SerializeField]
    private GameObject bullet;


    private void Awake()
    {
        cam = Camera.main;
    }

    //Shoot Projectile - Stun
    public void ShootStun()
    {

        //Get firing direction
        Vector3 bulletDir = cam.transform.forward;

        //Create gameObject and store it
        GameObject currentBullet = Instantiate(bullet, cam.transform.position, Quaternion.identity);

        //Rotate bullet to desired direction
        currentBullet.transform.forward = cam.transform.forward;

        //Add force to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(bulletDir.normalized * shootForce, ForceMode.Impulse);
        
    }
}
