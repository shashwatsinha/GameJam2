﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RocketSelector : NetworkBehaviour
{
    public int weaponType;
    // Use this for initialization
    void Start()
    {
        //weaponType = Random.Range(0, 2);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer)
            return;

        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.weaponType = 2;
            player.ammo = 5.0f;
            WeaponPickup.weaponCount--;
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Missile")
        {
            Destroy(collision.gameObject);
            //Destroy(this.gameObject);
        }
    }
}
