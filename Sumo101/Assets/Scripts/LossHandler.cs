using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LossHandler : NetworkBehaviour
{
    public GameObject deathFireworks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.y < 0)
        {
            Die();
        }
    }


    private void Die() 
    {
        GetComponent<NetworkObject>().Despawn();
        GameObject df = Instantiate(deathFireworks,this.transform.position,Quaternion.identity);
        df.GetComponent<NetworkObject>().Spawn();
    }

    /*
    [ServerRpc]
    private void RequestDeathServerRpc()
    {
        RequestDeathClientRpc();
    }

    [ClientRpc]
    private void RequestClientRpc()
    {
        //kill client
    }
    */
}
