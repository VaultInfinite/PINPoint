using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the player shoot while aiming
/// </summary>
public class Shoot : MonoBehaviour
{
    //Variables
    public Transform FirePos;
    public Camera cam;
    private bool canShoot = true;
    public float shootCooldown;

    [Header("Bullet")]
    public GameObject bulletOBJ;
    public float bulletSpeed;
    public float dieTime;

    Transform attackPoint;
    Vector3 targetPoint;

    //Shoot the bullet
    public void Shooting()
    {
        //Check if can shoot
        if (!canShoot) return;

        //Set Shoot to false
        canShoot = false;

        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 fireDir = targetPoint - FirePos.position;


        GameObject newBullet = Instantiate(bulletOBJ, FirePos.position, Quaternion.identity);

        newBullet.transform.forward = FirePos.forward;

        newBullet.GetComponent<BulletControl>().expireTime = dieTime;

        newBullet.GetComponent<Rigidbody>().AddForce(fireDir.normalized * bulletSpeed, ForceMode.Impulse);

        //Call timer
        StartCoroutine(ShootingCooldown(shootCooldown));
    }

    IEnumerator ShootingCooldown(float timer)
    {
        yield return new WaitForSeconds(timer);

        canShoot = true;
    }
}
