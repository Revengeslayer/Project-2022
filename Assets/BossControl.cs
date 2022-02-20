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
    }
    // Update is called once per frame
    void Update()
    {




        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    BossFSM.mCurrentState = BossFSM.BossFSMState.Idle;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    BossFSM.mCurrentState = BossFSM.BossFSMState.Active;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    BossFSM.mCurrentState = BossFSM.BossFSMState.Attack1;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    BossFSM.mCurrentState = BossFSM.BossFSMState.Attack2;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    BossFSM.mCurrentState = BossFSM.BossFSMState.Stand;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    BossFSM.mCurrentState = BossFSM.BossFSMState.Walk;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    BossFSM.mCurrentState = BossFSM.BossFSMState.Roll;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    BossFSM.mCurrentState = BossFSM.BossFSMState.Die;
        //}
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
            Debug.Log("Boss攻擊範圍外");
        }   
    }

    void BossRollAtk()
    {       
        float bossAtkHorizontalDistance;//橫向距離
        float bossAtkDistance;//直向距離

        float a;//算角度分子
        float b;//算角度分母
        float cosValue;//cos值

        a = Vector3.Dot((objPlayer.transform.position - objBoss.transform.position), objBoss.transform.forward*2);
        b = Vector3.Distance(objPlayer.transform.position, objBoss.transform.position) * (objPlayer.transform.forward).magnitude*2;
        cosValue = a / b;

        bossAtkDistance = Vector3.Distance(objPlayer.transform.position, objBoss.transform.position) * cosValue;
        bossAtkHorizontalDistance = Mathf.Sqrt(Vector3.Distance(objPlayer.transform.position, objBoss.transform.position) * Vector3.Distance(objPlayer.transform.position, objBoss.transform.position)
                          - bossAtkDistance * bossAtkDistance);

        //距離值恆正
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
            Debug.Log("-----沒滾到-----");
        }
    }

    void BossRollMove()
    {
        objBoss.transform.LookAt (objPlayer.transform.position);
        
    }

    private int BossBehavior(int bossStatus)
    {
        int reBossDo = 0;
        if (bossStatus == 1)
        {
            reBossDo = Random.Range(0, 3);
        }

        return reBossDo;
    }
}
