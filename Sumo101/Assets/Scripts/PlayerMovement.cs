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

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;

        float[] inputs = new float[3]; 
        inputs[0] = Input.GetAxisRaw("Vertical");;
        inputs[1] = Input.GetAxisRaw("Horizontal");
        inputs[2] = Input.GetAxisRaw("Jump");

        RequestMoveServerRpc(inputs);
        
        Move(inputs);
    }

    [ServerRpc]
    private void RequestMoveServerRpc(float[] moveInputs)
    {
        MoveClientRpc(moveInputs);
    }

    [ClientRpc]
    private void MoveClientRpc(float[] moveInputs)
    {
        Move(moveInputs);
    }

    private void Move(float[] moveInputs)
    {
        float v = moveInputs[0];
        float h = moveInputs[1];
        float j = moveInputs[2];

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