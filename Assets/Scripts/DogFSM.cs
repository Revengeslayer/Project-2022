using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFSM : MonoBehaviour
{
    //目標
    private GameObject player;
    /// <summary>
    /// Dog相關
    /// </summary>
    //怪物初始位置
    private Vector3 initDic;
    [Header("走路速度")]
    public float dogWalkSpeed;
    [Header("跑步速度")]
    public float dogRunSpeed;
    /// <summary>
    /// Dog 動畫相關
    /// </summary>
    private Animator anim;
    /// <summary>
    /// 當前狀態
    /// </summary>
    public static DogFSMState mCurrentState;
    /// <summary>
    /// 各種偵測距離
    /// </summary>
    [Header("攻擊距離")]
    public float dogAtkDic;
    [Header("追逐距離")]
    public float dogChaseDic;
    [Header("警戒距離")]
    public float dogWarnDic;
    [Header("回歸距離")]
    public float dogBackToInitDic;
    [Header("更換待機指令的間隔時間")]
    public float actRestTme;
    [Header("追丟或超出範圍的間隔時間")]
    public float reRestTme;
    [Header("怪物思考時間")]
    public float mobThinkTme;
    //最後動作的時間
    private float lastActTime;
    //隨機動作權重
    private float[] actionWeight = { 3000, 4000 };
    //怪物的目標朝向
    private Quaternion targetRotation;
    //isRuning?
    private bool is_Running = false;
    //ReturnIdle Waittime
    private float returnIdleWaittime;
    //AttackIdle Waittime
    private float attackIdleTime;



    public enum DogFSMState
    {
        NONE = -1,
        Idle_Battle,
        AttackIdle,
        Attack01,
        Attack02,
        Block,
        Wander,
        Chase,
        GetHit,
        Die,
        Return,
        ReturnIdle,
        LookPlayer
    }

    void Start()
    {
        initDic = this.transform.position;
        player =GameObject.Find("Character");
        //player=GameObject.Find("Character(Clone)");
        mCurrentState = DogFSMState.Idle_Battle;
        anim = GetComponent<Animator>();
    }


    private void CheckNowState()
    {
        if (mCurrentState == DogFSMState.Idle_Battle)
        {
            if(Time.time-lastActTime>actRestTme)
            {
                anim.SetBool("Idle", false);
                RandomAction();
            }
            TargetDicChenk();
        }
        if(mCurrentState==DogFSMState.AttackIdle)
        {
            Vector3 vLookForword = player.transform.position - gameObject.transform.position;
            vLookForword.y = 0;
            gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, vLookForword,0.1f);
            
            if (CanNextActionCheck(attackIdleTime))
            {
                AttackIdleCheck();             
            }
        }
        if (mCurrentState == DogFSMState.Attack01)
        {
            float fToPlayerDic = Vector3.Distance(player.transform.position, gameObject.transform.position);
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime>1)
            {
                //Debug.Log("攻擊完畢");
                anim.SetBool("Attack1", false);
                attackIdleTime = Time.time;
                anim.SetBool("Idle", true);
                mCurrentState = DogFSMState.AttackIdle;
            }
            else
            {
                if(fToPlayerDic<=dogAtkDic)
                {
                    Debug.Log("攻擊到了");
                }
            }
        }
        if (mCurrentState == DogFSMState.Attack02)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                //Debug.Log("攻擊完畢");
                anim.SetBool("Attack2", false);
                attackIdleTime = Time.time;
                anim.SetBool("Idle", true);
                mCurrentState = DogFSMState.AttackIdle;
            }
        }
        if (mCurrentState == DogFSMState.Block)
        {

        }
        if (mCurrentState == DogFSMState.Wander)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * dogWalkSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);

            if (Time.time - lastActTime > actRestTme)
            {
                anim.SetBool("Wander", false);
                RandomAction();
            }
            WanderRadiusCheck();
        }
        if (mCurrentState == DogFSMState.Chase)
        {
            float fPlayerToDogInit = Vector3.Distance(player.transform.position, initDic);
            if(Vector3.Distance(gameObject.transform.position,initDic)< dogBackToInitDic)
            {
                //朝向玩家位置
                targetRotation = Quaternion.LookRotation(player.transform.position - gameObject.transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
                transform.Translate(Vector3.forward * Time.deltaTime * dogRunSpeed);
                ChaseCancelCheck();
            }
            else
            {
                if (fPlayerToDogInit > dogBackToInitDic)
                {
                    is_Running = false;
                    anim.SetBool("Chase", false);
                    anim.SetBool("Idle", true);
                    mCurrentState = DogFSMState.LookPlayer;
                }
                else
                {
                    targetRotation = Quaternion.LookRotation(player.transform.position - gameObject.transform.position, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
                    transform.Translate(Vector3.forward * Time.deltaTime * dogRunSpeed);
                    ChaseCancelCheck();
                }
            }
        }
        if (mCurrentState == DogFSMState.GetHit)
        {

        }
        if (mCurrentState == DogFSMState.Die)
        {

        }
        if(mCurrentState == DogFSMState.Return)
        {
            targetRotation = Quaternion.LookRotation(initDic - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            transform.Translate(Vector3.forward * Time.deltaTime * dogRunSpeed);
            ReturnCheck();
        }
        if(mCurrentState == DogFSMState.ReturnIdle)
        {
            if (ReturnIdleCheck(returnIdleWaittime))
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Wander", true);
                mCurrentState = DogFSMState.Return;
            }
            ReturnIdleCheck();
        }
        if(mCurrentState==DogFSMState.LookPlayer)
        {
            Vector3 vLookForword = player.transform.position - gameObject.transform.position;
            vLookForword.y = 0;
            gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, vLookForword, 0.1f);
            float fPlayerToDoInit = Vector3.Distance(player.transform.position, initDic);

            if (Vector3.Distance(player.transform.position, gameObject.transform.position) > dogChaseDic)
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Wander", true);
                mCurrentState = DogFSMState.Return;
            }
            else
            {
                if (fPlayerToDoInit < dogBackToInitDic)
                {
                    is_Running = true;
                    anim.SetBool("Idle", false);
                    anim.SetBool("Chase", true);
                    mCurrentState = DogFSMState.Chase;
                }
            }
            
        }
    }

    private bool CanNextActionCheck(float thinktime)
    {
        if (Time.time - thinktime > mobThinkTme)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void AttackIdleCheck()
    {
        //Debug.Log("可以行動");
        float fToPlayerDic = Vector3.Distance(player.transform.position, gameObject.transform.position);

        int num = UnityEngine.Random.Range(1, 3);
        if(num==1)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Attack1", true);
            mCurrentState = DogFSMState.Attack01;
        }
        if(num==2)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Attack2", true);
            mCurrentState = DogFSMState.Attack02;
        }

    }

    
    private void ReturnIdleCheck()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (diatanceToPlayer < dogChaseDic)
        {
            anim.SetBool("Idle", false);
            is_Running = true;
            anim.SetBool("Chase", true);
            mCurrentState = DogFSMState.Chase;
        }
    }

    private bool ReturnIdleCheck(float waitTime)
    {
        if(Time.time-waitTime>1.2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void WanderRadiusCheck()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        var diatanceToInitial = Vector3.Distance(transform.position, initDic);

        if (diatanceToPlayer < dogChaseDic)
        {
            if (diatanceToInitial >= dogBackToInitDic)
            {
                anim.SetBool("Wander", false);
                anim.SetBool("Idle", true);
                mCurrentState = DogFSMState.LookPlayer;
            }
            else
            {
                anim.SetBool("Wander", false);
                is_Running = true;
                anim.SetBool("Chase", true);
                mCurrentState = DogFSMState.Chase;
            }
        }

        if (diatanceToInitial> dogBackToInitDic)
        {
            targetRotation = Quaternion.LookRotation(initDic - transform.position, Vector3.up);
        }
    }
    /// <summary>
    /// 如果已經接近初始位置，則隨機一個待機狀態
    /// </summary>
    private void ReturnCheck()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);
        var diatanceToInitial = Vector3.Distance(gameObject.transform.position, initDic);
        if(diatanceToPlayer< dogChaseDic)
        {
            if (diatanceToInitial >= dogBackToInitDic)
            {
                anim.SetBool("Idle", true);
                mCurrentState = DogFSMState.LookPlayer;
            }
            else
            {
                anim.SetBool("Wander", false);
                is_Running = true;
                anim.SetBool("Chase", true);
                mCurrentState = DogFSMState.Chase;
            }
        }
        if (diatanceToInitial < 0.5f)
        {
            anim.SetBool("Wander", false);
            RandomAction();
        }
    }
    /// <summary>
    /// 追逐超出範圍/追逐跟丟的檢查
    /// </summary>
    private void ChaseCancelCheck()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);
        var diatanceToInitial = Vector3.Distance(gameObject.transform.position, initDic);

        if(diatanceToPlayer<=dogAtkDic)
        {
            is_Running = false;
            anim.SetBool("Chase", false);
            attackIdleTime = Time.time;
            anim.SetBool("Idle", true);
            mCurrentState = DogFSMState.AttackIdle;
        }
        if (diatanceToInitial > dogBackToInitDic || diatanceToPlayer > dogChaseDic)
        {
            is_Running = false;
            anim.SetBool("Chase", false);
            returnIdleWaittime = Time.time;
            anim.SetBool("Idle", true);
            mCurrentState = DogFSMState.ReturnIdle;
        }
    }
    /// <summary>
    /// 是否要追逐
    /// </summary>
    private void TargetDicChenk()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);
        var diatanceToInitial = Vector3.Distance(gameObject.transform.position, initDic);
        if (diatanceToPlayer < dogChaseDic && diatanceToInitial < dogBackToInitDic)
        {
            anim.SetBool("Idle", false);
            is_Running = true;
            anim.SetBool("Chase", true);
            mCurrentState = DogFSMState.Chase;          
        }
        if (diatanceToPlayer < dogChaseDic && diatanceToInitial >= dogBackToInitDic)
        {
            anim.SetBool("Idle", true);
            mCurrentState = DogFSMState.LookPlayer;
        }
    }
    /// <summary>
    /// 選擇待機/遊走
    /// </summary>
    private void RandomAction()
    {
        lastActTime = Time.time;

        float name = UnityEngine.Random.Range(0, actionWeight[0] + actionWeight[1]);
        if (name <= actionWeight[0])
        {
            mCurrentState = DogFSMState.Idle_Battle;
            anim.SetBool("Idle", true);
        }
        else if (actionWeight[0] < name && name <= actionWeight[0] + actionWeight[1])
        {
            mCurrentState = DogFSMState.Wander;
            anim.SetBool("Wander", true);
            targetRotation = Quaternion.Euler(0, UnityEngine.Random.Range(1, 5) * 90, 0);
        }
    }
    void Update()
    {
        //Debug.Log(mCurrentState);
        CheckNowState();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, dogAtkDic);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, dogChaseDic);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, dogWarnDic);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(initDic, dogBackToInitDic);
    }
}
