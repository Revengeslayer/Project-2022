using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private string colliderTag;
    private bool CARotate = false;
    private Vector3 CMRotateVillage;
    private Vector3 CMRotateBattle01;
    private Vector3 Boss01_1;
    private float YT;
    private GameObject Viking_Tower;
    private GameObject Tree;
    // Start is called before the first frame update


    // Update is called once per frame
    private void Start()
    {
        CMRotateVillage = Camera.main.transform.forward;
        CMRotateBattle01 = new Vector3(CMRotateVillage.x * -1, CMRotateVillage.y, CMRotateVillage.z * -1);
        Boss01_1 = -Camera.main.transform.right;
        Viking_Tower = GameObject.Find("Viking_Tower");
        Tree = GameObject.Find("tree03(4)");
    }
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        YT = 0;
        colliderTag = other.tag;
        Debug.Log(other.tag+ "enter");

        if (colliderTag == "Village")
        {
            if (FlowPlayer.offect.x > 0)
            {
                Camera.main.transform.position = gameObject.transform.position + new Vector3(-20f, 8.5f, 0);
                Camera.main.transform.forward = CMRotateBattle01;
                GameObject.Find("MiniMapCamera").transform.Rotate(0, 0, 180);
            }

            FlowPlayer.offect = new Vector3(-20f, 8.5f, 0);
            FlowPlayer.CMRotate = CMRotateBattle01;
            FlowPlayer.smoothTime = 0.25f;
            Viking_Tower.SetActive(true);
            Tree.SetActive(true);
        }
        else if (colliderTag == "Village01")
        {
            FlowPlayer.CARotate = true;
            FlowPlayer.offect = new Vector3(-8.5f, 7.5f, 0);
            FlowPlayer.CMRotate = CMRotateBattle01;
            FlowPlayer.smoothTime = 1;
        }
        else if (colliderTag == "Battle01")
        {
            if (FlowPlayer.offect.x < 0)
            {
                Camera.main.transform.position = gameObject.transform.position + new Vector3(20f, 8.5f, 0);
                Camera.main.transform.forward = CMRotateVillage;
                GameObject.Find("MiniMapCamera").transform.Rotate(0,0,-180);
            }
            FlowPlayer.offect = new Vector3(20f, 8.5f, 0);
            FlowPlayer.CMRotate = CMRotateVillage;
            FlowPlayer.smoothTime = 0.25f;
            
            Viking_Tower.SetActive(false);
            Tree.SetActive(false);
        }
        else if (colliderTag == "ClipNear01")
        {
            Camera.main.nearClipPlane = 8.6f;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        var Y = Input.GetKeyDown(KeyCode.Y);
        var T = 0.1f;
        YT += Time.deltaTime;
        if(Y == false)
        {
            return;
        }
        if (colliderTag == "Village")
        {
            if (Y && YT >T)
            {
                gameObject.transform.position = new Vector3(23.71191f, 2.691814f, 39.16151f);
                YT = 0;
                Y = false;
            }
        }
        if (colliderTag == "Battle01")
        {
            if (Y && YT > T)
            {
                gameObject.transform.position = new Vector3(32.21911f, 3.003113f, 39.03258f);
                YT = 0;
                Y = false;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        colliderTag = other.tag;
        if (colliderTag == "ClipNear01")
        {
            Camera.main.nearClipPlane = 0.1f;
        }
    }


}
