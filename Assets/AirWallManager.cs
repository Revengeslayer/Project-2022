using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWallManager : MonoBehaviour
{
    private List<AirWallState> airWalls;
    public struct AirWallState
    {
        public string airWallType;
        public GameObject wall;
        public bool display;
    }
    public void SetWalls(GameObject[] walls ,string airWallType)
    {
        Debug.Log("牆壁數量            ="+ walls.Length);
        AirWallState container = new AirWallState();
        for(int i=0; i< walls.Length; i++)
        {
            container.airWallType = airWallType;
            container.wall = walls[i];
            container.display = walls[i].GetComponent<BoxCollider>().isTrigger;
            airWalls.Add(container);
        }

    }

    private void Show()
    {
        if (airWalls == null)
        {
            return;
        }
        else
        {
            foreach( var container in airWalls)
            { 
                Debug.Log("門類型=  " + container.airWallType + "            門=  " + container.wall + "       門顯示=  " + container.display);
            }
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            airWalls[0].wall.GetComponent<BoxCollider>().isTrigger = false;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            airWalls[1].wall.GetComponent<BoxCollider>().isTrigger = false;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            airWalls[2].wall.GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    void Start()
    {
        airWalls = new List<AirWallState>();
    }

    // Update is called once per frame
    void Update()
    {
        Show();
    }
}
