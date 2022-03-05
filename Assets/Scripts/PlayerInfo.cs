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
    public static float playerMaxHp = 800;
    public static float playerHp;
    float playerDistance;//人物與怪物的距離
    //傳Skill給怪物
    public static int skillAttack;
    //傳Attack給怪物
    public static int zAttack;
    //public int reBirth;
    bool attack1 = false;
    bool attack2 = false;
    [SerializeField]
    bool attack3 = false;
    bool dodge = false;
    bool skill_X = false;
    bool skill_V = false;
    private float at3Time;
    //暈眩次數
    public static float DizzyCount;
    static float playerMaxDizzy = 50;
    //翻滾無敵
    private static bool dodgeInv;
    void Start()
    {
        DizzyCount = 0;
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
        //PlayerDizzyBar.GetComponent<Image>().fillAmount = DizzyCount / playerMaxDizzy;
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
        if (skill_V)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 8;
        }
        if (dodge)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 8;
        }
    }

    private void CheckDamageType()
    {
        RabbitArcherSteeringFSM.zAttack = zAttack;
        RabbitArcherSteeringFSM.skillAttack = skillAttack;

        BlueRBFSM.zAttack = zAttack;
        BlueRBFSM.skillAttack = skillAttack;

        GreenRBFSM.zAttack = zAttack;
        GreenRBFSM.skillAttack = skillAttack;

        EliteArcher.zAttack = zAttack;
        EliteArcher.skillAttack = skillAttack;

        DogFSM.zAttack = zAttack;
        DogFSM.skillAttack = skillAttack;

        EarthElementalFSM.zAttack = zAttack;
        EarthElementalFSM.skillAttack = skillAttack;

        CarrotController.skillAttack = skillAttack;
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
    private void Skill3Hurt()
    {
        skillAttack = 4;
    }
    private void Attack1MoveOpen()
    {
        attack1 = true;
    }
    private void Attack1MoveClose()
    {
        attack1 = false;
    }
    private void Attack2MoveOpen()
    {
        attack2 = true;
    }    private void Attack2MoveClose()
    {
        attack2 = false;
    }
    private void Attack3MoveOpen()
    {
        attack3 = true;
    }
    private void Attack3MoveClose()
    {
        attack3 = false;
    }

    private void Skill_X_MoveOpen()
    {       
        skill_X = true;
    }
    private void Skill_X_MoveClose()
    {
        skill_X = false;
    }

    private void Skill_V_MoveOpen()
    {
        skill_V = true;
    }
    private void Skill_V_MoveClose()
    {
        skill_V = false;
    }
    private void DodgeMoveOpen()
    {
        dodge = true;
    }
    private void DodgeMoveClose()
    {
        dodge = false;
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
    private void AtkMove()
    {
        attack1 = false;
        attack2 = false;
        attack3 = false;
    }
    private void Hit()
    {
        attack1 = false;
        attack2 = false;
        attack3 = false;
        skill_X = false;
        skill_V = false;
        dodge = false;
        dodgeInv = false;
    }
    private void Die()
    {
        attack1 = false;
        attack2 = false;
        attack3 = false;
        skill_X = false;
        skill_V = false;
        dodge = false;
        dodgeInv = false;
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
            //Spike
            else if (a == 13)
            {
                playerHp = playerHp - 50;
                DizzyCount += 10;
                FSM.isGitHit = true;
            }
        }

    }

    public static void CarrotArrowDamage(string ATKtype)
    {
        if (!dodgeInv && ATKtype == "A")
        {
            playerHpbar.GetComponent<Image>().fillAmount = (playerHp - 10) / playerMaxHp;
            playerHp = playerHp - 10;
            DizzyCount++;
            if (DizzyCount % 3 == 0 || DizzyCount % 50 == 0)
            {
                FSM.isGitHit = true;
            }
        }
        if (!dodgeInv && ATKtype == "B")
        {
            playerHpbar.GetComponent<Image>().fillAmount = (playerHp - 15) / playerMaxHp;
            playerHp = playerHp - 15;
            DizzyCount++;
            if (DizzyCount % 3 == 0 || DizzyCount % 50 == 0)
            {
                FSM.isGitHit = true;
            }
        }
        if (!dodgeInv && ATKtype == "C")
        {
            playerHpbar.GetComponent<Image>().fillAmount = (playerHp - 25) / playerMaxHp;
            playerHp = playerHp - 25;
            DizzyCount++;
            if (DizzyCount % 3 == 0 || DizzyCount % 50 == 0)
            {
                FSM.isGitHit = true;
            }
        }
        if (!dodgeInv && ATKtype == "D")
        {
            playerHpbar.GetComponent<Image>().fillAmount = (playerHp - 40) / playerMaxHp;
            playerHp = playerHp - 40;
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


    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, 3f);
    }

}