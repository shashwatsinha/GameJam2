using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]

public class Rocket : NetworkBehaviour
{

    public float damage = 100.0f;
    public GameObject ground;
    public int id;
    public GameObject explosion;
    public GameObject rocketExplode;
    void Start()
    {
        ground = GameObject.FindGameObjectWithTag("Ground");
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ground.GetComponent<Collider2D>());
        //  Debug.Log(id);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isServer)
            return;

        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.TakeDamage(damage);
            CmdOnExplode();
            //NetworkServer.Destroy(gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Ground")
        {
            CmdOnExplode();
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Wall")
        {
            CmdOnExplode();
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Missile")
        {
            Destroy(collision.gameObject);
            //Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.CmdDeath();
            CmdOnExplode();
            //enemy.Hurt();
            Destroy(gameObject);
            // NetworkServer.Destroy(gameObject);
        }


    }

    void CmdOnExplode()
    {
        // Create a quaternion with a random rotation in the z-axis.
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        // Instantiate the explosion where the rocket is with the random rotation.
        Instantiate(explosion, transform.position, randomRotation);
        Instantiate(rocketExplode, transform.position, transform.rotation);
    }
}
