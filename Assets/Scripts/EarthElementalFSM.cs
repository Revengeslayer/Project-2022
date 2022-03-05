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
    public Image hpImage1;
    public Image hpImage2;
    public Image hpImage0;
    private GameObject Target;
    public GameObject DieAnim;
    private Vector3 HitBox1;
    public GameObject HitBox2;
    public GameObject HitBox3;
    private float HitBoxDist1;
    private float HitBoxDist2;
    private float HitBoxDist3;
    private float Fever;

    //Add
    public static float zAttack;
    public static float skillAttack;
    private float playerHp;
    float monsterHp;
    float monsterHp1;
    float monsterHp2;
    float monsterTotalHp;
    float monsterMaxHp;
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
    private float DieTimer;

    //DoTimer
    private float ActivateTime;
    private float RollTime;
    private float Atk02Time;
    private float JumpAtkTime;
    private float ChargeTime;

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
    private bool Alife;
    private bool bHpCharge;
    private bool immortal;

    private GameObject Win;
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
        Attack02,
        Die
    }


    void Start()
    {

        Win = GameObject.Find("GameWin");
        Win.SetActive(false);
        EEAnim = GetComponent<Animator>();
        EERig = GetComponent<Rigidbody>();
        EEBox = GetComponent<BoxCollider>();

        Fever = 1;
        Alife = true;
        monsterMaxHp = 3000;
        monsterTotalHp = 0;
        monsterHp = monsterMaxHp / 3;
        monsterHp1 = monsterMaxHp / 3;
        monsterHp2 = monsterMaxHp / 3;
        monsterHpbar.SetActive(false);

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
        DieTimer = 0;
        ChargeTime = Time.time;

        //Dist
        DisToTarget = 0;
        DisForActivate = 13;
        DisForCHASE = 30;
        DisForATTACK = 6;
        DisForRockShoot = 0;

        //Bool
        immortal = true;

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

            monsterHpbar.SetActive(true);
            bHpCharge = true;
            //ChargeTime = Time.time + 5;
            //monsterHp += Time.deltaTime;
            //monsterHp = Mathf.Clamp(monsterHp + Time.deltaTime * 100 , 0, 1000);

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
        if (playerHp <= 0)
        {
            return;
        }
        if (Atk02CDForTimes == 2 && ActionBool)
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
        else if (DisToTarget < DisForCHASE && ActionBool && Time.time > RollCDTimer)
        {
            EEAnim.SetBool("isRoll", true);

            mCurrentState = FSMState.Roll;
            mCheckState = CheckRollState;
            mDoState = DoRollState;

            toRoll = true;
            RollTime = Time.time + 5.8f;

            JumpAtkCDForTimes++;
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
            Atk02CDForTimes ++;
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
            RollCDTimer = Time.time + 25;
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
            Atk02CDTimer = Time.time + 5;

            ActionBool = false;
            ActionTimer = 0;
        }
    }
    private void CheckDieState()
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
    }
    private void DoRollState()
    {
        var a = (Target.transform.position - gameObject.transform.position).normalized;
        a.y = 0;
        gameObject.transform.forward += a * Time.deltaTime * 5f;
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
        //if (WalkForTurn)
        {
            //gameObject.transform.forward += (a + new Vector3(0.1f, 0, 0)) * Time.deltaTime * 3f;
        }
        //else
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
    private void DoDieState()
    {
        EEAnim.Play("Die");
        Win.SetActive(true);
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

    private void HpCharge()
    {
        //monsterHp = Mathf.Clamp(monsterHp + Time.deltaTime * 100 , 0, 1000);

        monsterTotalHp += Time.deltaTime * (monsterMaxHp / 3);
        if (monsterTotalHp < monsterHp)
        {
            hpImage.fillAmount = (monsterTotalHp / monsterHp);
            //monsterHp --;
        }
        else if (monsterTotalHp < monsterHp + monsterHp1)
        {
            hpImage.fillAmount = 1;
            hpImage1.fillAmount = ((monsterTotalHp - monsterHp )/ monsterHp1);
            //monsterHp1--;
        }
        else if (monsterTotalHp < monsterHp + monsterHp1 + monsterHp2)
        {
            hpImage1.fillAmount = 1;
            hpImage2.fillAmount = ((monsterTotalHp - (monsterHp + monsterHp1)) / monsterHp2);
            //monsterHp2--;
        }


        Debug.Log("Docharge");
        Debug.Log(monsterHp);
        if (monsterTotalHp >= monsterMaxHp)
        {
            monsterTotalHp = monsterMaxHp;
            hpImage2.fillAmount = 1;
            bHpCharge = false;
            immortal = false;
        }
    }
    private void GetHitBox()
    {
        HitBox1 = gameObject.transform.position - (gameObject.transform.forward ) + new Vector3 (0,0.6f,0);
        //HitBox2 = gameObject.transform.position - (gameObject.transform.right * 1) + gameObject.transform.forward;
        //HitBox3 = gameObject.transform.position + (gameObject.transform.right * 1) + gameObject.transform.forward;
    }
    private void GetHurtDist()
    {

    }
    void DieEvent()
    {
        GameObject G = (GameObject)Instantiate(DieAnim, gameObject.transform.position, gameObject.transform.rotation);
        G.transform.localScale = new Vector3(3, 3, 3);
        //if (DieTimer > 0.5)
        {
            gameObject.SetActive(false);
            G.SetActive(true);
        }
    }
    private void PlayerAttack(float zAttack, float skillAttack , Vector3 HitBoxPos)
    {
        if (getHurt)
        {
            return ;
        }
        if(immortal)
        {
            return ;
        }
        if(monsterTotalHp <= monsterMaxHp / 3 * 2)
        {
            hpImage2.fillAmount = 0;
        }
        if(monsterTotalHp <= monsterMaxHp / 3)
        {
            hpImage1.fillAmount = 0;
        }
        if(monsterTotalHp <= 0)
        {
            hpImage0.fillAmount = 0;
        }

        var DisToTarget = (Target.transform.position - HitBoxPos).magnitude;
        var a = Vector3.Dot((HitBoxPos - Target.transform.position), Target.transform.forward);
        var b = Vector3.Distance(HitBoxPos, Target.transform.position) * (Target.transform.forward).magnitude;
        var cosValue = a / b;

        if (zAttack == 1 && cosValue >= 0.7 && hpImage.fillAmount > 0 && DisToTarget <= 3.0f)
        {
            if (monsterTotalHp > monsterHp + monsterHp1)
            {
                hpImage2.fillAmount = ((monsterHp2 - 25.0f) / (monsterMaxHp / 3));
                monsterHp2 -= 25.0f;
            }
            else if (monsterTotalHp > monsterHp)
            {
                hpImage1.fillAmount = ((monsterHp1 - 25.0f) / (monsterMaxHp / 3));
                monsterHp1 -= 25.0f;
            }
            else if (monsterTotalHp >= 0)
            {
                hpImage.fillAmount = ((monsterHp - 25.0f) / (monsterMaxHp / 3));
                monsterHp -= 25.0f;
            }
            //hpImage.fillAmount = ((monsterHp - 25.0f) / monsterMaxHp);
            monsterTotalHp -= 25.0f;
            zAttack = 0;
            getHurt = true;
        }
        else if (zAttack == 2 && cosValue >= 0.7 && hpImage.fillAmount > 0 && DisToTarget <= 3.0f)
        {
            if (monsterTotalHp > monsterHp + monsterHp1)
            {
                hpImage2.fillAmount = ((monsterHp2 - 25.0f) / (monsterMaxHp / 3));
                monsterHp2 -= 25.0f;
            }
            else if (monsterTotalHp > monsterHp)
            {
                hpImage1.fillAmount = ((monsterHp1 - 25.0f) / (monsterMaxHp / 3));
                monsterHp1 -= 25.0f;
            }
            else if (monsterTotalHp >= 0)
            {
                hpImage.fillAmount = ((monsterHp - 25.0f) / (monsterMaxHp / 3));
                monsterHp -= 25.0f;
            }
            //hpImage.fillAmount = ((monsterHp - 25.0f) / monsterMaxHp);
            monsterTotalHp -= 25.0f;
            getHurt = true;
            zAttack = 0;
            //Debug.Log("z2");
        }
        else if (cosValue >= 0.7 && zAttack == 3 && hpImage.fillAmount > 0 && DisToTarget <= 3.0f)
        {
            if (monsterTotalHp > monsterHp + monsterHp1)
            {
                hpImage2.fillAmount = ((monsterHp2 - 50.0f) / (monsterMaxHp / 3));
                monsterHp2 -= 50.0f;
            }
            else if (monsterTotalHp > monsterHp)
            {
                hpImage1.fillAmount = ((monsterHp1 - 50.0f) / (monsterMaxHp / 3));
                monsterHp1 -= 50.0f;
            }
            else if (monsterTotalHp >= 0)
            {
                hpImage.fillAmount = ((monsterHp - 50.0f) / (monsterMaxHp / 3));
                monsterHp -= 50.0f;
            }
            //hpImage.fillAmount = ((monsterHp - 25.0f) / monsterMaxHp);
            monsterTotalHp -= 50.0f;
            getHurt = true;
            zAttack = 0;
            //Debug.Log("z3");
        }

        //人物技能X 傷害第一段
        if (skillAttack == 1)
        {
            if (cosValue >= 0.8f && hpImage.fillAmount > 0 && DisToTarget <= 2.8f)
            {
                if (monsterTotalHp > monsterHp + monsterHp1)
                {
                    hpImage2.fillAmount = ((monsterHp2 - 40.0f) / (monsterMaxHp / 3));
                    monsterHp2 -= 40.0f;
                }
                else if (monsterTotalHp > monsterHp)
                {
                    hpImage1.fillAmount = ((monsterHp1 - 40.0f) / (monsterMaxHp / 3));
                    monsterHp1 -= 40.0f;
                }
                else if (monsterTotalHp >= 0)
                {
                    hpImage.fillAmount = ((monsterHp - 40.0f) / (monsterMaxHp / 3));
                    monsterHp -= 40.0f;
                }
                //hpImage.fillAmount = ((monsterHp - 25.0f) / monsterMaxHp);
                monsterTotalHp -= 40.0f;
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
            if (monsterTotalHp > monsterHp + monsterHp1)
            {
                hpImage2.fillAmount = ((monsterHp2 - 60.0f) / (monsterMaxHp / 3));
                monsterHp2 -= 60.0f;
            }
            else if (monsterTotalHp > monsterHp)
            {
                hpImage1.fillAmount = ((monsterHp1 - 60.0f) / (monsterMaxHp / 3));
                monsterHp1 -= 60.0f;
            }
            else if (monsterTotalHp >= 0)
            {
                hpImage.fillAmount = ((monsterHp - 60.0f) / (monsterMaxHp / 3));
                monsterHp -= 60.0f;
            }
            //hpImage.fillAmount = ((monsterHp - 25.0f) / monsterMaxHp);
            monsterTotalHp -= 60.0f;
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
                if (monsterTotalHp > monsterHp + monsterHp1)
                {
                    hpImage2.fillAmount = ((monsterHp2 - 20.0f) / (monsterMaxHp / 3));
                    monsterHp2 -= 20.0f;
                }
                else if (monsterTotalHp > monsterHp)
                {
                    hpImage1.fillAmount = ((monsterHp1 - 20.0f) / (monsterMaxHp / 3));
                    monsterHp1 -= 20.0f;
                }
                else if (monsterTotalHp >= 0)
                {
                    hpImage.fillAmount = ((monsterHp - 20.0f) / (monsterMaxHp / 3));
                    monsterHp -= 20.0f;
                }
                //hpImage.fillAmount = ((monsterHp - 25.0f) / monsterMaxHp);
                monsterTotalHp -= 20.0f;
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
            float disToMonster = (HitBoxPos - (Target.transform.position + Target.transform.forward * 7.0f)).magnitude;
            var c = Vector3.Dot((HitBoxPos - (Target.transform.position + Target.transform.forward * 7.0f)), Target.transform.position - (Target.transform.position + Target.transform.forward * 7.0f));
            var d = Vector3.Distance(HitBoxPos, (Target.transform.position + Target.transform.forward * 7.0f)) * (Target.transform.position - (Target.transform.position + Target.transform.forward * 7.0f)).magnitude;
            var cosValue2 = c / d;
            if (hpImage.fillAmount > 0 && disToMonster <= 6.8f)
            {
                if (cosValue2 >= 0.98)
                {
                    if (monsterTotalHp > monsterHp + monsterHp1)
                    {
                        hpImage2.fillAmount = ((monsterHp2 - 150.0f) / (monsterMaxHp / 3));
                        monsterHp2 -= 150.0f;
                    }
                    else if (monsterTotalHp > monsterHp)
                    {
                        hpImage1.fillAmount = ((monsterHp1 - 150.0f) / (monsterMaxHp / 3));
                        monsterHp1 -= 150.0f;
                    }
                    else if (monsterTotalHp >= 0)
                    {
                        hpImage.fillAmount = ((monsterHp - 150.0f) / (monsterMaxHp / 3));
                        monsterHp -= 150.0f;
                    }
                    //hpImage.fillAmount = ((monsterHp - 25.0f) / monsterMaxHp);
                    monsterTotalHp -= 150.0f;
                    getHurt = true;
                    skillAttack = 0;
                }
                else if (cosValue2 >= 0.95)
                {
                    if (monsterTotalHp > monsterHp + monsterHp1)
                    {
                        hpImage2.fillAmount = ((monsterHp2 - 80.0f) / (monsterMaxHp / 3));
                        monsterHp2 -= 80.0f;
                    }
                    else if (monsterTotalHp > monsterHp)
                    {
                        hpImage1.fillAmount = ((monsterHp1 - 80.0f) / (monsterMaxHp / 3));
                        monsterHp1 -= 80.0f;
                    }
                    else if (monsterTotalHp >= 0)
                    {
                        hpImage.fillAmount = ((monsterHp - 80.0f) / (monsterMaxHp / 3));
                        monsterHp -= 80.0f;
                    }
                    //hpImage.fillAmount = ((monsterHp - 25.0f) / monsterMaxHp);
                    monsterTotalHp -= 80.0f;
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

    private float EEdamageCul(float damage)
    {
        return 1.0f;
    }
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        ////Gizmos.DrawLine(this.transform.position, Target.transform.position);
        ////Gizmos.color = Color.blue;
        ////Gizmos.DrawWireSphere(this.transform.position, DisForActivate);
        ////Gizmos.color = Color.red;
        ////Gizmos.DrawWireSphere(this.transform.position, DisForATTACK);
        ////Gizmos.color = Color.cyan;
        ////Gizmos.DrawWireSphere(this.transform.position, DisForRockShoot);
        ////Gizmos.DrawWireSphere(this.transform.position, DisForCHASE);
        //Gizmos.DrawWireSphere(HitBox1, 1.2f);
        //Gizmos.DrawWireSphere(HitBox2.transform.position, 1);
        //Gizmos.DrawWireSphere(HitBox3.transform.position, 1);
        ////Debug.DrawRay(HitBox1, Target.transform.position - HitBox1 , Color.blue , 2 , false);
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawLine(HitBox1, HitBox1 +(Target.transform.position - HitBox1).normalized * 3);
        //Gizmos.DrawLine(HitBox2.transform.position, HitBox2.transform.position + (Target.transform.position - HitBox2.transform.position).normalized * 3) ;
        //Gizmos.DrawLine(HitBox3.transform.position, HitBox3.transform.position + (Target.transform.position - HitBox3.transform.position).normalized * 3) ;
    }

    private void Update()
    {
        //Damage
        if (zAttack != 0 || skillAttack != 0)
        {
            PlayerAttack(zAttack, skillAttack , HitBox1);
            PlayerAttack(zAttack, skillAttack, HitBox2.transform.position);
            PlayerAttack(zAttack, skillAttack, HitBox3.transform.position);
            getHurt = false;
        }

        if(monsterTotalHp < 0 && Alife)
        {
            hpImage.fillAmount = 0;
            mCurrentState = FSMState.Die;
            mCheckState = CheckDieState;
            mDoState = DoDieState;
        }
        //PlayDie
        playerHp = PlayerInfo.playerHp;
        if (playerHp <= 0)
        {
            //Target = ReSetTarget;
            //DisForESCAPE = 0;
            //DisForATTACK = 0;
            //DisForSIGHT = 0;
            //DisForCHASE = 0;
            //TargetInSight = false;
        }
        //HpCharge
        if(bHpCharge)
        {
            HpCharge();
        }
        GetHitBox();

        Debug.Log(hpImage.fillAmount);
        Debug.Log(monsterTotalHp);
        Debug.Log(bHpCharge);
        Debug.Log("目前狀態          " + mCurrentState);
        
        mCheckState();
        //狀態做甚麼
        mDoState();
        //Debug.Log(Time.time - Atk02CDTimer);
    }
}
