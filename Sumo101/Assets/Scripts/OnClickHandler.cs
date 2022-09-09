using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.EventSystems;

public class OnClickHandler : MonoBehaviour
{
    public GameObject menuCanvas;

    public void OnClick()
    {
        //if(!IsOwner) {return;}

        string name = EventSystem.current.currentSelectedGameObject.name;
        if(name == "StartHostButton") {NetworkManager.Singleton.StartHost(); Destroy(menuCanvas);} 
        else if(name == "StartClientButton") {NetworkManager.Singleton.StartClient(); Destroy(menuCanvas);}
    }

    /*[ServerRpc]
    public void RequestDestroyServerRpc(GameObject g)
    {
        RequestDestroyClientRpc(g);
    }

    [ClientRpc]
    public void RequestDestroyClientRpc(GameObject g)
    {
        Destroy(g);
    }*/
}
