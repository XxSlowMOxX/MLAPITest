using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlacementTest : NetworkedBehaviour
{
    public bool place = true;
    public int gridSize = 2;
    public int buildingsLeft = 1;
    public GameObject myPrefab;
    private Vector3 posMod;

    void Update()
    {
        if(NetworkedObject.IsLocalPlayer && place)
        {
            Vector3 pos = GetComponentInChildren<Camera>().ScreenToWorldPoint(Input.mousePosition);
            posMod = new Vector3(Snapping.Snap(pos.x, gridSize), Snapping.Snap(pos.y, gridSize));
            if (Input.GetMouseButtonDown(0) && buildingsLeft > 0)
            {
                if (IsServer)
                {
                    PlaceObject(posMod, 0);
                }
                else
                {
                    InvokeServerRpc(PlaceObject, posMod, OwnerClientId);
                }
            }
        }
    }

    [ServerRPC]
    void PlaceObject(Vector3 posi, ulong ID)
    {
        int playerResources = NetworkingManager.Singleton.GetComponent<NetworkManager>().playerList[ID].Resources;
        if(playerResources > 0)
        {
            GameObject gO = Instantiate(myPrefab, posi, Quaternion.identity);
            gO.GetComponent<NetworkedObject>().Spawn(null, true); //Der Bool sagt ob das Dings beim Scenenwechsel zerstört werden soll, und das sollen Gebäude werden
            SceneManager.MoveGameObjectToScene(gO, SceneManager.GetActiveScene());
            NetworkingManager.Singleton.GetComponent<NetworkManager>().playerList[ID].Resources -= 1;
        }
        else
        {
            print("Building could not be placed, get more cash");
        }
    }

    void OnDrawGizmos()
    {
        if (NetworkedObject.IsLocalPlayer && place)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(posMod, 0.1f);
        }
    }
}
