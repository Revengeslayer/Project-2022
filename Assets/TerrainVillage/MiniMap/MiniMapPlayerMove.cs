using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MiniMapPlayerMove : MonoBehaviour
{
    public GameObject miniMapPlayerImg;

    private float mapPlayerx;
    private float mapPlayery;
    private float mapPlayerz;

    //private Vector3 miniMapPlayer = GameObject.Find("MiniMapPlayer").transform.position;

    private float mapStartx;
    private float mapStartz;

    private float mapEndx;
    private float mapEndz;

    private float miniStartx;
    private float miniStartz;

    private float miniEndx;
    private float miniEndz;

    private Vector3 miniPlayermove;
    void Start()
    {
       
    } 


    void FixedUpdate()
    {
        MiniMapPlayerMoves();
    }

    public void MiniMapPlayerMoves()
    {
        mapPlayerx = GameObject.Find("Character(Clone)").transform.position.x;
        mapPlayery = GameObject.Find("Character(Clone)").transform.position.y;
        mapPlayerz = GameObject.Find("Character(Clone)").transform.position.z;
 
        //private Vector3 miniMapPlayer = GameObject.Find("MiniMapPlayer").transform.position;

        mapStartx = GameObject.Find("GameMapStart").transform.position.x;
        mapStartz = GameObject.Find("GameMapStart").transform.position.z;

        mapEndx = GameObject.Find("GameMapEnd").transform.position.x;
        mapEndz = GameObject.Find("GameMapEnd").transform.position.z;

        miniStartx = GameObject.Find("MiniMapStart").transform.position.x;
        miniStartz = GameObject.Find("MiniMapStart").transform.position.z;

        miniEndx = GameObject.Find("MiniMapEnd").transform.position.x;
        miniEndz = GameObject.Find("MiniMapEnd").transform.position.z;

        miniPlayermove = new Vector3( ((mapPlayerx-mapStartx)/(mapEndx-mapStartx))*(miniEndx-miniStartx)+ miniStartx, this.transform.position.y, ((mapPlayerz-mapStartz)/(mapEndz-mapStartz))*(miniEndz-miniStartz)+ miniStartz);

        miniMapPlayerImg.transform.forward = GameObject.Find("Character(Clone)").transform.forward;
        miniMapPlayerImg.transform.Rotate(0, 90, 0);
        this.transform.position = miniPlayermove;
    }
}
