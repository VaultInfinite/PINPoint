using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private Transform targetCameraPos;
    public Camera targetCamera;
    private Rigidbody rb;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float minPauseDuration, maxPauseDuration;
    private Vector3 targetLocation;
    [HideInInspector]
    public CrowdSpawner crowdRegion;

    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;

    public Animator npcAnim;

    public bool isTarget;
    [SerializeField]
    private bool reachedGoal;

    private void Start()
    {
        GaMaControl.Instance.npcs.Add(this);

        targetLocation = RandomPointInRegion();

        Material material = meshRenderer.material;
        material.color = Random.ColorHSV();

        rb = GetComponent<Rigidbody>();

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetLocation, 0.1f);
    }

    private void Update()
    {
        if (isTarget)
        {
            targetCamera.transform.position = targetCameraPos.position;
            targetCamera.transform.LookAt(gameObject.transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(targetLocation, transform.position) < 0.1f && !reachedGoal)
        {
            StartCoroutine(NPCIdle());
        }
        else if (!reachedGoal)
        {
            rb.velocity = (targetLocation - transform.position).normalized * speed;
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

    private IEnumerator NPCIdle()
    {
        reachedGoal = true;
        yield return new WaitForSeconds(Random.Range(minPauseDuration,maxPauseDuration));
        targetLocation = RandomPointInRegion();
        reachedGoal = false;
    }
}
