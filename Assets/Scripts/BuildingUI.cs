using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MLAPI;
using MLAPI.Connection;

public class BuildingUI : MonoBehaviour
{
    private PlacementTest place;
    private List<Building> selectedBuildings = new List<Building>();
    private List<GameObject> buildings = new List<GameObject>();

    void Start()
    {
        place = GetComponent<PlacementTest>();
        buildings = AssetSearch.getBuildings();
    }

    void Update()
    {
        if(!place.place && Input.GetMouseButtonDown(0))
        {
            print("Hit");
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
                print("2Hit");
                build.setSelected(true);
                selectedBuildings.Add(build);
            }
        }
    }

    void OnGUI()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            if(GUI.Button(new Rect(Screen.width - ((i+1) * 110), Screen.height - 110, 100, 100), buildings[i].name))
            {
                place.place = !place.place;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (GetComponent<NetworkedObject>().IsLocalPlayer && place.place)
        {            
            Gizmos.color = new Color(0,1,0,0.5f);
            if(Physics2D.OverlapBox(place.getPosMod(), new Vector2(1, 1), 0) != null){
                Gizmos.color = new Color(1, 0, 0, 0.5f);

            }            
            Gizmos.DrawCube(place.getPosMod(), new Vector3(1,1,0));
        }
    }
}
