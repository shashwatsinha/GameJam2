using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Gift : NetworkBehaviour {

    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 0.0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isServer)
            return;

        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.Heal();
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.GainHealth();
            Destroy(gameObject);
        }
    }
}
