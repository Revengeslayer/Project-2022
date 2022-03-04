using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDust : MonoBehaviour
{
    GameObject EE;
    private float PDTimer;
    private void Start()
    {
        EE =  GameObject.Find("EarthElemental Variant");
        gameObject.transform.position = EE.transform.position + new Vector3(0,0.2f,0);
        gameObject.SetActive(true);
        PDTimer = 0;
    }
    private void Update()
    {
        PDTimer += Time.deltaTime;
        if(PDTimer > 1.5)
        {
            Destroy(gameObject);
        }
    }
}
