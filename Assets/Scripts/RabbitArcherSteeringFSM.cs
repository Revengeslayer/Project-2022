using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RabbitArcherSteeringFSM : MonoBehaviour
{
    //檢查狀態的delegate
    private delegate void CheckState();
    private CheckState mCheckState;
    //做狀態的delegate
    private delegate void DoState();
    private CheckState mDoState;

    private Animator rabaAnim;
    private Rigidbody rabaRig;
    public GameObject monsterHpbar;
    public Image hpImage;
    public Image hpImage0;
    public GameObject Target;

    //Add
    public static float zAttack;
    public static float skillAttack;
    private float playerHp;
    public static string SpawnArea;
    float monsterHp;
    public GameObject dropItem;



    private FSMState mCurrentState;

    InstantiateManager raManger;

    //Carrot
    private List<CarrotArrow> CarrotContainer;
    public static GameObject Carrot;
    private Vector3 CarrotTargetPos;
    private Vector3 CarrotVecG;

    //Distance
    private float DisToTarget;
    private float DisForSIGHT;
    private float DisForCHASE;
    private float DisForATTACK;
    private float DisForESCAPE;

    //Timer
    private float BattleActionTimer;
    private float AttackTimer;
    private float IdleTimer;
    private float EscTimer;
    private float EscCD;
    private float DamageTimer;
    private float PATimer;
    //Bool
    private bool NextActionInBattle;
    private bool NextAttack;
    private bool TargetInSight;
    private bool Shooted;
    private bool PAshooted;
    private bool canPA;
    private bool getHurt;
    private bool Alife;
    public static bool CarrotVisible;

    private bool toSPAWN;
    private bool toIDLE;
    private bool toMOVE;
    private bool toESC;
    private bool doESC;
    private bool toCHASE;
    private bool toATTACK;
    private bool canATTACK;
    public enum FSMState
    {
        NONE = -1,
        Idle,
        BattleIdle,
        Wander,
        Move,
        Attack,
        PowerAttack,
        KnockBack,
        SideJump,
        Damage,
        Die,
        Spawn
    }

    public class CarrotArrow
    {
        public GameObject Carrot;
        public bool CarrotVisible;
    }

    void Start()
    {
        rabaAnim = GetComponent<Animator>();
        rabaRig = GetComponent<Rigidbody>();
        monsterHp = 200;

        mCurrentState = FSMState.Spawn;
        mCheckState = CheckSpawnState;
        mDoState = DoSpawnState;
        Alife = true;


        //CarrotContainer = new List<CarrotArrow>();

        Target = GameObject.Find("Character(Clone)");

        //Timer
        BattleActionTimer = 0;
        IdleTimer = 0;
        EscTimer = 0;
        EscCD = Time.time;
        DamageTimer = 0;
        PATimer = 0;

        //Dist
        DisForCHASE = 15;
        DisForSIGHT = 11;
        DisForATTACK = 6;
        DisForESCAPE = 3;
    }

    #region TransitionCheck
    private void CheckIdleState()
    {
        CheckTargetDist();  //set bool by distance

        if (TargetInSight)  //CheckTargetInSight bool
        {
            rabaAnim.SetBool("isIDLE", false);

            mCurrentState = FSMState.BattleIdle;
            mCheckState = CheckBattleIdleState;
            mDoState = DoBattleIdleState;
        }
    }
    private void CheckBattleIdleState()
    {
        toMOVE = false;
        CheckBattleAction();   //set bool by timer to control Action CD
        CheckNextAttackAction();   //NextAttack CD
        CheckTargetDist();
        CheckIDLE();   //to check if over N seconds noAction in BattleIdle
        CheckMOVE();
        CheckATTACK();  //bool canAttack from CheckDist > get bool toATTACK
        CheckPowerATTACK();
        CheckESC();

        if (canPA && NextAttack && TargetInSight)  //bool by Timer & Timer & Dist
        {
            rabaAnim.SetBool("isPA", true);

            mCurrentState = FSMState.PowerAttack;
            mCheckState = CheckPowerAttackState;
            mDoState = DoPowerAttackState;
            BattleActionTimer = 0;
        }
        else if (NextActionInBattle && doESC)
        {
            rabaAnim.SetBool("isMOVE", true);

            mCurrentState = FSMState.Move;
            mCheckState = CheckMoveState;
            mDoState = DoMoveState;
        }
        else if (NextAttack && toATTACK && !doESC) //Timer & Disance Bool  if Dis < ESC then (!toATTACK)
        {
            rabaAnim.SetBool("isATTACK", true);

            mCurrentState = FSMState.Attack;
            mCheckState = CheckAttackState;
            mDoState = DoAttackState;
            BattleActionTimer = 0;
        }

        //if (NextActionInBattle && doESC)
        //{
        //    rabaAnim.SetBool("isMOVE", true);

        //    mCurrentState = FSMState.Move;
        //    mCheckState = CheckMoveState;
        //    mDoState = DoMoveState;
        //}


        else if (toIDLE) //CheckIDLE bool
        {
            rabaAnim.SetBool("isIDLE", true);
            int IDLEnum = Random.Range(2, 4);
            rabaAnim.SetInteger("RanIDLE", IDLEnum);

            mCurrentState = FSMState.Idle;
            mCheckState = CheckIdleState;
            mDoState = DoIdleState;
            toIDLE = false;

            //Debug.Log(IDLEnum);
        }
        else if (toMOVE && NextActionInBattle) //Chase
        {
            rabaAnim.SetBool("isMOVE", true);
            mCurrentState = FSMState.Move;
            mCheckState = CheckMoveState;
            mDoState = DoMoveState;
        }
    }
    private void CheckWanderState()
    {

    }
    private void CheckMoveState()
    {
        CheckNextAttackAction(); //ATTACKTimer
        CheckTargetDist();
        CheckMOVE(); //DistanceOnly
        Debug.Log("check");
        if (!toMOVE)
        {
            Debug.Log("check out");
            rabaAnim.SetBool("isMOVE", false);

            mCurrentState = FSMState.BattleIdle;
            mCheckState = CheckBattleIdleState;
            mDoState = DoBattleIdleState;
        }
    }
    private void CheckAttackState()
    {
        CheckTargetDist(); // canATTACK BOOL
        CheckATTACK();  //toATTACK BOOL
        CheckNextAttackAction(); //NextAttack CD

        if (!toATTACK || !NextAttack) // Distance || Timer
        {
            rabaAnim.SetBool("isATTACK", false);

            mCurrentState = FSMState.BattleIdle;
            mCheckState = CheckBattleIdleState;
            mDoState = DoBattleIdleState;
        }

    }
    private void CheckPowerAttackState()
    {
        //CheckTargetDist(); // canATTACK BOOL
        //CheckATTACK();  //toATTACK BOOL
        //CheckNextAttackAction(); //NextAttack CD

        if (!canPA) //bool by Timer
        {
            rabaAnim.SetBool("isPA", false);

            mCurrentState = FSMState.BattleIdle;
            mCheckState = CheckBattleIdleState;
            mDoState = DoBattleIdleState;
        }
    }
    private void CheckKnockBackState()
    {

    }
    private void CheckSideJumpState()
    {

    }
    private void CheckDamageState()
    {
        //DamageTimer += Time.time;

        if (rabaAnim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            var CD = Time.time - EscCD;
            if (CD <= 0)
            {
                EscCD += 0.6f;
            }
            rabaAnim.SetBool("isDamage", false);
            mCurrentState = FSMState.BattleIdle;
            mCheckState = CheckBattleIdleState;
            mDoState = DoBattleIdleState;
            getHurt = false;
        }
    }
    private void CheckDieState()
    {

    }
    private void CheckSpawnState()
    {
        rabaRig.AddForce(new Vector3(0, 300, 0));
        if (!toSPAWN)
        {
            int IDLEnum = Random.Range(2, 4);
            rabaAnim.SetInteger("RanIDLE", IDLEnum);

            mCurrentState = FSMState.BattleIdle;
            mCheckState = CheckBattleIdleState;
            mDoState = DoBattleIdleState;
            toIDLE = false;
        }
    }
    #endregion

    #region DoState
    private void DoIdleState()
    {

    }
    private void DoBattleIdleState()
    {
        //gameObject.transform.LookAt(Target.transform.position);
        var CurrentForward = Target.transform.position - gameObject.transform.position;
        CurrentForward.y = 0;
        gameObject.transform.forward = CurrentForward.normalized;
    }

    private void DoWanderState()
    {

    }
    private void DoMoveState()
    {
        var Vec = Target.transform.position - gameObject.transform.position;
        if (doESC)  //fix : esc for N sceonds && use updated forward && normalized speed
        {
            Debug.Log("esc");
            CheckESC();
            gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, -Vec, 0.95f);
            gameObject.transform.position += gameObject.transform.forward * Time.deltaTime * 3;
            BattleActionTimer = 0;
            NextActionInBattle = false;
            //Debug.Log("ESC");
        }
        else if (toCHASE)
        {
            Debug.Log("chase");
            //var turnX = Vector3.Dot(gameObject.transform.forward, Vec);
            //var turnZ = Vector3.Dot(gameObject.transform.right, Vec);
            gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward + (gameObject.transform.right * 0.1f), Vec, 1 * Time.deltaTime );
            gameObject.transform.position += gameObject.transform.forward * Time.deltaTime * 2;
            toCHASE = false;
            toMOVE = false;
            //Debug.Log("CHASE");
        }
    }
    private void DoAttackState()
    {
        //var CarrotVec = Vector3.Normalize( Target.transform.position - gameObject.transform.position );
        //var SpawnPos = gameObject.transform.position + (new Vector3(0, 0.45f, 0) + gameObject.transform.forward);



        //    Carrot.transform.position = gameObject.transform.position + (new Vector3(0,0.45f,0) + gameObject.transform.forward);
        //    Carrot.transform.forward = -CarrotVec;
        //    CarrotTargetPos = Target.transform.position + new Vector3(0, 0.65f, 0);

        Shooted = true;
        NextAttack = false;
        AttackTimer = 0;
    }
    private void DoPowerAttackState()
    {
        PAshooted = true;
        canPA = false;
        NextAttack = false;
        PATimer = 0;
        AttackTimer = 0;
        if(rabaAnim.GetCurrentAnimatorStateInfo(0).IsName("POWERATTACK"))
        {
            getHurt = false;
        }
    }
    private void DoKnockBackState()
    {

    }
    private void DoSideJumpState()
    {

    }
    private void DoDamageState()
    {
        toMOVE = false;
        rabaAnim.SetBool("isMOVE", false);
    }
    private void DoDieState()
    {
        //rabaAnim.SetBool("isDIE", false);
    }
    IEnumerator Die()
    {
        //rabaAnim.Play("Die");
        yield return new WaitForSeconds(1.6f);

        //掉落道具為怪物位置
        //Vector3 itemPosition = this.transform.position;
        //itemPosition += new Vector3(Random.Range(-2, 2), 0.2f, Random.Range(-2, 2));

        //Instantiate(dropItem, itemPosition, dropItem.transform.rotation);
        gameObject.SetActive(false);
        hpImage.fillAmount = 1;
        mCurrentState = FSMState.Spawn;
        mCheckState = CheckSpawnState;
        mDoState = DoSpawnState;
        rabaRig.isKinematic = false;
        Alife = true;
        getHurt = false;

        Target = GameObject.Find("Character(Clone)");

        //Timer
        BattleActionTimer = 0;
        IdleTimer = 0;
        EscTimer = 0;
        EscCD = Time.time;
        DamageTimer = 0;
        PATimer = 0;

        //Dist
        DisForCHASE = 15;
        DisForSIGHT = 11;
        DisForATTACK = 6;
        DisForESCAPE = 3;
        yield break;
    }
    private void DoSpawnState()
    {
        rabaRig.AddForce(new Vector3(0, 600, 0));
    }
    #endregion

    private void CheckNextAttackAction()
    {
        AttackTimer += Time.deltaTime;
        if (AttackTimer > 1.95)
        {
            NextAttack = true;
        }
    }
    private void CheckBattleAction()
    {
        BattleActionTimer += Time.deltaTime;
        if (BattleActionTimer > 2)
        {
            NextActionInBattle = true;
            BattleActionTimer = 0;
        }
        else
        {
            NextActionInBattle = false;
        }
    }

    private void CheckESC()
    {
        if (toESC && EscTimer < 1.5)
        {
            doESC = true;
            toMOVE = true;
            if (rabaAnim.GetCurrentAnimatorStateInfo(0).IsName("MOVE"))
            {
                EscTimer += Time.deltaTime;
            }
        }
        //else if (!toESC && !doESC)
        //{
        //    EscTimer = 0;
        //    doESC = false;
        //}
        else if (doESC && EscTimer > 1.5)
        {
            toESC = false;
            doESC = false;
            toMOVE = false;
            EscTimer = 0;
        }
    }
    private void CheckTargetDist()
    {
        //use raySensor to recognize abstract

        var Dist = (Target.transform.position - gameObject.transform.position).magnitude;
        //Debug.Log("距離:"+Dist);

        if (Dist < DisForESCAPE)
        {
            TargetInSight = true;
            rabaAnim.SetBool("inSIGHT", true);
            canATTACK = true;
        }
        else if (Dist < DisForATTACK)
        {
            TargetInSight = true;
            rabaAnim.SetBool("inSIGHT", true);
            canATTACK = true;
        }
        else if (Dist < DisForSIGHT)
        {
            TargetInSight = true;
            rabaAnim.SetBool("inSIGHT", true);
            canATTACK = false;
        }
        else if (Dist < DisForCHASE)
        {
            TargetInSight = false;
            rabaAnim.SetBool("inSIGHT", false);
            canATTACK = false;
        }
        else
        {
            TargetInSight = false;
            rabaAnim.SetBool("inSIGHT", false);
            canATTACK = false;
        }
    }
    private void CheckIDLE()
    {
        if (!rabaAnim.GetBool("inSIGHT"))
        {
            IdleTimer += Time.deltaTime;
            if (IdleTimer > 4)
            {
                toIDLE = true;
                IdleTimer = 0;
            }
        }
    }

    private void CheckMOVE()
    {
        var Dist = (Target.transform.position - gameObject.transform.position).magnitude;

        if (Dist < DisForESCAPE && Time.time > EscCD)
        {
            EscCD = Time.time + 7;
            toMOVE = true;
            toESC = true;
        }

        else if (Dist > DisForATTACK && Dist < DisForCHASE)
        {
            toMOVE = true;
            toCHASE = true;
        }
        //else
        //{
        //    toMOVE = false;
        //}

        //else if(wander)
    }
    private void CheckATTACK()
    {
        if (!canATTACK) //By Distance
        {
            toATTACK = false;
        }
        else if (canATTACK) //By Distance
        {
            toATTACK = true;
        }
    }
    private void CheckPowerATTACK()
    {
        PATimer += Time.deltaTime;
        if (PATimer > 5)
        {
            canPA = true;
        }
    }

    public void InsCarrot()
    {
        CarrotArrow ca = new CarrotArrow(); //class for GO & bool
        GameObject carrotIns = Instantiate(Resources.Load("Weapons/carrotarrow")) as GameObject;  //Ins

        carrotIns.SetActive(false);
        CarrotVisible = false;

        ca.CarrotVisible = false;
        ca.Carrot = carrotIns;
        CarrotContainer.Add(ca);
    }

    private void PlayerAttack(float zAttack, float skillAttack)
    {
        DisToTarget = (Target.transform.position - gameObject.transform.position).magnitude;
        var a = Vector3.Dot((gameObject.transform.position - Target.transform.position), Target.transform.forward * 2);
        var b = Vector3.Distance(gameObject.transform.position, Target.transform.position) * (Target.transform.forward * 2).magnitude;
        var cosValue = a / b;


        if (zAttack == 1 && cosValue >= 0.7 && hpImage.fillAmount > 0&& DisToTarget <= 3.0f)
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
        else if (cosValue >= 0.85 && zAttack == 3 && hpImage.fillAmount > 0 && DisToTarget <= 3.0f)
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
        else if (cosValue >= 0.7f && hpImage.fillAmount > 0 && DisToTarget <= 3.5f)
        {

            //前方一段距離的圓傷害判定用
            //Vector3 playrerAtkPosition;
            //float dogMonsterkDistance;

            //playrerAtkPosition = objPlayer.transform.position + objPlayer.transform.forward * 1.0f;
            //dogMonsterkDistance = Vector3.Distance(playrerAtkPosition, objMonster.transform.position);
            //前方一段距離的圓傷害判定用

            if (cosValue >= 0.7f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (60.0f / monsterHp);
                getHurt = true;
                skillAttack = 0;
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                //Debug.Log("s2");
                //Debug.Log("造成傷害 40");
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移
            }
        }
        else if (skillAttack == 3 && DisToTarget <= 2.3f)
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

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, DisForESCAPE);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, DisForATTACK);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, DisForSIGHT);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, DisForCHASE);
    }

    void Update()
    {
        Debug.Log("目前狀態          " + mCurrentState);

        //Damage
        //if (DisToTarget < 2.3f)
        //{
        //    PlayerAttack(zAttack, skillAttack);
        //}
        if(zAttack!=0 || skillAttack!=0)
        {
             PlayerAttack(zAttack, skillAttack);
        }

        if (getHurt && Alife && Time.time > DamageTimer && !rabaAnim.GetCurrentAnimatorStateInfo(0).IsName("POWERATTACK"))
        {
            DamageTimer = Time.time + 1;
            rabaAnim.SetBool("isDamage", true);
            mCheckState = CheckDamageState;
            mCurrentState = FSMState.Damage;
            mDoState = DoDamageState;
            getHurt = false;
        }
        else
        {
            getHurt = false;
        }
        //PlayerDie
        playerHp = PlayerInfo.playerHp;
        if (playerHp <= 0)
        {
            //Target = ReSetTarget;
            DisForESCAPE = 0;
            DisForATTACK = 0;
            DisForSIGHT = 0;
            DisForCHASE = 0;
            TargetInSight = false;
        }

        //MonsterDie
        if (hpImage.fillAmount <= 0 && Alife == true)
        {
            Debug.Log("inDie");
            getHurt = false;
            Alife = false;
            rabaAnim.SetBool("isDamage", false);
            rabaAnim.Play("Die");
            rabaRig.isKinematic = true;
            mCheckState = CheckDieState;
            mDoState = DoDieState;
            mCurrentState = FSMState.Die;
            StartCoroutine(Die());
        }

        mCheckState();
        //狀態做甚麼
        mDoState();

        //DisToTarget = (Target.transform.position - gameObject.transform.position).magnitude;
        monsterHpbar.transform.forward = -Camera.main.transform.forward;

        if (Shooted == true)
        {
            if (rabaAnim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK") && rabaAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f)
            {
                var CarrotVec = Vector3.Normalize(Target.transform.position - gameObject.transform.position);
                List<Vector3> TargetVecList = new List<Vector3>();
                TargetVecList.Add(CarrotVec);
                var SpawnPos = gameObject.transform.position + (new Vector3(0, 0.45f, 0) + gameObject.transform.forward * 0.5F);

                CarrotController.InsCarrot(SpawnPos, TargetVecList, "A" , new Vector3(0.5f, 0.5f, 0.5f));
                Shooted = false;
            }
        }
        if (PAshooted == true)
        {
            if (rabaAnim.GetCurrentAnimatorStateInfo(0).IsName("POWERATTACK") && rabaAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.575f)
            {
                List<Vector3> TargetVecList = new List<Vector3>();
                var SpawnPos = gameObject.transform.position + (new Vector3(0, 0.25f, 0) + gameObject.transform.forward * 0.3F);
                for (int i = 0; i < 5; i++)
                {
                    var SectorVec = gameObject.transform.right * -0.5f;
                    var CarrotVec = Vector3.Normalize(Target.transform.position - gameObject.transform.position) + new Vector3(0, 0.08f, 0) + gameObject.transform.right + SectorVec * i;
                    TargetVecList.Add(CarrotVec);
                }
                CarrotController.InsCarrot(SpawnPos, TargetVecList, "B" , new Vector3(0.5f, 0.5f, 0.5f));
                Debug.Log(TargetVecList.Count);
                PAshooted = false;
            }
        }
    }
    private void FixedUpdate()
    {
        //if (getHurt && Alife && Time.time > DamageTimer)
        //{
        //    DamageTimer = Time.time + 1;
        //    rabaAnim.SetBool("isDamage", true);
        //    mCheckState = CheckDamageState;
        //    mCurrentState = FSMState.Damage;
        //    mDoState = DoDamageState;
        //    getHurt = false;
        //}
        //else
        //{
        //    getHurt = false;
        //}
    }
}
