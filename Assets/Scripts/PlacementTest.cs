using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlacementTest : NetworkedBehaviour
{
    public bool place = true;
    public int gridSize = 2;
    private Vector3 posMod;
    void Update()
    {
        if(NetworkedObject.IsLocalPlayer && place)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posMod = new Vector3(Snapping.Snap(pos.x, gridSize), Snapping.Snap(pos.y, gridSize));
            Debug.Log(posMod);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(posMod, 0.1f);
    }
}
