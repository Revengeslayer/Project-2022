using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStateController : MonoBehaviour
{
    // Start is called before the first frame update
    float Timer;
    public static Vector3 TargetPos;
    void Start()
    {
        Timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = TargetPos;
        Timer += Time.deltaTime;
        if (Timer > 0.5)
        {
            Destroy(gameObject);
        }
    }
}
