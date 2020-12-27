using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using System;

public class NetworkManager : NetworkedBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HostSetup()
    {
        NetworkingManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkingManager.ConnectionApprovedDelegate callback)
    {
        print("TEST");
        ulong? prefabHash = SpawnManager.GetPrefabHashFromGenerator("WeirdSprite");
        callback(true, prefabHash, true, Vector3.zero, Quaternion.identity);
    }
}
