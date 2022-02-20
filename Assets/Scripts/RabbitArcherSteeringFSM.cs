using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject Target;
    
    
    private FSMState mCurrentState;

    //Carrot
    private List<CarrotArrow> CarrotContainer;
    public static GameObject Carrot;
    private Vector3 CarrotTargetPos;
    private Vector3 CarrotVec;

    //Distance
    private float DisForSIGHT;
    private float DisForCHASE;
    private float DisForATTACK;
    private float DisForESCAPE;

    //Timer
    private float BattleActionTimer;
    private float AttackTimer;
    private float IdleTimer;
    private float EscTimer;
    //Bool
    private bool NextActionInBattle;
    private bool NextAttack;
    private bool TargetInSight;
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
        mCurrentState = FSMState.Spawn;
        mCheckState = CheckSpawnState;
        mDoState = DoSpawnState;

        CarrotContainer = new List<CarrotArrow>();
        //Carrot = 
            InsCarrot();


        Target = GameObject.Find("Character(Clone)");

        //Timer
        BattleActionTimer = 0;
        IdleTimer = 0;
        EscTimer = 2;

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

        if(TargetInSight)  //CheckTargetInSight bool
        {
            rabaAnim.SetBool("isIDLE", false);

            mCurrentState = FSMState.BattleIdle;
            mCheckState = CheckBattleIdleState;
            mDoState = DoBattleIdleState;
        }
    }
    private void CheckBattleIdleState()
    {

        CheckBattleAction();   //set bool by timer to control Action CD
        CheckNextAttackAction();   //NextAttack CD
        CheckTargetDist();  
        CheckIDLE();   //to check if over N seconds noAction in BattleIdle
        CheckMOVE();
        CheckATTACK();  //bool canAttack from CheckDist > get bool toATTACK
        CheckESC();


        if (NextAttack && toATTACK && !doESC) //Timer & Disance Bool  if Dis < ESC then (!toATTACK)
        {
            rabaAnim.SetBool("isATTACK", true);

            mCurrentState = FSMState.Attack;
            mCheckState = CheckAttackState;
            mDoState = DoAttackState;
            BattleActionTimer = 0;
        }
        if (toESC && NextActionInBattle && doESC) 
        {
            rabaAnim.SetBool("isMOVE", true);

            mCurrentState = FSMState.Move;
            mCheckState = CheckMoveState;
            mDoState = DoMoveState;
        }

         
        else if(toIDLE) //CheckIDLE bool
        {
            rabaAnim.SetBool("isIDLE", true);
            int IDLEnum = Random.Range(2, 4);
            rabaAnim.SetInteger("RanIDLE", IDLEnum);

            mCurrentState = FSMState.Idle;
            mCheckState = CheckIdleState;
            mDoState = DoIdleState;
            toIDLE = false;

            Debug.Log(IDLEnum);
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

        if (!toMOVE)
        {
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

    }
    private void CheckKnockBackState()
    {

    }
    private void CheckSideJumpState()
    {

    }
    private void CheckDamageState()
    {

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
        gameObject.transform.LookAt(Target.transform.position);
    }

    private void DoWanderState()
    {

    }
    private void DoMoveState()
    {
        var Vec = Target.transform.position - gameObject.transform.position;
        
        if(toESC)  //fix : esc for N sceonds && use updated forward && normalized speed
        {
            CheckESC();
            gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, -Vec, 0.95f);
            gameObject.transform.position += gameObject.transform.forward * Time.deltaTime * 3 ;
            toESC = false;
            toMOVE = false;
            //Debug.Log("ESC");
        }
        if(toCHASE)
        {
            gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, Vec, 0.2f);
            gameObject.transform.position += gameObject.transform.forward * Time.deltaTime  * 2;
            toCHASE = false;
            toMOVE =false;
            //Debug.Log("CHASE");
        }
    }
    private void DoAttackState()
    {
        var CarrotVec = Vector3.Normalize( Target.transform.position - gameObject.transform.position );
        foreach(CarrotArrow ca in CarrotContainer)  //get invisible
        {

            if(ca.CarrotVisible)
            {
                return;
            }
            else
            {
                Carrot = ca.Carrot;

            }
        }
        if (rabaAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            Carrot.transform.position = gameObject.transform.position + (new Vector3(0,0.45f,0) + gameObject.transform.forward);
            Carrot.transform.forward = -CarrotVec;
            CarrotTargetPos = Target.transform.position + new Vector3(0, 0.65f, 0);

            Carrot.SetActive(true);
            CarrotVisible = true;

            //Vector3 TargetDist = Target.transform.position - Carrot.transform.position;
            //CarrotVec = (CarrotTargetPos - gameObject.transform.position).normalized;
            //Carrot.transform.position += CarrotVec * Time.deltaTime * 10;
        }
        NextAttack = false;
        AttackTimer = 0;
    }
    private void DoPowerAttackState()
    {

    }
    private void DoKnockBackState()
    {

    }
    private void DoSideJumpState()
    {

    }
    private void DoDamageState()
    {

    }
    private void DoDieState()
    {

    }
    private void DoSpawnState()
    {
        rabaRig.AddForce(new Vector3(0, 600, 0));
    }
    #endregion

    private void CheckNextAttackAction()
    {
        AttackTimer += Time.deltaTime;
        if(AttackTimer > 1.95)
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
        EscTimer += Time.deltaTime;
        if(toESC && EscTimer < 3)
        {
            doESC = true;
        }
        else
        {
            EscTimer = 0;
            doESC = false;
        }
    }
    private void CheckTargetDist()
    {
        //use raySensor to recognize abstract

        var Dist = (Target.transform.position - gameObject.transform.position).magnitude;
        //Debug.Log("距離:"+Dist);

        if(Dist < DisForESCAPE)
        {
            TargetInSight = true;
            rabaAnim.SetBool("inSIGHT", true);
            canATTACK = true;
        }
        else if(Dist <  DisForATTACK)
        {
            TargetInSight = true;
            rabaAnim.SetBool("inSIGHT", true);
            canATTACK = true;
        }
        else if(Dist < DisForSIGHT)
        {
            TargetInSight = true;
            rabaAnim.SetBool("inSIGHT", true);
            canATTACK = false;
        }
        else if(Dist < DisForCHASE)
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
        if(!rabaAnim.GetBool("inSIGHT"))
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

        if (Dist < DisForESCAPE )
        {
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
        if(!canATTACK) //By Distance
        {
            toATTACK = false;
        }
        else if(canATTACK) //By Distance
        {
            toATTACK = true;
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

        //return carrot1;
    }
    private void CarrotController()
    {
        Vector3 CurrentPos;
        //Vector3 TargetDist = Target.transform.position - Carrot.transform.position;
        var CarrotVec = (CarrotTargetPos - gameObject.transform.position).normalized;
        Carrot.transform.position += CarrotVec * Time.deltaTime * 10;
        //var CVTDdot = Vector3.Dot(CarrotVec, TargetDist);
        //if (CarrotVisible)
        {
            //if (CVTDdot < 0.1f)
            //{
            //    //Carrot.SetActive(false);
            //    //CarrotVisible = false;
            //    //Carrot.transform.position += Vector3.down * Time.deltaTime *5;
            //}
            //Carrot.transform.position += CarrotVec * Time.deltaTime * 10;
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

        mCheckState();
        //狀態做甚麼
        mDoState();
        Debug.Log(CarrotVisible);
        //Debug.Log(AttackTimer); CHECK ATTACK CD
    }
    private void FixedUpdate()
    {
        //CarrotController();
    }
}
