using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent (typeof(NetworkIdentity))]
[RequireComponent(typeof(Rigidbody2D))]


public class PlayerMovement : NetworkBehaviour
{
    private NetworkIdentity identity;
    private Rigidbody2D rigidBody;

    public bool grounded;
    public int weaponType;
    bool allowFire;
    public float speed = 0.5f;
    public float weaponTimer;
    public Transform missileSpawnPoint;
    public Transform missileSpawnPoint1;
    public Transform missileSpawnPoint2;
    public float missileSpeed = 50;
    public GameObject missilePrefab;
    public GameObject healEffect;
    float missileLifeTime = 1.5f;
    private Vector3 shootPos;
    public static float posX;
    public bool playerOrientation;

    [SyncVar]
    private float health = 100;
    public float Health { get { return health; } }

	// Use this for initialization
	void Start ()
    {
        identity = GetComponent<NetworkIdentity>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerOrientation = true;
        grounded = true;
        weaponTimer = 10.0f;
        shootPos = new Vector3(1, 0, 0);
	}

    void Update()
    {
        if (!identity.isLocalPlayer)
        {
            return;
        }

        CmdTimer();

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + (5f * Time.deltaTime),transform.position.y,transform.position.z);
            playerOrientation = true;
            CmdDirection(0);
        }

        else if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - (5f * Time.deltaTime),transform.position.y,transform.position.z);
            playerOrientation = false;
            CmdDirection(1);
        }
       
        //Debug.Log(weaponTimer);
       
        if (Input.GetKeyDown(KeyCode.W) && grounded == true)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * 4200.0f);
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdDoFire(missileLifeTime);
        }

        posX = transform.position.x;

        if (playerOrientation == true)
        {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }

        else if (playerOrientation == false)
        {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            grounded = true;
            Debug.Log("Grounded = "+grounded);
        }

    }

    [Command]
    void CmdDirection(float direction)
    {
        if (direction == 0)
            shootPos = new Vector3(1, 0, 0);

        if (direction == 1)
            shootPos = new Vector3(-1, 0, 0);
    }

    [Command]
    void CmdTimer()
    {
        if (weaponTimer <= 0.0f)
        {
            weaponTimer = 0.0f;
        }
        else if (weaponTimer < 10.0f && weaponTimer > 0.0f)
        {
            weaponTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "HealthBox")
        {
            Destroy(collider.gameObject);
            Heal();

            GameObject heal;
            heal = (GameObject)Instantiate(healEffect, transform.position, Quaternion.identity);
            heal.transform.parent = transform;

            Destroy(heal, 1);
        }
    }

    public void Heal()
    {
        if (!isServer)
            return;

        health = health + 5;
    }

    public void TakeDamage(float damage)
    {
        if (!isServer)
            return;

        health -= damage;

        if(health<=0)
        {
            health = 0;
            NetworkServer.Destroy(gameObject);
        }
    }
   
    [Command]
    public void CmdDoFire(float lifeTime)
    {
        if (weaponType == 0)
        {
            GameObject missile;
            missile = (GameObject)Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
            Missile m = missile.GetComponent<Missile>();
            Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
            rigid.velocity = shootPos * missileSpeed;
            Destroy(missile, lifeTime);
            NetworkServer.Spawn(missile);
        }
        else if(weaponType == 1)
        {
            if (weaponTimer > 0.0f)
            {
                GameObject[] missile = new GameObject[3];
                for (int i = 0; i < 3; i++)
                {
                    if (i == 0)
                        missile[i] = (GameObject)Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
                    if (i == 1)
                        missile[i] = (GameObject)Instantiate(missilePrefab, missileSpawnPoint1.position, Quaternion.identity);
                    if (i == 2)
                        missile[i] = (GameObject)Instantiate(missilePrefab, missileSpawnPoint2.position, Quaternion.identity);
                    Missile m = missile[i].GetComponent<Missile>();
                    Rigidbody2D rigid = missile[i].GetComponent<Rigidbody2D>();
                    rigid.velocity = shootPos * missileSpeed;
                    Destroy(missile[i], lifeTime);
                    NetworkServer.Spawn(missile[i]);
                }
            }
            else
                weaponType = 0;
        }
    }
    
    void OnGUI()
    {
        if (!identity.isLocalPlayer)
        {
            return;
        }

        GUI.Label(new Rect(100, 100, 100, 100), health.ToString());
    }
}
