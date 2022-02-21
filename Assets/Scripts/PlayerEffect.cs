using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    public GameObject atklight1;
    public GameObject atklight2;
    public GameObject atklight3_1;
    public GameObject atklight3_2;

    void AtklightOpen1()
    {
        atklight1.SetActive(true);
    }
    void AtklightClose1()
    {
        atklight1.SetActive(false);
    }

    void AtklightOpen2()
    {
        atklight2.SetActive(true);
    }
    void AtklightClose2()
    {
        atklight2.SetActive(false);
    }

    void AtklightOpen3_1()
    {
        atklight3_1.SetActive(true);
    }
    void AtklightClose3_1()
    {
        atklight3_1.SetActive(false);
    }

    void AtklightOpen3_2()
    {
        atklight3_2.SetActive(true);
    }
    void AtklightClose3__2()
    {
        atklight3_2.SetActive(false);
    }
}
