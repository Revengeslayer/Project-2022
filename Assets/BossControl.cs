using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    public GameObject objPlayer;
    public GameObject objBoss;

    private float bossHp;

    private int bossState;
    private int bossDo;
    
    private void Start()
    {
        objPlayer = GameObject.Find("Character(Clone)");
        objBoss = this.gameObject;

        bossState = 0;
        bossHp = 1000;

        BossFSM.mCurrentState = BossFSM.BossFSMState.Active;
    }
    // Update is called once per frame
    void Update()
    {
        //if (Vector3.Distance(objPlayer.transform.position, objBoss.transform.position) <= 10.0f)
        //{
        //    objBoss.transform.LookAt(objPlayer.transform.position);
        //    BossFSM.mCurrentState = BossFSM.BossFSMState.Walk;



        //    gameObject.transform.position += gameObject.transform.forward * 2.0f * Time.deltaTime;
        //}



        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Idle;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Active;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Attack1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Attack2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Stand;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Walk;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Roll;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Die;
        }
    }

    void BossNormalAtk()
    {
        Vector3 bossAtkPosition0;
        float bossAtkDistance0;

        bossAtkPosition0 = objBoss.transform.position + objBoss.transform.forward * 6.0f;
        bossAtkDistance0 = Vector3.Distance(bossAtkPosition0, objPlayer.transform.position);
        if (bossAtkDistance0 < 2.0f)
        {
            PlayerInfo.PlayerHpCal(10);
        }
        else
        {
            Debug.Log("Boss§ðÀ»½d³ò¥~");
        }   
    }

    void BossLeftAtk()
    {
        Vector3 bossAtkPosition0;
        float bossAtkDistance0;
        bossAtkPosition0 = objBoss.transform.position + objBoss.transform.forward * 5.0f;
        bossAtkDistance0 = Vector3.Distance(bossAtkPosition0, objPlayer.transform.position);
        if (bossAtkDistance0 < 2.0f)
        {
            PlayerInfo.PlayerHpCal(20);
        }
        else
        {
            Debug.Log("Boss§ðÀ»½d³ò¥~");
        }
    }
    void BossRollAtk()
    {       
        float bossAtkHorizontalDistance;//¾î¦V¶ZÂ÷
        float bossAtkDistance;//ª½¦V¶ZÂ÷

        float a;//ºâ¨¤«×¤À¤l
        float b;//ºâ¨¤«×¤À¥À
        float cosValue;//cos­È

        a = Vector3.Dot((objPlayer.transform.position - objBoss.transform.position), objBoss.transform.forward*2);
        b = Vector3.Distance(objPlayer.transform.position, objBoss.transform.position) * (objPlayer.transform.forward).magnitude*2;
        cosValue = a / b;

        bossAtkDistance = Vector3.Distance(objPlayer.transform.position, objBoss.transform.position) * cosValue;
        bossAtkHorizontalDistance = Mathf.Sqrt(Vector3.Distance(objPlayer.transform.position, objBoss.transform.position) * Vector3.Distance(objPlayer.transform.position, objBoss.transform.position)
                          - bossAtkDistance * bossAtkDistance);

        //¶ZÂ÷­È«í¥¿
        if(bossAtkDistance< 0)
        {
            bossAtkDistance = -bossAtkDistance;
        }

        if(bossAtkHorizontalDistance < 3.5f && bossAtkDistance <4.0f)
        {
            PlayerInfo.PlayerHpCal(11);
        }
        else if (bossAtkHorizontalDistance >= 3.5f || bossAtkDistance >= 4.0f || bossAtkDistance <= 5.0f)
        {
            Debug.Log("-----¨Sºu¨ì-----");
        }
    }

    void BossRollMove()
    {
        objBoss.transform.LookAt (objPlayer.transform.position);        
    }

    private int BossBehavior(int bossState)
    {
        int reBossDo = 0;
        if (bossState == 1)
        {
            reBossDo = Random.Range(0, 3);
        }
        //if (bossState == 2)
        //{
        //    reBossDo = Random.Range(3, 5);
        //}
        return reBossDo;
    }
}
