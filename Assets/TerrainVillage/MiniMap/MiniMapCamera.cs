using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    private Vector3 offset;
    void Start()
    {
        offset = GameObject.Find("MiniMapPlayer").transform.position - this.transform.position;
    }

  
    void Update()
    {
        this.transform.position = GameObject.Find("MiniMapPlayer").transform.position - offset;
    }
}
