using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CollisionHandler : MonoBehaviour
{
    float secondsToApplyForce = (float)0.1,timeCount = 0;
    bool addingForce = false;
    Vector3 force;

    private void FixedUpdate() 
    {
        if(addingForce)
        {
            timeCount += Time.deltaTime;
   
            if(timeCount < secondsToApplyForce)
            {
                GetComponent<Rigidbody>().AddForce(force * 100);
            }

            else 
            {
                timeCount = 0;
                addingForce = false;   
            }
        }
    } 

    void Move(Vector3 v) 
    {
        addingForce = true;
        force = v;
    }

    private void OnCollisionEnter(Collision other) 
    {
        //add force in the form of velocity in order to not worry about deceleration
        if(other.gameObject.CompareTag("player")) 
        {
            Vector3 velocity = other.gameObject.GetComponent<Rigidbody>().velocity;
            RequestMoveServerRpc(velocity);
        }
    }

    [ServerRpc]
    private void RequestMoveServerRpc(Vector3 moveInputs)
    {
        MoveClientRpc(moveInputs);
    }

    [ClientRpc]
    private void MoveClientRpc(Vector3 moveInputs)
    {
        Move(moveInputs);
    }
}
