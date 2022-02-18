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
    float playerDistance;//人物與怪物的距離
    //傳Skill給怪物
    public static int skillAttack;
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
