using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    public LevelManager.Difficulty difficulty;
    public GameObject npcs;

    [SerializeField]
    private int minNpcs, maxNpcs;
    private float Radius => Mathf.Min(size.x, size.y) / 4.0f;

    public Vector2 size;
    /// <summary>
    /// Gizmos to visualize crowd spawning region
    /// </summary>
    private void OnDrawGizmos()
    {
        //The navmesh region for the NPCs to wander
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(size.x, 0, size.y));

        //The region in which NPCS will be spawned
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Vector3.zero, Radius);
    }

    public void Spawn()
    {
        int spawnCount = Random.Range(minNpcs, maxNpcs + 1);

        for (int index = 0; index < spawnCount; index++)
        {
            Vector3 spawnPoint = RandomSpawnPoint(Radius);

            GameObject guy = Instantiate(npcs, spawnPoint + Vector3.up, Quaternion.identity);
            NPC npc = guy.GetComponent<NPC>();
            npc.crowdRegion = this;
        }
    }

    public Vector3 RandomSpawnPoint(float radius)
    {
        float d, x, z;
        do
        {
            x = Random.Range(-1.0f, 1.0f);
            z = Random.Range(-1.0f, 1.0f);
            d = x * x + z * z;
        } while (d > 1.0f);

        var position = new Vector3(x, 0.0f, z);
        position = position.normalized;
        position *= radius;
        position += transform.position;
        return position;
    }
}
