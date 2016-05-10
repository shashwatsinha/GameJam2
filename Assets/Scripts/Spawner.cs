using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Spawner : NetworkBehaviour
{
    public float spawnTime = 5f;        // The amount of time between each spawn.
    public float spawnDelay = 5f;       // The amount of time before spawning starts.
    public GameObject[] enemies;        // Array of enemy prefabs.
    public static int enemyCount;

    public int enemylevel = 1;
    public GameObject enemyPrefab;
    // public float respawntime;

    public GameObject otherEndEdge;
    public bool isParentonleft;


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
        // Instantiate a random enemy.
        if (transform.childCount < 1)
        {
            //int enemyIndex = Random.Range(0, enemies.Length);
            //GameObject enemyObj = (GameObject)Instantiate(enemies[enemyIndex], transform.position, transform.rotation);
            float xposition = transform.position.x + 1.0f;
            GameObject enemyObj;

            if (isParentonleft)
            {
                enemyObj = (GameObject)Instantiate(enemyPrefab, new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z), transform.rotation);
                enemyObj.SendMessage("SetLeftMovementRange", this.gameObject);
                enemyObj.SendMessage("SetRightMovementRange", otherEndEdge);
            }
            else
            {
                enemyObj = (GameObject)Instantiate(enemyPrefab, new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, gameObject.transform.position.z), transform.rotation);
                enemyObj.SendMessage("SetLeftMovementRange", otherEndEdge);
                enemyObj.SendMessage("SetRightMovementRange", this.gameObject);
            }
            enemyObj.transform.parent = this.gameObject.transform;
            enemyObj.SendMessage("SetHitpoints", enemylevel);
            NetworkServer.Spawn(enemyObj);
            enemylevel++;
            //enemyCount++;
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log(collider.gameObject.name);
        //if (collider.gameObject.tag == "hero")
        //{
        //    Physics.IgnoreCollision(collider.GetComponent<Collider>(), GetComponent<Collider>());
        //}

        collider.gameObject.SendMessage("CmdFlip");

    }

}
