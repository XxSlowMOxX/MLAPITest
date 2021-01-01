﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MLAPI;
using MLAPI.Connection;

public class BuildingUI : NetworkedBehaviour
{
    private PlacementTest place;
    private int currentIndex = 0;
    private List<Building> selectedBuildings = new List<Building>();
    private List<GameObject> buildings = new List<GameObject>();

    void Start()
    {
        place = GetComponent<PlacementTest>();
        buildings = AssetSearch.getBuildings();
    }

    void Update()
    {
        if (!NetworkedObject.IsLocalPlayer) return;
        if(!place.place && Input.GetMouseButtonDown(0))
        {
            Building build;
            Collider2D col = Physics2D.OverlapCircle(place.getPosMod(), 1);
            if(col != null && col.gameObject.TryGetComponent<Building>(out build))
            {
                if (!Input.GetKey(KeyCode.LeftShift)) //Allows multiple Building Selection
                {
                    foreach (Building item in selectedBuildings)
                    {
                        item.setSelected(false);
                    }
                    selectedBuildings.Clear();
                }
                build.setSelected(true);
                selectedBuildings.Add(build);
            }
        }
    }

    void OnGUI()
    {
        if (!NetworkedObject.IsLocalPlayer) return;
        for (int i = 0; i < buildings.Count; i++)
        {
            if(GUI.Button(new Rect(Screen.width - ((i+1) * 110), Screen.height - 110, 100, 100), buildings[i].name))
            {
                togglePlace(i);
                currentIndex = i;
            }
        }
    }
    void togglePlace(int index)
    {
        if (place.place)
        {
            //Wechsel von Platzieren auf nix Platzieren
            place.place = false;
        }
        else
        {
            //Wechsel von auswählen auf platzieren
            place.place = true;
            foreach(Building build in selectedBuildings)
            {
                build.setSelected(false);
            }
            selectedBuildings.Clear();
        }
        place.buildingName = buildings[index].name;
        place.setBuildSize(buildings[currentIndex].GetComponent<Building>().getSize());
    }

    void OnDrawGizmos() //replace with Preview Render
    {
        if (GetComponent<NetworkedObject>().IsLocalPlayer && place.place)
        {            
            Gizmos.color = new Color(0,1,0,0.5f);
            if(Physics2D.OverlapBox(place.getPosMod(),buildings[currentIndex].GetComponent<Building>().getSize(), 0) != null){
                Gizmos.color = new Color(1, 0, 0, 0.5f);

            }            
            Gizmos.DrawCube(place.getPosMod(),buildings[currentIndex].GetComponent<Building>().getSize());            
        }
    }
}
