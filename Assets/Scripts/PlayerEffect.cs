using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    public GameObject atklight1;
    public GameObject atklight2;
    public GameObject atklight3_1;
    public GameObject atklight3_2;
    public GameObject skillX_1;
    public GameObject skillX_2;
    public GameObject skillC;
    public GameObject skillV0;
    public GameObject skillV;
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
    void AtklightClose3_2()
    {
        atklight3_2.SetActive(false);
    }
    void SkillX_1_Open()
    {
        skillX_1.SetActive(true);
    }
    void SkillX_1_Close()
    {
        skillX_1.SetActive(false);
    }
    void SkillX_2_Open()
    {
        skillX_2.SetActive(true);
    }
    void SkillX_2_Close()
    {
        skillX_2.SetActive(false);
    }

    void SkillC_Open()
    {
        skillC.SetActive(true);
    }
    void SkillC_Close()
    {
        skillC.SetActive(false);
    }

    void SkillV0_Open()
    {
        skillV0.SetActive(true);
    }
    void SkillV0_Close()
    {
        skillV0.SetActive(false);
    }
    void SkillV_Open()
    {
        skillV.SetActive(true);
    }
    void SkillV_Close()
    {
        skillV.SetActive(false);
    }

    void AtkEffectClose()
    {
        AtklightClose1();
        AtklightClose2();
        AtklightClose3_1();
        AtklightClose3_2();
    }

    void SkillEffectClose()
    {
        SkillX_1_Close();
        SkillX_2_Close();
        SkillC_Close();
        SkillV0_Close();
        SkillV_Close();
    }
    void DieEffect()
    {
        AtklightClose1();
        AtklightClose2();
        AtklightClose3_1();
        AtklightClose3_2();


        SkillX_1_Close();
        SkillX_2_Close();
        SkillC_Close();
        SkillV0_Close();
        SkillV_Close();
    }
}
