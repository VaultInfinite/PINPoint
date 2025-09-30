using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public Transform spawn;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        enemy.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y + 2, spawn.transform.position.z);
    }

    public void SpawnPoint(Transform enemySpawn)
    {
        spawn = enemySpawn.transform;
    }
}
