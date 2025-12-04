using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float speed, panicBoost;
    [SerializeField]
    private float minPauseDuration, maxPauseDuration;
    private Vector3 targetLocation;
    [HideInInspector]
    public CrowdSpawner crowdRegion;

    public SkinnedMeshRenderer meshRenderer;

    public Animator npcAnim;

    public bool isTarget;
    [SerializeField]
    private bool reachedGoal;

    [SerializeField]
    private float playerDetectionRange;
    private bool playerDetected;

    [SerializeField]
    private LayerMask playerLayer;

    private void Start()
    {
        targetLocation = RandomPointInRegion();

        rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetLocation, 0.1f);
        //Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
    }

    private void Update()
    {
        playerDetected = Physics.CheckSphere(transform.position, playerDetectionRange, playerLayer);
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(targetLocation, transform.position) < 0.1f && !reachedGoal && !playerDetected)
        {
            StartCoroutine(NPCIdle());
            npcAnim.SetTrigger("Idle");
            npcAnim.ResetTrigger("Walk");
        }
        if (Vector3.Distance(targetLocation, transform.position) < 0.1f && !reachedGoal && playerDetected)
        {
            StartCoroutine(NPCPanic());
            npcAnim.SetTrigger("Idle");
            npcAnim.ResetTrigger("Walk");
        }
        else if (!reachedGoal)
        {
            rb.velocity = (targetLocation - transform.position).normalized * speed;
            transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
            npcAnim.SetTrigger("Walk");
            npcAnim.ResetTrigger("Idle");
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public Vector3 RandomPointInRegion()
    {
        var x = Random.Range(-crowdRegion.size.x / 2 + 0.5f, crowdRegion.size.x / 2 - 0.5f);
        var z = Random.Range(-crowdRegion.size.y / 2 + 0.5f, crowdRegion.size.y / 2 - 0.5f);

        Vector3 position = new(x, 1f, z);
        position = crowdRegion.transform.localToWorldMatrix.MultiplyPoint(position);
        return position;
    }

    private Vector3 RandomPointInRegionPanic()
    {
        var x = Random.Range(-crowdRegion.size.x / 2 + 0.5f, crowdRegion.size.x / 2 - 0.5f);
        var z = Random.Range(-crowdRegion.size.y / 2 + 0.5f, crowdRegion.size.y / 2 - 0.5f);

        Vector3 position = new(x, 1f, z);
        position = crowdRegion.transform.localToWorldMatrix.MultiplyPoint(position);
        return position;
    }

    private IEnumerator NPCIdle()
    {
        reachedGoal = true;
        yield return new WaitForSeconds(Random.Range(minPauseDuration,maxPauseDuration));
        targetLocation = RandomPointInRegion();
        reachedGoal = false;
    }

    private IEnumerator NPCPanic()
    {
        speed = speed * panicBoost;
        reachedGoal = true;
        targetLocation = RandomPointInRegionPanic();
        yield return new WaitForSeconds(0.4f);
        reachedGoal = false;
        speed = speed / panicBoost;
    }
}
