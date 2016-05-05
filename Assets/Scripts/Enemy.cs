using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour
{
    public float moveSpeed = 2f;        // The speed the enemy moves at.
    public int HP = 2;                  // How many times the enemy can be hit before it dies.
    public GameObject[] weapons;

    private bool dead = false;          // Whether or not the enemy is dead.

    void Start()
    {
        //GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f); //C#
    }
   
   
    void FixedUpdate()
    {
        if (!isServer)
            return;


        // Set the enemy's velocity to moveSpeed in the x direction.
        GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);

       

        // If the enemy has zero or fewer hit points and isn't dead yet...
        if (HP <= 0 && !dead)
            // ... call the death function.
            CmdDeath();
    }

    public void Hurt()
    {
        // Reduce the number of hit points by one.
        HP--;
    }

    public void GainHealth()
    {
        // Reduce the number of hit points by one.
        HP = HP + 3; 
        
    }
    [Command]
    void CmdDeath()
    {

        // Set dead to true.
        dead = true;
        Spawner.enemyCount--;

        int randomNum = Random.Range(0, 2);
        if (randomNum == 1)
        {
            int random = Random.Range(0, 3);
            GameObject weaponPickup = (GameObject)Instantiate(weapons[random], transform.position, transform.rotation);
            NetworkServer.Spawn(weaponPickup);
            
        }
        Destroy(gameObject);
    }

    [Command]
    public void CmdFlip()
    {
        // Multiply the x component of localScale by -1.
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            int k = Random.Range(0, 3);
            if (k == 2)
            {
                CmdFlip();
            }
        }

        if (collision.gameObject.tag == "Enemy")
        {
            CmdFlip();
        }

        if (collision.gameObject.tag == "Wall")
        {
            CmdFlip();
        }

        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.TakeDamage(15.0f);
            //NetworkServer.Destroy(gameObject);
            CmdFlip();
        }

    //    if (collision.gameObject.tag == "Missile")
    //    {
          //  Enemy player = collision.gameObject.GetComponent<Enemy>();
      //      Hurt();
            //  Debug.Log("Hurt");
        //    NetworkServer.Destroy(collision.gameObject);
    //    }

    }

    }
