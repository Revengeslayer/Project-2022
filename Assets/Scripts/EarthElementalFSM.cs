using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthElementalFSM : MonoBehaviour
{

    //檢查狀態的delegate
    private delegate void CheckState();
    private CheckState mCheckState;
    //做狀態的delegate
    private delegate void DoState();
    private CheckState mDoState;

    private Animator EEAnim;
    private Rigidbody EERig;
    private BoxCollider EEBox;
    //public GameObject monsterHpbar;
    //public Image hpImage;
    //public Image hpImage0;
    private GameObject Target;

    //Add
    public static float zAttack;
    public static float skillAttack;
    private float playerHp;
    float monsterHp;

    private FSMState mCurrentState;

    //Distance
    private float DisToTarget;
    private float DisForActivate;
    private float DisForCHASE;
    private float DisForATTACK;
    private float DisForRockShoot;

    //Vector
    private Vector3 ActionVec;

    //Timer
    private float ActionTimer;
    private float RollCDTimer;
    private float Atk02CDTimer;
    private int JumpAtkCDForTimes;

    //DoTimer
    private float ActivateTime;
    private float RollTime;
    private float Atk02Time;
    private float JumpAtkTime;

    //Bool
    private bool toIdleActivate;
    private bool toActivate;
    private bool toIdle;
    private bool toRoll;
    private bool toJumpAttack;
    private bool toWalk;
    private bool toAtk01;
    private bool toAtk02;
    private bool toHit;
    private bool toDie;
    private bool ActionBool;
    private bool AtkSwitch;
    public enum FSMState
    {
        NONE = -1,
        IdleActivate,
        Activate,
        Idle,
        Roll,
        JumpAttack,
        Walk,
        Attack01,
        Attack02
    }

    
    void Start()
    {
        EEAnim = GetComponent<Animator>();
        EERig = GetComponent<Rigidbody>();
        EEBox = GetComponent<BoxCollider>();

        monsterHp = 200;

        Target = GameObject.Find("Character(Clone)");

        mCurrentState = FSMState.IdleActivate;
        mCheckState = CheckIdleActivateState;
        mDoState = DoIdleActivateState;

        //Timer
        ActivateTime = 0;
        ActionTimer = 1;
        RollTime = Time.time;
        RollCDTimer = Time.time;
        Atk02CDTimer = Time.time;
        Atk02Time = Time.time;
        JumpAtkTime = Time.time;
        JumpAtkCDForTimes = 0;

        //Dist
        DisToTarget = 0;
        DisForActivate = 13;
        DisForCHASE = 30;
        DisForATTACK = 6;
        DisForRockShoot = 0;


    //Target = GameObject.Find("Character(Clone)");

    //Timer


    //Dist

}
    #region CheckState
    private void CheckIdleActivateState()
    {
        CheckDistToTarget();
        if(DisToTarget < DisForActivate)
        {
            EEAnim.SetBool("isActivate" , true);

            mCurrentState = FSMState.Activate;
            mCheckState = CheckActivateState;
            mDoState = DoIdleActivateState;

            ActionVec = (Target.transform.position - gameObject.transform.position);
        }
    }
    private void CheckActivateState()
    {
        ActivateTime += Time.deltaTime;
        if (ActivateTime > 5.2f)
        {
            EEAnim.SetBool("isActivate", false);

            mCurrentState = FSMState.Idle;
            mCheckState = CheckIdleState;
            mDoState = DoIdleState;
        }
    }
    private void CheckIdleState()
    {
        CheckDistToTarget();
        CheckActionTimer();
        if(JumpAtkCDForTimes ==2 && ActionBool)
        {
            EEAnim.SetBool("isJumpAtk", true);

            mCurrentState = FSMState.JumpAttack;
            mCheckState = CheckJumpAttackState;
            mDoState = DoJumpAttackState;

            ActionBool = false;
            ActionTimer = 0;
            JumpAtkCDForTimes = 0;
            JumpAtkTime = Time.time + 1.95f;
            toJumpAttack = true;
        }
        else if (DisToTarget < DisForATTACK && ActionBool)
        {
            EEAnim.SetBool("isAtk01", true);

            mCurrentState = FSMState.Attack01;
            mCheckState = CheckAttack01State;
            mDoState = DoAttack01State;
            toAtk01 = true;
            ActionBool = false;
            ActionTimer = 0;
            JumpAtkCDForTimes += 2;
        }
        else if (!ActionBool && EEAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            EEAnim.SetBool("isWalk", true);

            mCurrentState = FSMState.Walk;
            mCheckState = CheckWalkState;
            mDoState = DoWalkState;
        }
        else if (DisToTarget < DisForCHASE && ActionBool && Time.time > RollCDTimer)
        {
            EEAnim.SetBool("isRoll", true);

            mCurrentState = FSMState.Roll;
            mCheckState = CheckRollState;
            mDoState = DoRollState;

            toRoll = true;
            RollTime = Time.time + 5.8f;

            JumpAtkCDForTimes += 1;
        }
        else if (DisToTarget > DisForATTACK && Time.time > Atk02CDTimer && ActionBool)
        {
            EEAnim.SetBool("isAtk02", true);

            mCurrentState = FSMState.Attack02;
            mCheckState = CheckAttack02State;
            mDoState = DoAttack02State;

            toAtk02 = true;
            Atk02Time = Time.time + 3.2f;
            ActionBool = false;
            ActionTimer = 0;
            JumpAtkCDForTimes += 1;
        }
        //else
        //{
        //    EEAnim.SetBool("isJumpAtk", true);

        //    mCurrentState = FSMState.JumpAttack;
        //    mCheckState = CheckJumpAttackState;
        //    mDoState = DoJumpAttackState;

        //    ActionBool = false;
        //    ActionTimer = 0;
        //    JumpAtkCDForTimes = 0;
        //    JumpAtkTime = Time.time + 1.95f;
        //    toJumpAttack = true;
        //}
        
    }
    private void CheckRollState()
    {
        if (!toRoll)
        {
            EEAnim.SetBool("isRoll", false);

            ActionBool = false;
            ActionTimer = 0;

            mCurrentState = FSMState.Idle;
            mCheckState = CheckIdleState;
            mDoState = DoIdleState;
            RollCDTimer += Time.time + 10;
        }
    }
    private void CheckJumpAttackState()
    {
        if(!toJumpAttack)
        {
            EEAnim.SetBool("isJumpAtk", false);

            mCurrentState = FSMState.Idle;
            mCheckState = CheckIdleState;
            mDoState = DoIdleState;
        }
    }
    private void CheckWalkState()
    {
        CheckDistToTarget();

        if (DisToTarget > DisForATTACK)
        {
            RollCDTimer -= Time.deltaTime;
            //Debug.Log(RollCDTimer);
        }

        CheckActionTimer();
        if(ActionBool)
        {
            EEAnim.SetBool("isWalk", false);

            mCurrentState = FSMState.Idle;
            mCheckState = CheckIdleState;
            mDoState = DoIdleState;
        }

    }
    private void CheckAttack01State()
    {
        if(!toAtk01)
        {
            EEAnim.SetBool("isAtk01", false);

            mCurrentState = FSMState.Idle;
            mCheckState = CheckIdleState;
            mDoState = DoIdleState;
        }
    }
    private void CheckAttack02State()
    {
        if(!toAtk02)
        {
            EEAnim.SetBool("isAtk02", false);

            mCurrentState = FSMState.Idle;
            mCheckState = CheckIdleState;
            mDoState = DoIdleState;
            Atk02CDTimer = Time.time + 8;

            ActionBool = false;
            ActionTimer = 0;
        }
    }
    #endregion
    #region DoState

    private void DoIdleActivateState()
    {

    }
    private void DoAtivateState()
    {
        
    }
    private void DoIdleState()
    {
    }
    private void DoRollState()
    {
        var a = (Target.transform.position - gameObject.transform.position).normalized;
        //a.y = 0;
        gameObject.transform.forward += a * Time.deltaTime * 1.2f;
        if (Time.time > RollTime)
        {
            toRoll = false;
        }
    }
    private void DoJumpAttackState()
    {
        if(Time.time > JumpAtkTime)
        {
            toJumpAttack = false;
        }
    }
    private void DoWalkState()
    {
        gameObject.transform.forward += ((Target.transform.position - gameObject.transform.position).normalized +new Vector3 (0.1f,0,0)) * Time.deltaTime * 2f;
        gameObject.transform.position += gameObject.transform.forward * Time.deltaTime * 0.8f;
    }
    private void DoAttack01State()
    {
        if (EEAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01")/* && EEAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.83*/)
        {
            toAtk01 = false;
        }
    }
    private void DoAttack02State()
    {
        if (Time.time > Atk02Time) 
        {
            toAtk02 = false;
        }
    }
    #endregion

    private void CheckDistToTarget()
    {
        DisToTarget = (Target.transform.position - gameObject.transform.position).magnitude;
    }
    private void CheckActionTimer()
    {
        ActionTimer += Time.deltaTime;
        if(ActionTimer > 3)
        {
            ActionBool = true;
        }
    }
    private void CheckAttackType()
    {

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, Target.transform.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, DisForActivate);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, DisForATTACK);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, DisForRockShoot);
        Gizmos.DrawWireSphere(this.transform.position, DisForCHASE);

    }

    private void Update()
    {
        Debug.Log("目前狀態          " + mCurrentState);
        
        mCheckState();
        //狀態做甚麼
        mDoState();
        //Debug.Log(Time.time - Atk02CDTimer);
    }
}
