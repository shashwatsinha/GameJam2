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
    public float fallTimer;
    //private int id;
    
    public float speed = 0.5f;

    

    public Transform missileSpawnPoint;
    public float missileSpeed = 50;
    public GameObject missilePrefab;
    float missileLifeTime = 1.5f;
    public float jumpTimer;
    
  
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
        
	}

    void Update()
    {
        if (!identity.isLocalPlayer)
        {
            return;
        }
       // Debug.Log(identity.playerControllerId);
        
        

        if (jumpTimer <= 0.0f)
        {
            jumpTimer = 0.0f;
        }
        if (jumpTimer > 0.0f)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W) && jumpTimer == 0.0f)
        {

            GetComponent<Rigidbody2D>().AddForce(Vector3.up * 5000.0f);
            jumpTimer = 1.0f;

        }

     /*   else if (Input.GetKeyDown(KeyCode.S) && fallTimer == 0.0f)
        {

            if (ButtonCooler > 0 && ButtonCount == 1 )
            {

                // GetComponent<BoxCollider2D>().enabled = false;
                RpcDisableCollider();
                fallTimer = 0.6f;
            }
            else
            {
                ButtonCooler = 0.5f;
                ButtonCount += 1;
            }
        }
    */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdDoFire(missileLifeTime);
        }

        posX = transform.position.x;
        if (Input.GetKey(KeyCode.D))
        {
            // transform.Translate(Vector3.right * speed);
            // GetComponent<Rigidbody2D>().AddForce(Vector3.right * speed);
            transform.position = new Vector3(
          transform.position.x + (5f * Time.deltaTime),
          transform.position.y,
          transform.position.z
      );

            playerOrientation = true;
        }

        else if (Input.GetKey(KeyCode.A))
        {
            //transform.Translate(Vector3.right * speed);
            // GetComponent<Rigidbody2D>().AddForce(Vector3.right * -speed);
            transform.position = new Vector3(
           transform.position.x - (5f * Time.deltaTime),
           transform.position.y,
           transform.position.z);
             playerOrientation = false;
        }





        if (playerOrientation == true)
        {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }

        else if (playerOrientation == false)
        {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
        }
    }
	
	


   

    [ClientRpc]
    void RpcDamage(float amount)
    {
     //   Debug.Log("Took damage:" + amount);

        //update local GUI here
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

        RpcDamage(damage);
    }

    [Command]
    public void CmdDoFire(float lifeTime)
    {
       
        GameObject missile;
        missile = (GameObject)Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
        Missile m = missile.GetComponent<Missile>();
        m.id = identity.netId.GetHashCode();
        Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();      
        rigid.velocity = transform.right * missileSpeed;
        
        Destroy(missile, lifeTime);       
        NetworkServer.SpawnWithClientAuthority(missile, connectionToClient);
    }
    
  

    

    }
