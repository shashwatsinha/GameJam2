using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Missile : NetworkBehaviour {

    public float damage = 15.0f;
    public GameObject ground;
    public int id;
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

        if(collision.gameObject.tag=="Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.TakeDamage(damage);
            //NetworkServer.Destroy(gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Ground")
        {
           
        //    Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Wall")
        {

            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Missile")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Hurt();
            Destroy(gameObject);
           // NetworkServer.Destroy(gameObject);
        }


    }
}
