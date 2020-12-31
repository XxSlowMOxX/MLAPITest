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
            if(Physics2D.OverlapCircle(place.getPosMod(), 1) != null){
                Gizmos.color = Color.red;
            }            
            Gizmos.DrawSphere(place.getPosMod(), 0.1f);
        }
    }
}
