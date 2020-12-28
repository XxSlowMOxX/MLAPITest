using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using System;
using UnityEngine.SceneManagement;

public class NetworkManager : NetworkedBehaviour
{
    public int connectedClientNo = 0;
    private Scene menuScene;

    void Start()
    {
        menuScene = SceneManager.GetActiveScene();
        print("Start");
    }

    public void HostServer()
    {
        print("Host Server Called");
        NetworkingManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkingManager.Singleton.OnClientConnectedCallback += ConnectCallback;
        NetworkingManager.Singleton.StartHost();
    }

    private void ConnectCallback(ulong obj)
    {
        print("TEST");
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkingManager.ConnectionApprovedDelegate callback)
    {
        print("TEST");
        ulong? prefabHash = SpawnManager.GetPrefabHashFromGenerator("WeirdSprite");
        if(SceneManager.GetActiveScene() == menuScene)
        {
            callback(true, prefabHash, true, Vector3.zero, Quaternion.identity);
        }
        else
        {
            print("Connection refused because Scene was not Sample Scene");
        }        
    }
}
