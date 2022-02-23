using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour
{
    private static GameObject playerHpbar;
    private static GameObject PlayerDizzyBar;
    //public Image hpImage;
    static float playerMaxHp = 500;
    public static float playerHp;
    float playerDistance;//人物與怪物的距離
    //傳Skill給怪物
    public static int skillAttack;
    //傳Attack給怪物
    public static int zAttack;
    //public int reBirth;
    bool attack1 = false;
    bool attack2 = false;
    bool attack3 = false;
    bool dodge = false;
    bool skill_X = false;
    private float at3Time;
    //暈眩次數
    public static float DizzyCount;
    static float playerMaxDizzy = 50;
    //翻滾無敵
    private static bool dodgeInv;
    void Start()
    {
        playerHp = playerMaxHp;
        playerHpbar = GameObject.Find("PlayerHpBar");
        PlayerDizzyBar = GameObject.Find("PlayerDizzyBar");

    }

    // Update is called once per frame
    void Update()
    {

        CheckDamageType();

        DizzyCount = Mathf.Clamp(DizzyCount, 0, playerMaxDizzy);
        skillAttack = 0;
        zAttack = 0;
        playerHpbar.GetComponent<Image>().fillAmount = playerHp / playerMaxHp;
        PlayerDizzyBar.GetComponent<Image>().fillAmount = DizzyCount / playerMaxDizzy;
        playerHp = Mathf.Clamp(playerHp, 0, playerMaxHp);

        if (playerHpbar.GetComponent<Image>().fillAmount <= 0)
        {
            FSM.isDeath = true;
        }

        if (attack1)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 6;
        }
        if (attack2)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 6;
        }
        if (attack3)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 8;
        }
        if (skill_X)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 6;
        }
        if (dodge)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 8;
        }
    }

    private void CheckDamageType()
    {
        if (zAttack == 0)
        {
            RabbitArcherSteeringFSM.zAttack = 0;
        }
        if (zAttack == 1)
        {
            RabbitArcherSteeringFSM.zAttack = 1;
        }
        if (zAttack == 2)
        {
            RabbitArcherSteeringFSM.zAttack = 2;
        }
        if (zAttack == 3)
        {
            RabbitArcherSteeringFSM.zAttack = 3;
        }
        if (skillAttack == 0)
        {
            RabbitArcherSteeringFSM.skillAttack = 0;
        }
        if (skillAttack == 1)
        {
            RabbitArcherSteeringFSM.skillAttack = 1;
        }
        if (skillAttack == 2)
        {
            RabbitArcherSteeringFSM.skillAttack = 2;
        }
        if (skillAttack == 3)
        {
            RabbitArcherSteeringFSM.skillAttack = 3;
        }
    }


    #region AnimationEvent

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
    private void Skill1Hurt1()
    {
        skillAttack = 1;
    }
    private void Skill1Hurt2()
    {
        skillAttack = 2;
    }
    private void Skill2Hurt()
    {
        skillAttack = 3;
    }
    private void Attack1Move()
    {
        if (attack1 == false)
        {
            attack1 = true;
        }
        else if (attack1 == true)
        {
            attack1 = false;
        }
    }
    private void Attack2Move()
    {
        if (attack2 == false)
        {
            attack2 = true;
        }
        else if (attack2 == true)
        {
            attack2 = false;
        }
    }
    private void Attack3Move()
    {
        if (attack3 == false)
        {
            attack3 = true;
        }
        else if (attack3 == true)
        {
            attack3 = false;
        }
    }

    private void Skill_X_Move()
    {
        if (skill_X == false)
        {
            skill_X = true;
        }
        else if (skill_X == true)
        {
            skill_X = false;
        }
    }
    private void DodgeMove()
    {
        if (dodge == false)
        {
            dodge = true;
        }
        else if (dodge == true)
        {
            dodge = false;
        }
    }
    private void Die()
    {
        attack1 = false;
        attack2 = false;
        attack3 = false;
    }
    #endregion
    public static void PlayerHpCal(int a)
    {

        if (!dodgeInv)
        {
            //阿狗普攻
            if (a == 1)
            {
                playerHp = playerHp - 10;
                DizzyCount++;
                FSM.isGitHit = true;
            }

            //Boss普攻
            else if (a == 10)
            {
                playerHp = playerHp - 30;
                DizzyCount++;
                FSM.isGitHit = true;
            }
            //Boss轉動攻
            else if (a == 11)
            {
                playerHp = playerHp - 50;
                DizzyCount += 2;
                FSM.isGitHit = true;
            }
            //Boss跳攻
            else if (a == 12)
            {
                playerHp = playerHp - 200;
                DizzyCount += 20;
                FSM.isGitHit = true;
            }
        }

    }

    public static void CarrotArrowDamage()
    {
        if (!dodgeInv)
        {
            playerHpbar.GetComponent<Image>().fillAmount = (playerHp - 1) / playerMaxHp;
            playerHp = playerHp - 1;
            DizzyCount++;
            if (DizzyCount % 3 == 0 || DizzyCount % 50 == 0)
            {
                FSM.isGitHit = true;
            }
        }
        if (playerHpbar.GetComponent<Image>().fillAmount <= 0)
        {
            FSM.isDeath = true;
        }
    }

    private void DodgeCheck()
    {
        if (dodgeInv == false)
        {
            dodgeInv = true;
        }
        else
        {
            dodgeInv = false;
        }

    }
}