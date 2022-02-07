using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    private static Object prefab;

    public static GameObject LoadData()
    {
        GameObject characterPrefabIn;
        prefab = Resources.Load("Character/Character");
        characterPrefabIn =GameObject.Instantiate(prefab) as GameObject;
        return characterPrefabIn;
    }
}
