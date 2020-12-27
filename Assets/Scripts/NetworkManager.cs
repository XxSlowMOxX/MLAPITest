using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using System;

public class NetworkManager : NetworkedBehaviour
{
    public int connectedClientNo = 0;
    public void HostServer()
    {
        print("Host Server Called");
        NetworkingManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkingManager.Singleton.OnClientConnectedCallback += ConnectCallback;
        NetworkingManager.Singleton.StartHost();
    }

    private void ConnectCallback(ulong obj)
    {
        print(obj);
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkingManager.ConnectionApprovedDelegate callback)
    {
        print("TEST");
        ulong? prefabHash = SpawnManager.GetPrefabHashFromGenerator("WeirdSprite");
        callback(true, prefabHash, true, Vector3.zero, Quaternion.identity);
    }
}
