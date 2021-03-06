﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RocketSelector : NetworkBehaviour
{
    public int weaponType;
    private float ammos=3.0f;
    // Use this for initialization
    void Start()
    {
        //weaponType = Random.Range(0, 2);
    }

    void SetAmmo(int level)
    {
        if(ammos<5)
            ammos = ammos+level ;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer)
            return;

        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.weaponType = 2;
            player.ammo = ammos;
            player.maxAmmo = ammos;
            WeaponPickup.weaponCount--;
            Destroy(gameObject);
        }
    }
}
