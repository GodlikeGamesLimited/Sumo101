using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement2 : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) {Destroy(this);}
    }

    // Update is called once per frame
    void Update()
    {
        bool i = Input.GetKey("i");
        bool j = Input.GetKey("j");
        bool k = Input.GetKey("k");
        bool l = Input.GetKey("l");

        Vector3 currV = this.GetComponent<Rigidbody>().velocity;
        int vCap = 8;
            
            if(i) 
            {
                if(currV.z < vCap)
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,(float)0.1), ForceMode.VelocityChange);
            }

            if(k)
            {
                if(currV.z > -vCap)
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,(float)(-0.1)), ForceMode.VelocityChange);
            }

            if(l) 
            {
                if(currV.x < vCap)
                    this.GetComponent<Rigidbody>().AddForce(new Vector3((float)0.1,0,0), ForceMode.VelocityChange);
            }

            if(j) 
            {
                if(currV.x > -vCap)
                    this.GetComponent<Rigidbody>().AddForce(new Vector3((float)(-0.1),0,0), ForceMode.VelocityChange);
            }
    }
}
