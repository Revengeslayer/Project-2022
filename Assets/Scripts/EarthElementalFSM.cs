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

    //DoTimer
    private float ActivateTime;
    //Bool
    private bool toIdleActivate;
    private bool toActivate;
    private bool toIdle;
    private bool toRolll;
    private bool toJumpAttack;
    private bool toWalk;
    private bool toAtk01;
    private bool toAtk02;
    private bool toHit;
    private bool toDie;
    private bool ActionBool;
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
        monsterHp = 200;

        Target = GameObject.Find("Character(Clone)");

        mCurrentState = FSMState.IdleActivate;
        mCheckState = CheckIdleActivateState;
        mDoState = DoIdleActivateState;

        //Timer
        ActivateTime = 0;
        ActionTimer = 0;

        //Dist
        DisToTarget = 0;
        DisForActivate = 13;
        DisForCHASE = 0;
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
        Debug.Log(ActivateTime);
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

        if (DisToTarget < DisForATTACK && ActionBool)
        {
            EEAnim.SetBool("isAtk01", true);

            mCurrentState = FSMState.Attack01;
            mCheckState = CheckAttack01State;
            mDoState = DoAttack01State;
            toAtk01 = true;
            ActionBool = false;
            ActionTimer = 0;
        }
    }
    private void CheckRollState()
    {

    }
    private void CheckJumpAttackState()
    {

    }
    private void CheckWalkState()
    {

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
        gameObject.transform.forward += (Target.transform.position - gameObject.transform.position).normalized * Time.deltaTime *1.2f;
        gameObject.transform.position += gameObject.transform.forward * Time.deltaTime *0.8f;
    }
    private void DoRollState()
    {

    }
    private void DoJumpAttackState()
    {

    }
    private void DoWalkState()
    {

    }
    private void DoAttack01State()
    {
        if (EEAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && EEAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.83)
        {
            toAtk01 = false;
        }
    }
    private void DoAttack02State()
    {

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
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(this.transform.position, Target.transform.position);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(this.transform.position, DisForActivate);
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(this.transform.position, DisForATTACK);
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(this.transform.position, DisForRockShoot);
        //Gizmos.DrawWireSphere(this.transform.position, DisForCHASE);

    }

    private void Update()
    {
        Debug.Log("目前狀態          " + mCurrentState);

        mCheckState();
        //狀態做甚麼
        mDoState();

    }
}
