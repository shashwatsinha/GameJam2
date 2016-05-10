using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent (typeof(NetworkIdentity))]
[RequireComponent(typeof(Rigidbody2D))]


public class PlayerMovement : NetworkBehaviour
{
    private NetworkIdentity identity;
    private Rigidbody2D rigidBody;
    private Image healthBar;
    private Image WeaponBar;
    private int maxHealth = 100;

    public GameObject groundCheck;
    public bool grounded;
    public int weaponType;
    bool allowFire;
    public float speed = 0.5f;
    public Transform missileSpawnPoint;
    public Transform missileSpawnPoint1;
    public Transform missileSpawnPoint2;
    public Transform bulletCapSpawnPoint;
    public float missileSpeed = 50;
    public GameObject missilePrefab;
    public GameObject rocketPrefab;                     // Prefab of the rocket.
    public float rocketSpeed = 20f;                      // The speed the rocket will fire at.
    public GameObject healEffect;
    float missileLifeTime = 1.5f;
    float capsTime = 5f;
    private Vector3 shootPos;
    public static float posX;
    public static float posY;
    public static int directionFacing=2;
    private int directionFacingBefore = 0;
    public bool playerOrientation;
    private RaycastHit2D hit;
    private Transform platformCollisionIgnored;
    public GameObject BulletSound;
    public GameObject JumpSound;
    public float ammo;
    private Animator anim;
    public GameObject bulletCaps;

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
        shootPos = new Vector3(1,0,0);
        ammo = 10.0f;
        anim = GetComponent<Animator>();
        healthBar = transform.FindChild("Canvas").FindChild("Health").GetComponent<Image>();
        WeaponBar = transform.FindChild("Canvas").FindChild("Weapon").GetComponent<Image>();
        WeaponBar.fillAmount = 0;
    }

    void Update()
    {
        
        if (!identity.isLocalPlayer)
        {
            return;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
          //  if (identity.isClient || identity.isLocalPlayer)
            {
                transform.position = new Vector3(transform.position.x + (5f * Time.deltaTime), transform.position.y, transform.position.z);
                playerOrientation = true;
                Orientation(playerOrientation);
                CmdOrientation(playerOrientation);
                directionFacing = 0;
                directionFacingBefore = directionFacing;
                Direction(0);
                CmdDirection(0);
                anim.SetFloat("Speed", 1.0f);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
           //if (identity.isServer || identity.isClient || identity.isLocalPlayer)
            {

                transform.position = new Vector3(transform.position.x - (5f * Time.deltaTime), transform.position.y, transform.position.z);
                playerOrientation = false;
                Orientation(playerOrientation);
                CmdOrientation(playerOrientation);
                directionFacing = 1;
                directionFacingBefore = directionFacing;
                Direction(1);
                CmdDirection(1);
                anim.SetFloat("Speed", 1.0f);
            }
        }
        else
        {
            directionFacing = 2;
            anim.SetFloat("Speed", 0.0f);
            if (directionFacingBefore == 0)
            {
                Orientation(true);
                CmdOrientation(true);
            }
            else if (directionFacingBefore == 1)
            {
                Orientation(false);
                CmdOrientation(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && grounded == true)
        {
            anim.SetTrigger("Jump");
            if (platformCollisionIgnored != null)
                Physics2D.IgnoreCollision(platformCollisionIgnored.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * 4200.0f);
            GameObject jumpSound = Instantiate(JumpSound, this.transform.position, this.transform.rotation) as GameObject;
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            GameObject bulletSound = Instantiate(BulletSound, this.transform.position, this.transform.rotation) as GameObject;

            CmdDoFire(missileLifeTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (platformCollisionIgnored != null)
                Physics2D.IgnoreCollision(platformCollisionIgnored.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);

            Vector3 pos = new Vector3(groundCheck.transform.position.x, groundCheck.transform.position.y, 0);
            hit = Physics2D.Linecast(transform.position, pos, 1 << LayerMask.NameToLayer("Ground"));
            if (hit)
            {
                platformCollisionIgnored = hit.transform;
                Physics2D.IgnoreCollision(hit.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }

        posX = transform.position.x;
        posY = transform.position.y;
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
            shootPos = new Vector3(1, 0, 0);

        if (direction == 1)
            shootPos = new Vector3(-1, 0, 0);
    }

    void Direction(float direction)
    {
        if (direction == 0)
            shootPos = new Vector3(1, 0, 0);

        if (direction == 1)
            shootPos = new Vector3(-1, 0, 0);
    }


    [Command]
    void CmdOrientation(bool orientation)
    {
        if (orientation == true)
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (orientation == false)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void Orientation(bool orientation)
    {
        if (orientation == true)
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (orientation == false)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
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
            healthBar.fillAmount = health / maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isServer)
            return;

        health -= damage;
        healthBar.fillAmount = health / maxHealth;

        if (health<=0)
        {
            health = 0;
            NetworkServer.Destroy(gameObject);
        }
    }
   
    [Command]
    public void CmdDoFire(float lifeTime)
    {
        GameObject caps;
        caps = (GameObject)Instantiate(bulletCaps, bulletCapSpawnPoint.position, Quaternion.identity);
        Destroy(caps, capsTime);
        NetworkServer.Spawn(caps);
            
        if (weaponType == 0)
        {
            GameObject missile;
            missile = (GameObject)Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
            Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
            rigid.velocity = shootPos * missileSpeed;
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
                    Rigidbody2D rigid = missile[i].GetComponent<Rigidbody2D>();
                    rigid.velocity = shootPos * missileSpeed;
                    Destroy(missile[i], lifeTime);
                    NetworkServer.Spawn(missile[i]);
                }
                ammo = ammo - 1.0f;
            }
            else {
                weaponType = 0;
                GameObject missile;
                missile = (GameObject)Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
                Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
                rigid.velocity = shootPos * missileSpeed;
                Destroy(missile, lifeTime);
                NetworkServer.Spawn(missile);
            }
        }
        else if (weaponType == 2)
        {
            if (ammo > 0.0f)
            {
                CmdShootRocket(lifeTime);
                ammo = ammo - 1.0f;
            }
            else {
                weaponType = 0;
                GameObject missile;
                missile = (GameObject)Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
                Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
                rigid.velocity = shootPos * missileSpeed;
                Destroy(missile, lifeTime);
                NetworkServer.Spawn(missile);
            }
        }
    }

    [Command]
    void CmdShootRocket(float lifetime)
    {
        GameObject missile = null;
        if (shootPos.x > 0)
            missile = (GameObject)Instantiate(rocketPrefab, missileSpawnPoint.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        else if (shootPos.x < 0)
            missile = (GameObject)Instantiate(rocketPrefab, missileSpawnPoint.position, Quaternion.Euler(new Vector3(0, 0, 180f)));
        Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
        rigid.velocity = shootPos * rocketSpeed;
        Destroy(missile, lifetime);
        NetworkServer.Spawn(missile);
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
