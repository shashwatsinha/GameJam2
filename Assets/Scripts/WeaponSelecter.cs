﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WeaponSelecter : NetworkBehaviour
{
    public int weaponType;
    private int ammos = 10;
    // Use this for initialization
    void Start()
    {
        //weaponType = Random.Range(0, 2);
    }

    void SetAmmo(int level)
    {
        ammos = ammos * (level);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer)
            return;

        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.weaponType = 1;
            player.ammo = ammos;
            WeaponPickup.weaponCount--;
            Destroy(gameObject);
        }
    }
}
