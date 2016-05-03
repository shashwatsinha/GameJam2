using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WeaponSelecter : NetworkBehaviour
{
    public int weaponType;
    // Use this for initialization
    void Start()
    {
        weaponType = Random.Range(0, 2);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer)
            return;

        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.weaponType = 1;
            player.ammo = 10.0f;
            WeaponPickup.weaponCount--;
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Missile")
        {
            Destroy(collision.gameObject);
        }
    }
}
