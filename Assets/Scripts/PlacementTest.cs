using MLAPI;
using MLAPI.Messaging;
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
                    PlaceObject(posMod);
                }
                else
                {
                    InvokeServerRpc(PlaceObject, posMod);
                }
            }
        }
    }

    [ServerRPC]
    void PlaceObject(Vector3 posi)
    {
        GameObject gO = Instantiate(myPrefab, posi, Quaternion.identity);
        gO.GetComponent<NetworkedObject>().Spawn();
        SceneManager.MoveGameObjectToScene(gO, SceneManager.GetActiveScene());
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
