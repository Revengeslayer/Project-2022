using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMapMob : MonoBehaviour
{
    public GameObject test;
    private float mapStartx;
    private float mapStartz;

    private float mapEndx;
    private float mapEndz;

    private float miniStartx;
    private float miniStartz;

    private float miniEndx;
    private float miniEndz;
    // Start is called before the first frame update
    void Start()
    {
        mapStartx = GameObject.Find("GameMapStart").transform.position.x;
        mapStartz = GameObject.Find("GameMapStart").transform.position.z;

        mapEndx = GameObject.Find("GameMapEnd").transform.position.x;
        mapEndz = GameObject.Find("GameMapEnd").transform.position.z;

        miniStartx = GameObject.Find("MiniMapStart").transform.position.x;
        miniStartz = GameObject.Find("MiniMapStart").transform.position.z;

        miniEndx = GameObject.Find("MiniMapEnd").transform.position.x;
        miniEndz = GameObject.Find("MiniMapEnd").transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        ShowCursor(test);
    }
    void ShowCursor(GameObject mobPos)
    {

        Vector3 mobMiniPos = new Vector3(((mobPos.transform.position.x - mapStartx) / (mapEndx - mapStartx)) * (miniEndx - miniStartx) + miniStartx, this.transform.position.y, ((mobPos.transform.position.z - mapStartz) / (mapEndz - mapStartz)) * (miniEndz - miniStartz) + miniStartz);

        Debug.Log(mobMiniPos);
        this.transform.Rotate(0, 90, 0);

        this.transform.position = mobMiniPos;
        this.transform.forward = mobPos.transform.forward;
    }
}
