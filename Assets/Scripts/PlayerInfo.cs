using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour
{
    private static GameObject playerHpbar;
    private int[] getHit;
    private static Animator playerAnimator;

    private List<GameObject> dogMonsters;
    //public Image hpImage;
    static float playerMaxHp = 300;
    public static float playerHp;
    float playerDistance;//人物與怪物的距離
                         //public int reBirth;


    void Start()
    {
        dogMonsters = new List<GameObject>();
        playerAnimator = GetComponent<Animator>();
        playerHp = playerMaxHp;

        playerHpbar = GameObject.Find("PlayerHpBar");
    }
    private void Update()
    {
        //Debug.Log(playerHp);
    }
    public static void PlayerHpCal()  //dog
    {
        playerHpbar.GetComponent<Image>().fillAmount = (playerHp - 10) / playerMaxHp;
        playerHp = playerHp - 10;
        if (playerHpbar.GetComponent<Image>().fillAmount <= 0)
        {
            FSM.isDeath = true;
        }
    }
    public static void CarrotArrowDamage()
    {
        playerHpbar.GetComponent<Image>().fillAmount = (playerHp - 10) / playerMaxHp;
        playerHp = playerHp - 10;
        if (playerHpbar.GetComponent<Image>().fillAmount <= 0)
        {
            FSM.isDeath = true;
        }
    }
}

