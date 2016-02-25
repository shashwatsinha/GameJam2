using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{
    public float spawnTime = 5f;        // The amount of time between each spawn.
    public float spawnDelay = 5f;       // The amount of time before spawning starts.
    public GameObject[] enemies;        // Array of enemy prefabs.
    

    void Start()
    {
        // Start calling the Spawn function repeatedly after a delay .
        CmdSpawn();
       
    }

    [Command]
    void CmdSpawn()
    {
        if (!isServer)
            return;
        // Instantiate a random enemy.
        int enemyIndex = Random.Range(0, enemies.Length);
        GameObject enemyObj = (GameObject) Instantiate(enemies[enemyIndex], transform.position, transform.rotation);
        NetworkServer.Spawn(enemyObj);
        InvokeRepeating("CmdSpawn", spawnDelay, spawnTime);
    }
}
