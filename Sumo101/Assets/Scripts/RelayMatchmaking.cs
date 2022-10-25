using System.Transactions;
using System.Net;
using System.Security;
using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.UI;

public class RelayMatchmaking : NetworkBehaviour
{
    //TODO: 1. set up buttons and input and display text for join code properly
    //TODO: 2. make sure this shit actually works
    
    const int maxPlayers = 8;

    private UnityTransport transport;
    private PowerUpSpawner powerUpSpawner;

    public Button joinButton,createButton;

    public InputField input;

    public Text joinCodeDisplay;

    string joinCode;

    // Start is called before the first frame update
    async void Awake()
    {
        joinCodeDisplay.gameObject.SetActive(false);
        
        transport = FindObjectOfType<UnityTransport>();

        await Authenticate();

        joinButton.interactable = true;
        createButton.interactable = true;
    }

    private static async Task Authenticate()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void CreateGame()
    {
        joinButton.interactable = false;
        createButton.interactable = false;

        //a is an Allocation, a type not recognised by Unity for some reason in this project.
        var a = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
        joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
        Debug.Log("joinCode: " + joinCode);

        transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port,a.AllocationIdBytes,a.Key, a.ConnectionData);

        joinCodeDisplay.text = joinCode;
        joinCodeDisplay.gameObject.SetActive(true);

        NetworkManager.Singleton.StartHost();

        //activate scripts that depend on host.
        powerUpSpawner = FindObjectOfType<PowerUpSpawner>();
        powerUpSpawner.enabled = true;
    }

    public async void JoinGame()
    {
        joinButton.interactable = false;
        createButton.interactable = false;

        //a is an Allocation, a type not recognised by Unity for some reason in this project.
        var a = await RelayService.Instance.JoinAllocationAsync(input.text);

        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port,a.AllocationIdBytes,a.Key, a.ConnectionData, a.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }
}
