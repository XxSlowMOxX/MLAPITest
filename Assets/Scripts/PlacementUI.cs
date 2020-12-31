using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Connection;

public class PlacementUI : MonoBehaviour
{
    private PlacementTest place;

    void Start()
    {
        place = GetComponent<PlacementTest>();
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(Screen.width - 110, Screen.height - 110, 100, 100), "Building"))
        {
            place.place = !place.place;
        }
    }

    void OnDrawGizmos()
    {
        if (GetComponent<NetworkedObject>().IsLocalPlayer && place.place)
        {            
            Gizmos.color = Color.green;
            if(Physics2D.OverlapBox(place.getPosMod(), new Vector2(1, 1), 0) != null){
                Gizmos.color = Color.red;
            }            
            Gizmos.DrawCube(place.getPosMod(), new Vector3(1,1,0));
        }
    }
}
