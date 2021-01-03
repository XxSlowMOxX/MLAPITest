using System.Collections;
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
        if (place.place) {
            PreviewRender(Physics2D.OverlapBox(place.getPosMod(), buildings[currentIndex].GetComponent<Building>().getSize(), 0) != null);
        }
        else {
            if(selectedBuildings.Count == 1) {
                int i = 0;
                foreach(GameObject unit in selectedBuildings[0].GetComponent<Building>().buildableUnits) {
                    if (GUI.Button(new Rect(Screen.width - ((i + 1) * 110), Screen.height - 220, 100, 100), unit.name)) {
                        selectedBuildings[0].BuildUnit(i);
                    }
                    i++;
                }
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

    void PreviewRender(bool possible)
    {
        Vector2 buildingSize = buildings[currentIndex].GetComponent<Building>().getSize();
        Vector3 leftTopCorner = this.GetComponentInChildren<Camera>().WorldToScreenPoint(place.getPosMod() + TopLeft(buildingSize) * 0.5f);
        Vector3 rightBottomCorner = this.GetComponentInChildren<Camera>().WorldToScreenPoint(place.getPosMod() - TopLeft(buildingSize) * 0.5f);
        Rect imagePos = new Rect(leftTopCorner.x,Screen.height - leftTopCorner.y,(rightBottomCorner - leftTopCorner).x, (leftTopCorner - rightBottomCorner).y);
        Texture2D tex = buildings[currentIndex].GetComponent<SpriteRenderer>().sprite.texture;
        if (!possible)
        {
            GUI.color = new Color(0, 1, 0, 0.5f);
        }
        else
        {
            GUI.color = new Color(1, 0, 0, 0.5f);
        }
        GUI.DrawTexture(imagePos, tex);
        GUI.color = Color.white;
    }

    Vector3 TopLeft(Vector2 vec)
    {
        return new Vector3(-vec.x, vec.y, 0);
    }
}
