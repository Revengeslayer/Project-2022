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
    private Vector3 InitDic;
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
    //最後動作的時間
    private float lastActTime;
    //隨機動作權重
    private float[] actionWeight = { 3000, 4000 };
    //怪物的目標朝向
    private Quaternion targetRotation;
    //isRuning?
    private bool is_Running = false;



    public enum DogFSMState
    {
        NONE = -1,
        Idle_Battle,
        Attack01,
        Attack02,
        Block,
        Wander,
        Chase,
        GetHit,
        Die
    }

    void Start()
    {
        InitDic = this.transform.position;
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
        if (mCurrentState == DogFSMState.Attack01)
        {

        }
        if (mCurrentState == DogFSMState.Attack02)
        {

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
        }
        if (mCurrentState == DogFSMState.Chase)
        {
            
            //朝向玩家位置
            targetRotation = Quaternion.LookRotation(player.transform.position - gameObject.transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            transform.Translate(Vector3.forward * Time.deltaTime * dogRunSpeed);
            ChaseRadiusCheck();
        }
        if (mCurrentState == DogFSMState.GetHit)
        {

        }
        if (mCurrentState == DogFSMState.Die)
        {

        }
    }

    private void ChaseRadiusCheck()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);
        var diatanceToInitial = Vector3.Distance(gameObject.transform.position, InitDic);

        if (diatanceToInitial > dogBackToInitDic || diatanceToPlayer > dogChaseDic)
        {
            anim.SetBool("Chase", false);
            is_Running = false;
            anim.SetBool("Idle", true);
            mCurrentState = DogFSMState.Idle_Battle;
        }
    }
    /// <summary>
    /// 是否要追逐
    /// </summary>
    private void TargetDicChenk()
    {
        var dicToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if(dicToPlayer< dogChaseDic)
        {
            anim.SetBool("Idle", false);
            is_Running = true;
            anim.SetBool("Chase", true);
            mCurrentState = DogFSMState.Chase;
            
        }
    }

    void Update()
    {
        Debug.Log(mCurrentState);
        CheckNowState();
    }

    private void RandomAction()
    {
        lastActTime = Time.time;

        float name = Random.Range(0, actionWeight[0]+ actionWeight[1]);
        if(name<= actionWeight[0])
        {
            mCurrentState = DogFSMState.Idle_Battle;
            anim.SetBool("Idle", true);
        }
        else if(actionWeight[0] < name&&name <= actionWeight[0]+ actionWeight[1])
        {
            mCurrentState = DogFSMState.Wander;
            anim.SetBool("Wander", true);
            targetRotation = Quaternion.Euler(0, Random.Range(1, 5) * 90, 0);
        }
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
        Gizmos.DrawWireSphere(InitDic, dogBackToInitDic);
    }
}
