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
    public GameObject monsterHpbar;
    public Image hpImage;
    public Image hpImage0;
    private GameObject Target;

    //Add
    public static float zAttack;
    public static float skillAttack;
    private float playerHp;
    float monsterHp;
    GameObject Smoke;

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
    private int Atk02CDForTimes;

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
    private bool getHurt;
    private bool WalkForTurn;
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

        monsterHp = 1000;

        Target = GameObject.Find("Character(Clone)");
        Smoke = gameObject.transform.Find("DustSmoke_A").gameObject;

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
        Atk02CDForTimes = 0;

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
        if(Atk02CDForTimes == 3 && ActionBool)
        {
            EEAnim.SetBool("isAtk02", true);

            mCurrentState = FSMState.Attack02;
            mCheckState = CheckAttack02State;
            mDoState = DoAttack02State;

            toAtk02 = true;
            Atk02Time = Time.time + 3.2f;
            ActionBool = false;
            ActionTimer = 0;
            Atk02CDForTimes = 0;
        }
        else if(JumpAtkCDForTimes ==2 && ActionBool)
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
            JumpAtkCDForTimes = 0;
            JumpAtkCDForTimes += 2;
            Atk02CDForTimes ++ ;
        }
        else if (!ActionBool && EEAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            EEAnim.SetBool("isWalk", true);

            mCurrentState = FSMState.Walk;
            mCheckState = CheckWalkState;
            mDoState = DoWalkState;
            CheckWalkTurn();
        }
        else if (DisToTarget < DisForCHASE && ActionBool && Time.time > RollCDTimer)
        {
            EEAnim.SetBool("isRoll", true);

            mCurrentState = FSMState.Roll;
            mCheckState = CheckRollState;
            mDoState = DoRollState;

            toRoll = true;
            RollTime = Time.time + 5.8f;

            JumpAtkCDForTimes ++ ;
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
            JumpAtkCDForTimes ++ ;
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
            RollCDTimer = Time.time + 10;
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
            WalkForTurn = false;
            //Smoke.SetActive(false);
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
        a.y = 0;
        gameObject.transform.forward += a * Time.deltaTime * 2.4f;
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
        var a = (Target.transform.position - gameObject.transform.position).normalized;
        a.y = 0;
        //Smoke.SetActive(true);
        if (WalkForTurn)
        {
            gameObject.transform.forward += (a + new Vector3(0.1f, 0, 0)) * Time.deltaTime * 3f;
        }
        else
        {
            gameObject.transform.forward += (a + new Vector3(0.1f, 0, 0)) * Time.deltaTime * 0.8f;
        }
        if(DisToTarget > 3)
        {
            gameObject.transform.position += gameObject.transform.forward * Time.deltaTime * 1.2f;
        }
        
        

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
        if (DisToTarget < 3)
        {
            ActionTimer += (Time.deltaTime * 0.5f);
        }
        if (ActionTimer > 3)
        {
            ActionBool = true;
        }
    }
    private void CheckAttackType()
    {

    }
    private void CheckWalkTurn()
    {
        var a = (Target.transform.position - gameObject.transform.position).normalized;
        var b = Vector3.Dot(gameObject.transform.forward, a);
        if(b > 0)
        {
            WalkForTurn = false;
        }
        else
        {
            WalkForTurn = true;
        }
    }
    private void PlayerAttack(float zAttack, float skillAttack)
    {
        DisToTarget = (Target.transform.position - gameObject.transform.position).magnitude;
        var a = Vector3.Dot((gameObject.transform.position - Target.transform.position), Target.transform.forward);
        var b = Vector3.Distance(gameObject.transform.position, Target.transform.position) * (Target.transform.forward).magnitude;
        var cosValue = a / b;


        if (zAttack == 1 && cosValue >= 0.7 && hpImage.fillAmount > 0 && DisToTarget <= 3.0f)
        {
            hpImage.fillAmount = hpImage.fillAmount - (25.0f / monsterHp);
            //dogAnimator.SetBool("gethit", true);
            zAttack = 0;
            getHurt = true;
            //Debug.Log("z1");
        }
        else if (zAttack == 2 && cosValue >= 0.7 && hpImage.fillAmount > 0 && DisToTarget <= 3.0f)
        {
            hpImage.fillAmount = hpImage.fillAmount - (25.0f / monsterHp);
            getHurt = true;
            zAttack = 0;
            //Debug.Log("z2");
        }
        else if (cosValue >= 0.7 && zAttack == 3 && hpImage.fillAmount > 0 && DisToTarget <= 3.0f)
        {
            hpImage.fillAmount = hpImage.fillAmount - (50.0f / monsterHp);
            getHurt = true;
            zAttack = 0;
            //Debug.Log("z3");
        }

        //人物技能X 傷害第一段
        if (skillAttack == 1)
        {
            if (cosValue >= 0.8f && hpImage.fillAmount > 0 && DisToTarget <= 2.8f)
            {
                hpImage.fillAmount = hpImage.fillAmount - (40.0f / monsterHp);
                getHurt = true;
                skillAttack = 0;
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                //Debug.Log("s1");
                //Debug.Log("造成傷害 40");

                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移
            }
        }

        //人物技能X 傷害第二段
        else if (skillAttack == 2 && cosValue >= 0.7f && hpImage.fillAmount > 0 && DisToTarget <= 3.5f)
        {

            //前方一段距離的圓傷害判定用
            //Vector3 playrerAtkPosition;
            //float dogMonsterkDistance;

            //playrerAtkPosition = objPlayer.transform.position + objPlayer.transform.forward * 1.0f;
            //dogMonsterkDistance = Vector3.Distance(playrerAtkPosition, objMonster.transform.position);
            //前方一段距離的圓傷害判定用
            hpImage.fillAmount = hpImage.fillAmount - (60.0f / monsterHp);
            getHurt = true;
            skillAttack = 0;
            //dogAnimator.SetBool("Attack01", false);
            //dogAnimator.SetBool("chase", false);
            //Debug.Log("s2");
            //Debug.Log("造成傷害 40");
            //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移

        }
        else if (skillAttack == 3 && DisToTarget <= 2.8f)
        {
            if (hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (20.0f / monsterHp);
                getHurt = true;
                skillAttack = 0;
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                //Debug.Log("s3");
                //Debug.Log("造成傷害 20");
            }
        }
        else if (skillAttack == 4)
        {
            float disToMonster = (gameObject.transform.position - (Target.transform.position + Target.transform.forward * 7.0f)).magnitude;
            var c = Vector3.Dot((gameObject.transform.position - (Target.transform.position + Target.transform.forward * 7.0f)), Target.transform.position - (Target.transform.position + Target.transform.forward * 7.0f));
            var d = Vector3.Distance(gameObject.transform.position, (Target.transform.position + Target.transform.forward * 7.0f)) * (Target.transform.position - (Target.transform.position + Target.transform.forward * 7.0f)).magnitude;
            var cosValue2 = c / d;
            if (hpImage.fillAmount > 0 && disToMonster <= 6.8f)
            {
                if (cosValue2 >= 0.98)
                {
                    hpImage.fillAmount = hpImage.fillAmount - (150.0f / monsterHp);
                    getHurt = true;
                    skillAttack = 0;
                }
                else if (cosValue2 >= 0.95)
                {
                    hpImage.fillAmount = hpImage.fillAmount - (80.0f / monsterHp);
                    getHurt = true;
                    skillAttack = 0;
                }
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                //Debug.Log("s3");
                //Debug.Log("造成傷害 20");
            }
        }

    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(this.transform.position, Target.transform.position);
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(this.transform.position, DisForActivate);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(this.transform.position, DisForATTACK);
    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawWireSphere(this.transform.position, DisForRockShoot);
    //    Gizmos.DrawWireSphere(this.transform.position, DisForCHASE);

    //}

    private void Update()
    {
        //Damage
        if (zAttack != 0 || skillAttack != 0)
        {
            PlayerAttack(zAttack, skillAttack);
        }
        Debug.Log(JumpAtkCDForTimes);

        Debug.Log("目前狀態          " + mCurrentState);
        
        mCheckState();
        //狀態做甚麼
        mDoState();
        //Debug.Log(Time.time - Atk02CDTimer);
    }
}
