using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent (typeof(NetworkIdentity))]
[RequireComponent(typeof(Rigidbody2D))]


public class PlayerMovement : NetworkBehaviour
{
    private NetworkIdentity identity;
    private Rigidbody2D rigidBody;
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public GameObject groundCheck;
    public bool grounded;
    public int weaponType;
    bool allowFire;
    public float speed = 0.5f;
    public Transform missileSpawnPoint;
    public float jumpForce = 1000f;
    public Transform missileSpawnPoint1;
    public Transform missileSpawnPoint2;
    public float missileSpeed = 50;
    public GameObject missilePrefab;
    public GameObject healEffect;
    float missileLifeTime = 1.5f;
    private float shootPos;
    public static float posX;
    public static float posY;
    public static int directionFacing=2;
    public bool playerOrientation;
    private int directionFacingBefore = 0;
    private RaycastHit2D hit;
    public bool jump = false;
    private Transform platformCollisionIgnored;
    public GameObject BulletSound;
    public GameObject JumpSound;
    public float ammo;
    private Animator anim;
    public bool facingRight = true;

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
        shootPos = missileSpawnPoint.transform.position.x;
        ammo = 10.0f;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!identity.isLocalPlayer)
        {
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + (5f * Time.deltaTime), transform.position.y, transform.position.z);
            playerOrientation = true;
            directionFacing = 0;
            directionFacingBefore = directionFacing;
            CmdDirection(0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - (5f * Time.deltaTime), transform.position.y, transform.position.z);
            playerOrientation = false;
            directionFacing = 1;
            directionFacingBefore = directionFacing;
            CmdDirection(1);
        }
        else
        {
            directionFacing = 2;
        }

        //Debug.Log(weaponTimer);

        if (Input.GetKeyDown(KeyCode.W) && grounded == true)
        {
            if (platformCollisionIgnored != null)
                Physics2D.IgnoreCollision(platformCollisionIgnored.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * 4200.0f);
            GameObject jumpSound = Instantiate(JumpSound, this.transform.position, this.transform.rotation) as GameObject;
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            GameObject bulletSound = Instantiate(BulletSound, this.transform.position, this.transform.rotation) as GameObject;

            CmdDoFire(missileLifeTime,playerOrientation);
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (platformCollisionIgnored != null)
                Physics2D.IgnoreCollision(platformCollisionIgnored.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);

            Vector3 pos = new Vector3(0, 0, 0);
            if (directionFacingBefore == 0)
                pos = new Vector3(groundCheck.transform.position.x, groundCheck.transform.position.y, 0);
            else if (directionFacingBefore == 1)
                pos = new Vector3(groundCheck.transform.position.x, -groundCheck.transform.position.y, 0);
            hit = Physics2D.Linecast(transform.position, pos, 1 << LayerMask.NameToLayer("Ground"));
            if (hit)
            {
                platformCollisionIgnored = hit.transform;
                Physics2D.IgnoreCollision(hit.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }

        posX = transform.position.x;
        posY = transform.position.y;

        if (playerOrientation == true)
        {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }

        else if (playerOrientation == false)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }


    void FixedUpdate()
    {
        if (!identity.isLocalPlayer)
        {
            return;
        }
        float h = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(h));
        //  CmdMove();


    }
    [Command]
    void CmdMove()
    {
        float h = Input.GetAxis("Horizontal");

      

        /*  if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
              // ... add a force to the player.
              GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

          // If the player's horizontal velocity is greater than the maxSpeed...
          if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
              // ... set the player's velocity to the maxSpeed in the x axis.
              GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

          // If the input is moving the player right and the player is facing left...
          if (h > 0 && !facingRight)
              // ... flip the player.
              CmdFlip();
          // Otherwise if the input is moving the player left and the player is facing right...
          else if (h < 0 && facingRight)
              // ... flip the player.
              CmdFlip();
              */
        // The Speed animator parameter is set to the absolute value of the horizontal input.
        anim.SetFloat("Speed", Mathf.Abs(h));

        if (jump)
        {
            // Set the Jump animator trigger parameter.
            anim.SetTrigger("Jump");

            // Add a vertical force to the player.
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * 4200.0f);

            // Make sure the player can't jump again until the jump conditions from Update are satisfied.
            jump = false;
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            grounded = true;
        }

    }

    [Command]
    void CmdDirection(float direction)
    {
        if (direction == 0)
            shootPos = 1;

       if (direction == 1)
          shootPos = -1;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "HealthBox")
        {
            GameObject heal;
            heal = (GameObject)Instantiate(healEffect, transform.position, Quaternion.identity);
            heal.transform.parent = transform;

            Destroy(heal, 1);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "HealthBox")
        {
            GameObject heal;
            heal = (GameObject)Instantiate(healEffect, transform.position, Quaternion.identity);
            heal.transform.parent = transform;

            Destroy(heal, 1);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "HealthBox")
        {
            Heal();
        }
    }

    public void Heal()
    {
        if (!isServer)
            return;
        if(health < 100)
        {
            health = health + 1;
            if (health > 100)
                health = 100;
        }
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
    public void CmdDoFire(float lifeTime,bool direction)
    {
        
            
        if (weaponType == 0)
        {
            GameObject missile;
            missile = (GameObject)Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
            //Missile m = missile.GetComponent<Missile>();
            Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
            if(direction==true)
                rigid.velocity = Vector2.right * missileSpeed;
            else
                rigid.velocity = Vector2.right * missileSpeed;
            Destroy(missile, lifeTime);
            NetworkServer.Spawn(missile);
        }
        else if(weaponType == 1)
        {
            if (ammo > 0.0f)
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
                    //Missile m = missile[i].GetComponent<Missile>();
                    Rigidbody2D rigid = missile[i].GetComponent<Rigidbody2D>();
                    if (direction == true)
                        rigid.velocity = Vector2.right * missileSpeed;
                    else
                        rigid.velocity = Vector2.right * -missileSpeed;
                    Destroy(missile[i], lifeTime);
                    NetworkServer.Spawn(missile[i]);
                }
                ammo = ammo - 1.0f;
            }
            else {
                weaponType = 0;
                GameObject missile;
                missile = (GameObject)Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
                //Missile m = missile.GetComponent<Missile>();
                Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
              //  if (direction == true)
              //      rigid.velocity = shootPos * missileSpeed;
              //  else
             //       rigid.velocity = shootPos * -missileSpeed;
                Destroy(missile, lifeTime);
                NetworkServer.Spawn(missile);
            }
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
    [Command]
    void CmdFlip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
