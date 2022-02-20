using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalsvHP: MonoBehaviour
{
    //水晶血條
    public GameObject crystalsvHp;
    public Image hpImage;
    public Image hpImage0;

    // Update is called once per frame
    void Update()
    {
        //水晶血條面向攝影機
        crystalsvHp.transform.forward = GameObject.Find("Main Camera").transform.forward * -1; //怪物Hp條面向攝影機
    }
}
