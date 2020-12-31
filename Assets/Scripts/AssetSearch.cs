using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
        string[] gOs = AssetDatabase.FindAssets("t:gameObject");
        foreach(string gO in gOs)
        {
            GameObject posBuild = (GameObject) AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(gO), typeof(GameObject));
            Component comp;
            if(posBuild.TryGetComponent(typeof(Building),out comp))
            {
                if (networkedRequired)
                {
                    if(posBuild.TryGetComponent(typeof(NetworkedObject), out comp))
                    {
                        buildings.Add(posBuild);
                    }
                }
                else
                {
                    buildings.Add(posBuild);
                }
            }
        }
        return buildings;
    }
}
