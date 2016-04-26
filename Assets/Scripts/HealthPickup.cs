using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HealthPickup : NetworkBehaviour
{

    public float spawnTime = 5f;        // The amount of time between each spawn.
    public float spawnDelay = 5f;       // The amount of time before spawning starts.
    public static int healthPickupCount;
    public GameObject[] healthPickup;

    void Start()
    {
        // Start calling the Spawn function repeatedly after a delay .
        InvokeRepeating("CmdSpawn", spawnDelay, spawnTime);

    }

    [Command]
    void CmdSpawn()
    {
        if (!isServer)
            return;
        // Instantiate a health pickup.
        if (healthPickupCount < 4)
        {
            int index = Random.Range(0, healthPickup.Length);
            float[] randomY = new float[] { -2.9f, 0.39f, 3.6f };
            GameObject healthPickupObj = (GameObject)Instantiate(healthPickup[index], new Vector2(Random.Range(-35.0f, 30.0f), randomY[Random.Range(0, randomY.Length)]), Quaternion.identity);
            NetworkServer.Spawn(healthPickupObj);
            healthPickupCount++;
        }

    }
}


