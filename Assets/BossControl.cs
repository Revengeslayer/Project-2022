using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    private GameObject objPlayer;
    private GameObject objBoss;
    private Animator EEAnim;


    public GameObject atkWave;
    public GameObject jumpWave;
    public GameObject awakeDust;
    public GameObject jumpHint;

    private float bossHp;
    private float RollAtkTime;
    private float SpellTime;
    private float SpellRate;

    private int bossState;
    private int bossDo;

    bool bossRoll = false;
    bool bossJump = false;
    bool bossTurn = false;

    Vector3 bossJumpVec;
    Vector3 bossAtkPosition0;
    float bossAtkDistance0;
    private void Start()
    {
        EEAnim = GetComponent<Animator>();
        objPlayer = GameObject.Find("Character(Clone)");
        objBoss = this.gameObject;
        bossState = 0;
        bossHp = 1000;
        SpellTime = Time.time;
        SpellRate = 0;

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
        if(bossRoll == true)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 4;
            //BossRollAtk();
        }

        if (bossJump == true)
        {
            //objBoss.transform.LookAt(objPlayer.transform.position);
            //gameObject.transform.forward = bossJumpVec;
            if (EEAnim.GetCurrentAnimatorStateInfo(0).IsName("JumpAtk"))
            {
                this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime *6f ;
            }
            else
            {
                this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 15;
            }
        }

        if(bossTurn == true)
        {
            var a = (objPlayer.transform.position - gameObject.transform.position).normalized;
            //a.y = 0;
            gameObject.transform.forward += a * Time.deltaTime * 5;
        }


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
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.JumpAtk;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Die;
        }

        if (Time.time > SpellTime && EEAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
        {
            EEAnim.speed = 1;
        }
        if (EEAnim.speed <1 && EEAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
        {
            SpellRate += Time.deltaTime;
            if (SpellRate > 0.5f)
            {
                GameObject SpikeIns = Instantiate(Resources.Load("VTX/SpikeIns")) as GameObject;
                SpellRate = 0;
            }
        }
    }

    void BossNormalAtk()
    {
        //Vector3 bossAtkPosition0;
        //float bossAtkDistance0;

        bossAtkPosition0 = objBoss.transform.position + objBoss.transform.forward * 6.0f;
        bossAtkDistance0 = Vector3.Distance(bossAtkPosition0, objPlayer.transform.position);
        if (bossAtkDistance0 < 3.2f)
        {
            PlayerInfo.PlayerHpCal(10);
        }
        else
        {
            Debug.Log("Boss§ðÀ»½d³ò¥~");
        }   
    }

    void BossNAtkWaveOpen()
    {
        atkWave.SetActive(true);
    }

    void BossNAtkWaveClose()
    {
        atkWave.SetActive(false);
    }

    void BossJumpWaveOpen()
    {
        jumpWave.SetActive(true);
    }

    void BossJumpWaveClose()
    {
        jumpWave.SetActive(false);
    }

    void BossAwakeDustOpen()
    {
        awakeDust.SetActive(true);
    }

    void BossAwakeDustClose()
    {
        awakeDust.SetActive(false);
    }

    void BossJumpHintOpen()
    {
        jumpHint.SetActive(true);
    }

    void BossJumpHintClose()
    {
        jumpHint.SetActive(false);
    }
    void BossJumpDamage()
    {
        Vector3 bossAtkPosition0;
        float bossAtkDistance0;

        bossAtkPosition0 = objBoss.transform.position;
        bossAtkDistance0 = Vector3.Distance(bossAtkPosition0, objPlayer.transform.position);
        if (bossAtkDistance0 < 6.0f)
        {
            PlayerInfo.PlayerHpCal(12);
        }
        else
        {
            Debug.Log("Boss§ðÀ»½d³ò¥~");
        }
    }

    void BossJumpMove()
    {
        if (bossJump == false)
        {
            bossJump = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            bossJumpVec = (objPlayer.transform.position - gameObject.transform.position).normalized;
        }
        else if (bossJump == true)
        {
            bossJump = false;
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
    void BossJumpTurn()
    {
        if(bossTurn == false)
        {
            bossTurn = true;
        }
        else if(bossTurn == true)
        {
            bossTurn = false;
        }
    }
    //void BossLeftAtk()
    //{
    //    Vector3 bossAtkPosition0;
    //    float bossAtkDistance0;
    //    bossAtkPosition0 = objBoss.transform.position + objBoss.transform.forward * 5.0f; 
    //    bossAtkDistance0 = Vector3.Distance(bossAtkPosition0, objPlayer.transform.position);
    //    if (bossAtkDistance0 < 2.0f)
    //    {
    //        PlayerInfo.PlayerHpCal(20);
    //    }
    //    else
    //    {
    //        Debug.Log("Boss§ðÀ»½d³ò¥~");
    //    }
    //}
    void BossRollAtk()
    {
        //objBoss.transform.LookAt(objPlayer.transform.position);
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
        if (bossAtkDistance< 0)
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
        if (bossRoll == false)
        {
            bossRoll = true;
        }
        else if (bossRoll == true)
        {
            bossRoll = false;
        }               
    }
    void WaitForSpell()
    {
        EEAnim.speed = 0;
        SpellTime  = Time.time + 3;
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


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(bossAtkPosition0, 3.2f);
    //    Gizmos.DrawWireSphere(objBoss.transform.position, 6.0f);
    }
}
