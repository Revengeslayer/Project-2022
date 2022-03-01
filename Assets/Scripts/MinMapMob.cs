using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMapMob : MonoBehaviour
{
    [Header("´å¼Ð¹Ï¥Ü")]
    public GameObject prefab;
    private GameObject test;
    private float mapStartx;
    private float mapStartz;

    private float mapEndx;
    private float mapEndz;

    private float miniStartx;
    private float miniStartz;

    private float miniEndx;
    private float miniEndz;

    List<MobsCursor> Cursors;
    struct MobsCursor
    {
        public GameObject mob;
        public GameObject cursor;
    }



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

        Cursors = new List<MobsCursor>();
    }

    // Update is called once per frame
    void Update()
    {
        CursorAdjForword();
        
    }

    private void CursorAdjForword()
    {
        if (Cursors == null)
        {
            return;
        }
        else
        {
            foreach(var a in Cursors)
            {
                a.cursor.transform.forward = a.mob.transform.forward;
                a.cursor.transform.position = GameToMapPos(a.mob);
                if(a.mob.activeSelf)
                {
                    a.cursor.SetActive(true);
                }
                else
                {
                    a.cursor.SetActive(false);
                }
            }
        }
    }

    public void InstantiateCursor(GameObject mob)
    {
        Vector3 mobMiniPos = new Vector3(((mob.transform.position.x - mapStartx) / (mapEndx - mapStartx)) * (miniEndx - miniStartx) + miniStartx, this.transform.position.y+0.06f, ((mob.transform.position.z - mapStartz) / (mapEndz - mapStartz)) * (miniEndz - miniStartz) + miniStartz);
        var cursor =Instantiate(prefab, mobMiniPos, mob.transform.rotation);

        MobsCursor mobC = new MobsCursor();
        mobC.mob = mob;
        mobC.cursor = cursor;
        Cursors.Add(mobC);
    }

    private Vector3 GameToMapPos(GameObject mob )
    {
        return  new Vector3(((mob.transform.position.x - mapStartx) / (mapEndx - mapStartx)) * (miniEndx - miniStartx) + miniStartx, this.transform.position.y + 0.06f, ((mob.transform.position.z - mapStartz) / (mapEndz - mapStartz)) * (miniEndz - miniStartz) + miniStartz); ;
    }
}
