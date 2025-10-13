using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Camera targetCamera;
    [SerializeField]
    private Transform targetCameraPos;

    public bool isTarget;

    private void Start()
    {
        GaMaControl.Instance.npcs.Add(this);
        Material material = gameObject.GetComponent<MeshRenderer>().material;
        material.color = Random.ColorHSV();
    }

    private void Update()
    {
        if (isTarget)
        {
            targetCamera.transform.position = targetCameraPos.position;
            targetCamera.transform.LookAt(gameObject.transform.position);
        }
    }
}
