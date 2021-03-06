﻿using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PlacementTest : NetworkedBehaviour
{
    public bool place = true;
    public int gridSize = 2;
    public int buildingsLeft = 1;
    public string buildingName;
    public GameObject myPrefab;
    private Vector3 posMod;
    private Vector2 buildSize;

    public Vector3 getPosMod()
    {
        return posMod;
    }
    public void setBuildSize(Vector2 size)
    {
        print("new Build Size");
        buildSize = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
    }

    void Update()
    {
        if(NetworkedObject.IsLocalPlayer)
        {
            try {
                Vector3 pos = GetComponentInChildren<Camera>().ScreenToWorldPoint(Input.mousePosition);
                posMod = new Vector3(Snapping.Snap(pos.x, gridSize), Snapping.Snap(pos.y, gridSize));
            }
            catch (System.Exception e) {
                print(e);
            }            
            if (!place) return;
            if (Input.GetMouseButtonDown(0) && buildingsLeft > 0 && Physics2D.OverlapBox(posMod, buildSize, 0) == null)
            {
                if (IsServer)
                {
                    PlaceObject(posMod, 0, buildingName, buildSize);
                }
                else
                {
                    InvokeServerRpc(PlaceObject, posMod, OwnerClientId, buildingName, buildSize);
                }
            }
        }
    }

    [ServerRPC]
    void PlaceObject(Vector3 posi, ulong ID, string objectName, Vector2 boundingBox)
    {
        int playerResources = NetworkingManager.Singleton.GetComponent<NetworkManager>().playerList[ID].Resources;
        if(playerResources > 0 && Physics2D.OverlapBox(posi, new Vector2(1, 1), 0) == null)
        {
            GameObject gO = Instantiate((GameObject)Resources.Load(objectName), posi, Quaternion.identity);
            gO.GetComponent<NetworkedObject>().Spawn(null, true); //Der Bool sagt ob das Dings beim Scenenwechsel zerstört werden soll, und das sollen Gebäude werden
            SceneManager.MoveGameObjectToScene(gO, SceneManager.GetActiveScene());
            NetworkingManager.Singleton.GetComponent<NetworkManager>().playerList[ID].Resources -= 1;
        }
        else
        {
            print("Object could not be placed");
        }
    }    
}
