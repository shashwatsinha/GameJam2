using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Missile : NetworkBehaviour {

    public float damage = 15.0f;
    public GameObject ground;
    void Start()
    {
        ground = GameObject.FindGameObjectWithTag("Ground");
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), ground.GetComponent<BoxCollider2D>());
    }


	void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isServer)
            return;

        if(collision.gameObject.tag=="Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.TakeDamage(damage);
            NetworkServer.Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy player = collision.gameObject.GetComponent<Enemy>();
            player.Hurt();
            Debug.Log("Hurt");
            NetworkServer.Destroy(gameObject);
        }


    }
}
