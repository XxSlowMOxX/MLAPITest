using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

/// <summary>
/// Standard Way to Search for Sepcific Asset Groups, such as Buildi
/// </summary>
public static class AssetSearch
{
    /// <summary>
    /// Returns Building List
    /// </summary>
    /// <param name="networkedRequired"> Is a Networked Object required</param>
    /// <returns>Returns all Building Prefabs as Gameobjects</returns>
    public static List<GameObject> getBuildings(bool networkedRequired = true){
        List<GameObject> buildings = new List<GameObject>();
        GameObject[] gOs = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        foreach(GameObject gO in gOs)
        {            
            Component comp;
            if(gO.TryGetComponent(typeof(Building),out comp))
            {
                if (networkedRequired)
                {
                    if(gO.TryGetComponent(typeof(NetworkedObject), out comp))
                    {
                        buildings.Add(gO);
                    }
                }
                else
                {
                    buildings.Add(gO);
                }
            }
        }
        Debug.Log("returned " + buildings.Count.ToString() + " Buildings");
        return buildings;
    }
}
