using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMonster: MonoBehaviour
{
    private static Object[] prefabs;
    public static List<GameObject> LoadData()
    {
        List<GameObject> monsterPrefabIns = new List<GameObject>();
        prefabs = Resources.LoadAll("Monsters");

        foreach (var ins in prefabs)
        {
            monsterPrefabIns.Add(GameObject.Instantiate(ins) as GameObject);
        }

        return monsterPrefabIns;
    }
}
