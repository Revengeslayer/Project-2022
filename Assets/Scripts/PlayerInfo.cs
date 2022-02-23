using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour
{
    private static  GameObject playerHpbar;
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
        playerHpbar.GetComponent<Image>().fillAmount = playerHp / playerMaxHp;
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
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime*8;
        }
        if (skill_X)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 6;
        }
        if (dodge)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 12;
        }
    }


    # region AnimationEvent

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
        if(attack3 == false)
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
        //阿狗普攻
        if (a == 1)
        {
            playerHp = playerHp - 10;
        }

        //Boss普攻
        else if(a == 10)
        {
            playerHp = playerHp - 30;
        }
        //Boss轉動攻
        else if (a == 11)
        {
            playerHp = playerHp - 50;
        }
        //Boss跳攻
        else if (a == 12)
        {
            playerHp = playerHp - 50;
        }
    }
    public static void CarrotArrowDamage()
    {
        playerHpbar.GetComponent<Image>().fillAmount = (playerHp - 10) / playerMaxHp;
        playerHp = playerHp - 10;
        FSM.isGitHit = true;
        if (playerHpbar.GetComponent<Image>().fillAmount <= 0)
        {
            FSM.isDeath = true;
        }
    }
}
