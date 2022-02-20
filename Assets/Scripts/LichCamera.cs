using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichCamera : MonoBehaviour
{
    public float offsetX;
    public float offsetY;
    public float offsetZ;

    public float spinX;
    public float spinY;
    public float spinZ;
    public Camera camera;
    public Transform target;

    // Update is called once per frame
    //void Update()
    //{
    //    //camera.transform.position = target.position+ new Vector3(offsetX, offsetY, offsetZ);
    //    //camera.transform.Rotate(new Vector3(spinX, spinY, spinY));
    //}
}
