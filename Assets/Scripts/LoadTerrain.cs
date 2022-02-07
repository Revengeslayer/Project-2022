using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTerrain : MonoBehaviour
{
    private static Object[] prefabs;

    public static List<GameObject> LoadData()
    {
        List<GameObject> terrainPrefabIns = new List<GameObject>();
        prefabs = Resources.LoadAll("Terrains");

        foreach(var ins in prefabs)
        {
            terrainPrefabIns.Add(GameObject.Instantiate(ins) as GameObject);
        }

        return terrainPrefabIns;
    }
}
