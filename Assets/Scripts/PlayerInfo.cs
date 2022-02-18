using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour
{
    private static  GameObject playerHpbar;
    //public Image hpImage;
    static float playerMaxHp = 300;
    public static float playerHp;
    float playerDistance;//짩か팒㈖かず턟차
    //또Skill돌㈖か
    public static int skillAttack;
    //또Attack돌㈖か
    public static int zAttack;
    //public int reBirth;

    void Start()
    {
        playerHp = playerMaxHp;
        playerHpbar = GameObject.Find("PlayerHpBar");

    }

    // Update is called once per frame
    void Update()
    {
        skillAttack = 0;
        zAttack = 0;
    }

    private void Attack1Hurt()
    {
        zAttack = 1;
    } 
    private void Attack2Hurt()
    {
        zAttack = 2;
    }
    private void Attack3Hurt()
    {
        zAttack = 3;
    }
    private void Skill1Hurt()
    {
        skillAttack = 1;
    }
    private void Skill2Hurt()
    {
        skillAttack = 2;
    }
    public static void PlayerHpCal()
    {
        playerHpbar.GetComponent<Image>().fillAmount = (playerHp-10)/playerMaxHp;
        playerHp = playerHp - 10;
        if (playerHpbar.GetComponent<Image>().fillAmount <= 0)
        {
            FSM.isDeath = true;
        }
    }
}
