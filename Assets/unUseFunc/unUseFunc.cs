using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unUseFunc : MonoBehaviour
{
    delegate void keydown();
    Dictionary<string, keydown> creator = new Dictionary<string, keydown>();
    void Update()
    {
        //if (Input.anyKey)
        //{
        //    Debug.Log(Input.inputString);
        //    creator[Input.inputString]();
        //}
    }
    void Controller()
    {
        //creator[""] = () => { };
        //creator["w"] = () => player.transform.position += player.transform.forward * Time.deltaTime; ;
        //creator["s"] = () => player.transform.position -= player.transform.forward * Time.deltaTime;
        //creator["a"] = () => {
        //    player.transform.Rotate(0, -100 * Time.deltaTime, 0);
        //    player.transform.position += player.transform.forward * Time.deltaTime / 10;
        //};
        //creator["d"] = () => {
        //    player.transform.Rotate(0, 100 * Time.deltaTime, 0);
        //    player.transform.position += player.transform.forward * Time.deltaTime / 10;
        //};
        //creator["Space"] = () => player.transform.position -= player.transform.forward * Time.deltaTime;
    }
}
