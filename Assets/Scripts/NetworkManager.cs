using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Connection;
using System;
using UnityEngine.SceneManagement;
using MLAPI.Transports.UNET;

public class NetworkManager : NetworkedBehaviour
{
    public int connectedClientNo = 0;
    private Scene menuScene;
    [SerializeField]
    public Dictionary<ulong, Player> playerList = new Dictionary<ulong, Player>();

    void Start()
    {
        menuScene = SceneManager.GetActiveScene();
    }

    public void HostServer()
    {
        print("Host Server Called");
        NetworkingManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkingManager.Singleton.OnClientConnectedCallback += ConnectCallback;
        NetworkingManager.Singleton.StartHost();
        playerList.Add(GetComponent<UnetTransport>().ServerClientId, new Player((Teams)(connectedClientNo % 2), GetComponent<UnetTransport>().ServerClientId));
        connectedClientNo += 1;
    }

    public void StartClient(string adress, string port)
    {
        NetworkingManager.Singleton.OnClientConnectedCallback += ConnectCallback;
        NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectAddress = adress;
        NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectPort = int.Parse(port);
        NetworkingManager.Singleton.StartClient();
    }

    private void ConnectCallback(ulong obj)
    {
        print("TTTT");
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkingManager.ConnectionApprovedDelegate callback)
    {
        print("TEST");
        ulong? prefabHash = SpawnManager.GetPrefabHashFromGenerator("WeirdSprite");
        if(SceneManager.GetActiveScene() == menuScene)
        {
            callback(true, prefabHash, true, Vector3.zero, Quaternion.identity);
            playerList.Add(clientId, new Player((Teams)(connectedClientNo % 2), clientId));
            connectedClientNo += 1;
        }
        else
        {
            print("Connection refused because Scene was not Sample Scene");
        }        
    }
}
