using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWallManager : MonoBehaviour
{
    public bool openWall; 
    private List<AirWallState> airWalls;
    public struct AirWallState
    {
        public string airWallType;
        public GameObject wall;
        public bool display;
    }
    public void SetWalls(GameObject[] walls ,string airWallType)
    {
        //Debug.Log("牆壁數量            ="+ walls.Length);
        AirWallState container = new AirWallState();
        for(int i=0; i< walls.Length; i++)
        {
            container.airWallType = airWallType;
            container.wall = walls[i];
            if (walls[i].GetComponent<BoxCollider>().isTrigger == true)
            {
                walls[i].transform.GetChild(0).gameObject.SetActive(true); 
                walls[i].GetComponent<BoxCollider>().isTrigger = false;
                container.display = walls[i].GetComponent<BoxCollider>().isTrigger;
            }
            airWalls.Add(container);
        }

    }

    private void CheckAirWall()
    {
        if (openWall == false)
        {
            return;
        }
        else if (openWall && InstantiateManager.aliveCount == 0)
        {
            openWall = false;
            
            for (int i = 0; i < airWalls.Count; i++)
            {
                airWalls[i].wall.GetComponent<BoxCollider>().isTrigger = true;
                airWalls[i].wall.transform.GetChild(0).gameObject.SetActive(false);
            }
            airWalls.Clear();
        }
    }

    void Start()
    {
        openWall = false;
        airWalls = new List<AirWallState>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAirWall();
        //Debug.Log("數量            ="+airWalls.Count); ;
    }
}
