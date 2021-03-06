using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogFSM : MonoBehaviour
{
    //HP條
    public GameObject monsterHpbar;
    public Image hpImage;
    public Image hpImage0;
    float monsterHp;
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
    //CanAttack
    private bool canAttack;
    //AttackIdle Waittime
    private float attackIdleTime;
    //Atk1 時機
    private bool atk1;
    //Atk2 時機
    private bool atk2;
    //playerAtk
    public static float zAttack;
    //playerSAtk
    public static float skillAttack;
    [Header("次數內霸體")]
    public int canHitCount;
    //GetHit
    private bool getHit;
    //Monster Alive
    private bool monsterAlive;
    // 怪物掉落物
    public GameObject dropItem;

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
        monsterAlive = true;
        monsterHp = 300.0f;
        //player =GameObject.Find("Character");
        player=GameObject.Find("Character(Clone)");
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

            float ToPlayerDic = Vector3.Distance(player.transform.position, gameObject.transform.position);
            if(ToPlayerDic<= dogAtkDic)
            {
                lastActTime = Time.time;
                canAttack = true;
            }
            if(PlayerInfo.playerHp<=0)
            {
                RandomAction();
            }
            else if (CanNextActionCheck(attackIdleTime) && canAttack)
            {
                int num = UnityEngine.Random.Range(1, 3);
                if (num == 1)
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Attack1", true);
                    mCurrentState = DogFSMState.Attack01;
                }
                if (num == 2)
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Attack2", true);
                    mCurrentState = DogFSMState.Attack02;
                }
            }
            else if(CanNextActionCheck(attackIdleTime) && (ToPlayerDic>dogAtkDic))
            {
                anim.SetBool("Idle", false);
                is_Running = true;
                anim.SetBool("Chase", true);
                mCurrentState = DogFSMState.Chase;
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
                canAttack = false;
                anim.SetBool("Idle", true);
                mCurrentState = DogFSMState.AttackIdle;
            }
            else
            {
                if(fToPlayerDic<=dogAtkDic && (atk1 || atk2))
                {
                    PlayerInfo.PlayerHpCal(1);
                    //Debug.Log("攻擊到了");
                }
            }
        }
        if (mCurrentState == DogFSMState.Attack02)
        {
            float fToPlayerDic = Vector3.Distance(player.transform.position, gameObject.transform.position);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                //Debug.Log("攻擊完畢");
                anim.SetBool("Attack2", false);
                attackIdleTime = Time.time;
                canAttack = false;
                anim.SetBool("Idle", true);
                mCurrentState = DogFSMState.AttackIdle;
            }
            else
            {
                if (fToPlayerDic <= dogAtkDic && (atk1 || atk2))
                {
                    PlayerInfo.PlayerHpCal(1);
                    //Debug.Log("攻擊到了");
                }
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
            canAttack = false;
            atk1 = false;
            atk2 = false;
            is_Running = false;
                      

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit")&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime>1)
            {
                Debug.Log("受傷播完");
                attackIdleTime = Time.time;
                getHit = false;
                anim.SetBool("Gethit", false);
                mCurrentState = DogFSMState.AttackIdle;
            }
        }
        if (mCurrentState == DogFSMState.Die)
        {
            canAttack = false;
            atk1 = false;
            atk2 = false;
            is_Running = false;
            getHit = false;
            anim.SetBool("Gethit", false);
            StartCoroutine(Die());
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
            canAttack = true;
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
        Debug.Log(mCurrentState);
        Debug.Log("getHit                    "+getHit);
        monsterHpbar.transform.forward = GameObject.Find("Main Camera").transform.forward * -1; //怪物Hp條面向攝影機
        CheckDie();
        CheckHit();
        PlayerAttack(zAttack, skillAttack);
        CheckHpbar(0);
        CheckNowState();
        
    }

    private void CheckHit()
    {
        if(getHit && monsterAlive)
        {
            anim.SetBool("Wander", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Chase", false);
            anim.SetBool("Attack1", false);
            anim.SetBool("Attack2", false);
            mCurrentState = DogFSMState.GetHit;
            getHit = false;
        }
    }

    private void CheckDie()
    {
        if (hpImage.fillAmount <= 0)
        {
            hpImage0.fillAmount = 0;
            monsterAlive = false;
            mCurrentState = DogFSMState.Die;
        }
    }
    private void CheckHpbar(float damage)
    {
        if (hpImage.fillAmount <= 0)
        {
            hpImage0.fillAmount = 0;
            monsterAlive = false;
        }
        else
        {
            hpImage.fillAmount = hpImage.fillAmount - (damage / monsterHp);
        }
    }
    #region Animation
    private void Attack1()
    {
        if(atk1==false)
        {
            atk1 = true;
        }
        else if(atk1==true)
        {
            atk1 = false;
        }
    }

    private void Attack2()
    {
        if (atk2 == false)
        {
            atk2 = true;
        }
        else if (atk2 == true)
        {
            atk2 = false;
        }
    }
    #endregion

    void PlayerAttack(float zAttack,float skillAttack)
    {
        //Debug.Log(zAttack);
        //Debug.Log(skillAttack);
        float a;//算角度分子
        float b;//算角度分母
        float cosValue;//cos值
        float ToPlayerDic = Vector3.Distance(gameObject.transform.position, player.transform.position);

        a = Vector3.Dot((gameObject.transform.position - player.transform.position), player.transform.forward * 2);
        b = Vector3.Distance(gameObject.transform.position, player.transform.position) * (player.transform.forward * 2).magnitude;
        cosValue = a / b;

        if (zAttack == 1 && cosValue >= 0.7 && hpImage.fillAmount > 0 && ToPlayerDic <= 3.0f)
        {
            CheckHpbar(20);            
            anim.SetBool("Gethit", true);
            canHitCount++;
            if (canHitCount % 5 == 1)
            {
                getHit = true;
            }
            zAttack = 0;
        }
        else if (zAttack == 2 && cosValue >= 0.7 && hpImage.fillAmount > 0 && ToPlayerDic  <= 3.0f)
        {
            CheckHpbar(25);            
            anim.SetBool("Gethit", true);
            canHitCount++;
            if (canHitCount % 5 == 1)
            {
                getHit = true;
            }
            zAttack = 0;
        }
        else if ( zAttack == 3&&cosValue >= 0.85  && hpImage.fillAmount > 0 && ToPlayerDic <= 3.0f)
        {
            CheckHpbar(50);           
            anim.SetBool("Gethit", true);
            canHitCount++;
            if (canHitCount % 5 == 1)
            {
                getHit = true;
            }
            zAttack = 0;
        }

        if (skillAttack == 1)
        {
            if (cosValue >= 0.8f && hpImage.fillAmount > 0 && ToPlayerDic <= 2.8f)
            {
                CheckHpbar(40);
                
                anim.SetBool("Gethit", true);
                canHitCount++;
                if (canHitCount % 5 == 1)
                {
                    getHit = true;
                }
                skillAttack = 0;
            }
        }
        else if (skillAttack == 2)
        {
            if (cosValue >= 0.7f && hpImage.fillAmount > 0 && ToPlayerDic <= 3.5f)
            {
                CheckHpbar(60);               
                anim.SetBool("Gethit", true);
                canHitCount++;
                if (canHitCount % 5 == 1)
                {
                    getHit = true;
                }
                skillAttack = 0;
            }
        }
        else if (skillAttack == 3)
        {
            if (hpImage.fillAmount > 0 && ToPlayerDic <= 2.3f)
            {
                CheckHpbar(20);               
                anim.SetBool("Gethit", true);
                canHitCount++;
                if (canHitCount % 5 == 1)
                {
                    getHit = true;
                }
                skillAttack = 0;
            }
        }
    }

    IEnumerator Die()
    {
        anim.Play("Die");
        yield return new WaitForSeconds(2.0f);

        //掉落道具為怪物位置
        Vector3 itemPosition = this.transform.position;
        itemPosition.y -= 0.5f;
        //itemPosition += new Vector3(Random.Range(-2, 2),0.2f, Random.Range(-2, 2));

        Instantiate(dropItem, itemPosition, dropItem.transform.rotation);
        Destroy(this.gameObject);
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
