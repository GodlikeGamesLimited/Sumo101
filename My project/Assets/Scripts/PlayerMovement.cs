using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public GameObject floor;
    private bool canJump;

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
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        float j = Input.GetAxisRaw("Jump");

        Vector3 currV = this.GetComponent<Rigidbody>().velocity;
        int vCap = 8;

        //add vertical axis velocity unless vertical axis velocity is maxed. Note: vertical velocity is NOT Y-axis/jump
        if(v != 0)
        {
            
            if(v > 0) 
            {
                if(currV.z < vCap)
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,(float)0.1), ForceMode.VelocityChange);
            }

            else 
            {
                if(currV.z > -vCap)
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,(float)(-0.1)), ForceMode.VelocityChange);
            }
        }


        //same for horizontal
        if(h != 0)
        {
            if(h > 0) 
            {
                if(currV.x < vCap)
                    this.GetComponent<Rigidbody>().AddForce(new Vector3((float)0.1,0,0), ForceMode.VelocityChange);
            }

            else 
            {
                if(currV.x > -vCap)
                    this.GetComponent<Rigidbody>().AddForce(new Vector3((float)(-0.1),0,0), ForceMode.VelocityChange);
            }
        }


        //same for jump, but only allow jump if player is touching floor
        /*
        void OnCollisionEnter(Collision c)
        {
            Debug.Log("collision detected");
            if(c.gameObject.CompareTag("floor")) {canJump = true;}
        }

        void OnCollisionExit(Collision c)
        {
            if(c.gameObject.CompareTag("floor")) {canJump = false;}
        }*/
        canJump = (this.transform.position.y - floor.transform.position.y == (float)2.65);
        if(j != 0)
        {
            Debug.Log("Jump key detected");
            if(canJump)
            {
                this.GetComponent<Rigidbody>().AddForce(new Vector3(0,(float)(j/2),0), ForceMode.VelocityChange);
            }
        }
    }
}
