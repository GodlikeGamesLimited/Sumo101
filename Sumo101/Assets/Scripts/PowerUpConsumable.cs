using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PowerUpConsumable : NetworkBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if(!IsServer && !IsHost) {return;}

        //add actual power upping logic
        GameObject player = c.gameObject;
        player.GetComponent<PlayerMovement>().speedPowerUp();
        
        this.GetComponent<NetworkObject>().Despawn();

        FindObjectOfType<PowerUpSpawner>().powerUpCount -= 1;
    }
}