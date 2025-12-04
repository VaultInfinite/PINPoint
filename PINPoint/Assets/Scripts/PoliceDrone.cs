using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PoliceDrone : MonoBehaviour
{
    private Rigidbody rb;
    public Transform player;

    public LayerMask whatIsPlayer;

    public Transform[] navPoints;
    private int index;

    //Wandering
    public float speed;
    private Vector3 walkPoint;
    [SerializeField]
    private float minPauseDuration, maxPauseDuration;
    private bool reachedGoal;
    private bool playerApproach;

    //Agressive
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private float despawnTime;

    //StateSwitch
    public float sightRange, attackRange, tooCloseRange;
    public bool playerInSightRange, playerInAttackRange, playerTooClose;

    [HideInInspector]
    public StunControl stunControl;

    private void Awake()
    {
        stunControl = GetComponent<StunControl>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        walkPoint = NextPointInArray();
    }

    private void Update()
    {
        //check for if player is in range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerTooClose = Physics.CheckSphere(transform.position, tooCloseRange, whatIsPlayer);

        if (!stunControl.isStunned)
        {
            if (!playerInSightRange && !playerInAttackRange) Wandering();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer(true);
            if (playerInAttackRange && playerInSightRange && playerTooClose) AttackPlayer(false);
        }
    }

    private void FixedUpdate()
    {
        if (!stunControl.isStunned)
        {
            Movement();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void Wandering()
    {
        //if (!walkPointSet) SearchWalkPoint();
        //Vector3 distanceToWalkPoint = transform.position - walkPoint;

        Debug.Log("Wandering");



        //Walkpoint reached
        //if (distanceToWalkPoint.magnitude < 1f)
        //    walkPointSet = false;
    }

    private void ChasePlayer()
    {
        LookAt();

        //agent.SetDestination(player.position);
        walkPoint = new Vector3(player.transform.position.x, player.transform.position.y + 10f, player.transform.position.z);

        
    }

    private void AttackPlayer(bool approach)
    {
        LookAt();

        playerApproach = approach;

        //agent.SetDestination(transform.position);
        walkPoint = new Vector3(player.transform.position.x, player.transform.position.y + 10f, player.transform.position.z);



        if (!alreadyAttacked)
        {
            //Attack Code
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.LookRotation(player.transform.forward, Vector3.up));
            bullet.GetComponent<BulletControl>().endPoint = player.transform.position;
            //StartCoroutine(DespawnBullet(bullet));

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    //private void SearchWalkPoint()
    //{
    //    float randomZ = Random.Range(-walkPointRange, walkPointRange);
    //    float randomX = Random.Range(-walkPointRange, walkPointRange);

    //    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

    //    //check if this point is on the map
    //    if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
    //        walkPointSet = true;

        
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    /// <summary>
    /// Visual representation of the in editor navigation points
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Transform t in navPoints)
        {
            Gizmos.DrawWireSphere(t.position, 2f);
        }

        Gizmos.color = Color.white;
        for (int i = 0; i < navPoints.Length; i++)
        {
            Transform a, b;
            a = navPoints[i];
            b = navPoints[(i + 1) % navPoints.Length];

            Gizmos.DrawLine(a.position, b.position);
        }
    }

    private IEnumerator DespawnBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(despawnTime);
        bullet.SetActive(false);
    }

    private void Movement()
    {
        if (Vector3.Distance(walkPoint, transform.position) < 0.1f && !reachedGoal)
        {
            StartCoroutine(DroneIdle());
        }
        else if (!reachedGoal)
        {
            rb.velocity = (walkPoint - transform.position).normalized * speed;
            transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        }
        else if (playerApproach || reachedGoal)
        {
            rb.velocity = Vector3.zero;
        }
    }

    public Vector3 NextPointInArray()
    {
        Vector3 position = navPoints[index].transform.position;
        index = (index + 1) % navPoints.Length;

        return position;
    }

    public void LookAt()
    {
        Vector3 lookDirection = (gameObject.transform.position - player.transform.position).normalized;
        lookDirection.y = 0f;
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    private IEnumerator DroneIdle()
    {
        reachedGoal = true;
        yield return new WaitForSeconds(Random.Range(minPauseDuration, maxPauseDuration));
        walkPoint = NextPointInArray();
        reachedGoal = false;
    }
}
