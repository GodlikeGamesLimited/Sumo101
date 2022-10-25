using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;


public class PowerUpSpawner : NetworkBehaviour
{
    public GameObject powerUp;
    private float time = (float)0, period = (float)2;
    public int powerUpCount = 0, maxPowerUps = 4;

    // Update is called once per frame
    void Update()
    {
        if(!IsServer && !IsHost) {return;}    

        time += Time.deltaTime;
        
        if(time >= period && powerUpCount < maxPowerUps)
        {
            Debug.Log("spawning power up with count: " + powerUpCount.ToString() + " and max: " + maxPowerUps.ToString());
            System.Random r = new System.Random();

            float rx = 60 * (float)r.NextDouble() - 30;
            float rz = 60 * (float)r.NextDouble() - 30;

            
            time = (float)0;
            GameObject toSpawn = Instantiate(powerUp,new Vector3(rx,5,rz),Quaternion.identity);
            toSpawn.GetComponent<NetworkObject>().Spawn();

            powerUpCount += 1;
        }
    }
}