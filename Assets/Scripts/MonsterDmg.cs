using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDmg : MonoBehaviour
{
    public GameObject monsterHpbar;

    public GameObject swordLight;
    // Start is called before the first frame update
    private int zAttack;
    private int skillAttack;
    static float playerHp;
    public  GameObject objPlayer;
    public  GameObject objMonster;

    public  Image hpImage;
    public Image hpImage0;
    float monsterHp;
    float monsterDistance;//人物與怪物的距離
    bool monsteCollision = true;
    private Animator dogAnimator;
   
    /// <summary>
    /// 普攻計時用
    /// </summary>
    private float nowTimeAtk;
    private bool atkStatus = false;
    private bool atkCd = false;
    
    /// <summary>
    /// 傷害延遲計算
    /// </summary>
    private float nowTimeHurt = 0;
    private bool hertDelay = false;
    private bool hertWait = false;
    /// <summary>
    /// getHit動畫計時用
    /// </summary>
    private float hitAnimTime = 0;
    private bool attackHertGet = false;
    private bool hertAnimDelay = false;
    private bool hertAnimWait = false;
    /// <summary>
    /// 怪物掉落物
    /// </summary>
    public GameObject dropItem;
    /// <summary>
    /// 怪物間的行為- 碰撞重疊避免
    /// </summary>
    private List<GameObject> dogMonsters;
    private bool monsterCollision;

    void Start()
    {
        monsterHp = 300.0f;
        objPlayer = GameObject.Find("Character(Clone)");
        objMonster = this.gameObject;
        dogAnimator = gameObject.GetComponent<Animator>();
        
        
        dogMonsters = new List<GameObject>();
        foreach (var a in GameObject.FindGameObjectsWithTag("Monster"))
        {
            dogMonsters.Add(a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerHp = PlayerInfo.playerHp;//紀錄玩家血量

        zAttack = PlayerInfo.zAttack;
        skillAttack = PlayerInfo.skillAttack;

        monsterDistance = Vector3.Distance(objMonster.transform.position, objPlayer.transform.position);

        if (hpImage.fillAmount <= 0)
        {
            hpImage0.fillAmount = 0;
        }
        #region 先不用
        //MonsterDis(); 
        //if (monsterDistance <= 6.0f && hpImage.fillAmount > 0 && playerHp >0)
        //{
        //    //MonsteCollision();
        //    gameObject.transform.LookAt(objPlayer.transform.position);//面向主角

        //    if (monsterDistance >= 2.0f && monsteCollision)
        //    {  
        //        gameObject.transform.position += gameObject.transform.forward * 3.0f * Time.deltaTime;                  
        //        dogAnimator.SetBool("Attack01", false);
        //        dogAnimator.SetBool("chase", true);
        //        nowTimeAtk = Time.time;
        //    }
        //    else if (monsterDistance < 3.0f)
        //    {   
        //        //攻擊間隔判斷
        //        if (atkStatus == false)
        //        {
        //            dogAnimator.SetBool("Attack01", false);
        //            nowTimeAtk = Time.time;
        //            atkStatus = true;              
        //        }
        //        atkCd = Timer(1.4f,nowTimeAtk);
        //        //攻擊間隔判斷

        //        dogAnimator.SetBool("chase", false);
        //        if (atkCd)
        //        {
        //            gameObject.transform.position += gameObject.transform.forward * 2.0f * Time.deltaTime; //攻擊往前移動
        //            dogAnimator.SetBool("Attack01", true);


        //            //讓傷害延遲計算 與動畫動作合拍
        //            if (hertDelay == false)
        //            {
        //                nowTimeHurt = Time.time;
        //                hertDelay = true;
        //            }
        //            //讓傷害延遲計算 與動畫動作合拍

        //            atkStatus = false;
        //        }

        //        //讓傷害延遲計算 與動畫動作合拍
        //        if (nowTimeHurt != 0) //判斷有沒有獲取到nowTimeHurt的值
        //        {
        //            hertWait = Timer(0.3f, nowTimeHurt);
        //        }

        //        if (Time.time-nowTimeHurt > 0.4f)
        //        {
        //            hertWait = false;
        //            hertDelay = false;
        //        }

        //        if (hertWait)
        //        {

        //            PlayerInfo.PlayerHpCal(1);
        //            hertDelay = false;
        //            hertWait = false;
        //            nowTimeHurt = 0;
        //        }
        //        //讓傷害延遲計算 與動畫動作合拍

        //    }
        //}
        //else if(monsterDistance >= 6.0f && hpImage.fillAmount > 0)
        //{
        //    dogAnimator.SetBool("chase", false);            
        //}
        #endregion
        if (zAttack != 0 || skillAttack != 0)
        {
            attackHertGet = PlayerAttack(monsterDistance);
        }
        //getHit動畫停止判斷
        if(attackHertGet)
        {
            hitAnimTime = Time.time;
            attackHertGet = false;
            hertAnimDelay = true;
        }

        if (hertAnimDelay)
        {
            hertAnimWait = Timer(0.5f, hitAnimTime);
        }

        if (hertAnimWait)
        {
            dogAnimator.SetBool("gethit", false);
            hertAnimDelay = false;
            hertAnimWait = false;
        }
       

       
        //getHit動畫停止判斷

        monsterHpbar.transform.forward = GameObject.Find("Main Camera").transform.forward * -1; //怪物Hp條面向攝影機
        if(hpImage.fillAmount<=0)
        {
            StartCoroutine(Die());         
        }
    }

    public bool PlayerAttack(float monsterDistance)
    {
        
        float a;//算角度分子

        float b;//算角度分母

        float cosValue;//cos值

        a = Vector3.Dot((objMonster.transform.position - objPlayer.transform.position), objPlayer.transform.forward * 2);
        b = Vector3.Distance(objMonster.transform.position, objPlayer.transform.position) * (objPlayer.transform.forward * 2).magnitude;
        cosValue = a / b;

        dogAnimator.SetBool("gethit", false);
        if (zAttack == 1)
        {
            
            if (cosValue >= 0.5 && monsterDistance <= 3.0f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (20.0f/ monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("造成傷害 20");
                return true;
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移
            }
            else //if (cosValue < 0.7 || monsterDistance > 2.3f)
            {
                return false;
            }
        }
        
        else if (zAttack == 2)
        {        
            if (cosValue >= 0.5 && monsterDistance <= 3.0f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (40.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("造成傷害 40");
                return true;
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移
            }
            else //if (monsterDistance > 2.3f)
            {
                return false;
            }
        }

        else if (zAttack == 3)
        {
            if (cosValue >= 0.6 && monsterDistance <= 3.0f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (60.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("造成傷害 60");
                return true;
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移
            }
            else //if (monsterDistance > 2.3f)
            {
                return false;
            }
        }

        //人物技能X 傷害第一段
        else if (skillAttack == 1)
        {
            if (cosValue >= 0.8f && monsterDistance <= 2.8f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (40.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("造成傷害 40");
                return true;
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移
            }
            else //if (monsterDistance > 2.3f)
            {
                return false;
            }
            
        }

        //人物技能X 傷害第二段
        else if (skillAttack == 2)
        {
            //前方一段距離的圓傷害判定用
            //Vector3 playrerAtkPosition;
            //float dogMonsterkDistance;

            //playrerAtkPosition = objPlayer.transform.position + objPlayer.transform.forward * 1.0f;
            //dogMonsterkDistance = Vector3.Distance(playrerAtkPosition, objMonster.transform.position);
            //前方一段距離的圓傷害判定用

            if (cosValue >= 0.7f && monsterDistance <= 3.5f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (60.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("造成傷害 40");
                return true;
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移
            }
            else //if (monsterDistance > 2.3f)
            {
                return false;
            }

        }
        else if (skillAttack == 3)
        {
            if (monsterDistance <= 2.3f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (20.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("造成傷害 20");
                return true;
            }
            else //if (cosValue < 0.7 || monsterDistance > 2.3f)
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }

    public static bool Timer(float cdTime,float nowTime)
    {
         
        if(Time.time-nowTime >= cdTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Die觸發的相關函式
    IEnumerator Die()
    {     
        dogAnimator.Play("Die");
        yield return new WaitForSeconds(2.0f);

        //掉落道具為怪物位置
        Vector3 itemPosition = this.transform.position;
        itemPosition.y -= 0.5f;
        //itemPosition += new Vector3(Random.Range(-2, 2),0.2f, Random.Range(-2, 2));

        Instantiate(dropItem, itemPosition, dropItem.transform.rotation);
        Destroy(this.gameObject);
    }

    //public void MonsteCollision()
    //{
    //    float a;//算角度分子

    //    float b;//算角度分母

    //    float cosValue;//cos值

    //    float monsterDis;
    //    float forwardDis;

    //    for (int i = 0; i < dogMonsters.Count; i++)
    //    {
    //        a = Vector3.Dot((dogMonsters[i].transform.position - this.transform.position), this.transform.forward);
    //        b = Vector3.Distance(dogMonsters[i].transform.position, this.transform.position) * (this.transform.forward).magnitude;
    //        cosValue = a / b;
    //        monsterDis = Vector3.Distance(this.transform.position, dogMonsters[i].transform.position);
    //        forwardDis = monsterDis * cosValue;
    //        if (Vector3.Distance(this.transform.position+this.transform.forward*1, dogMonsters[i].transform.position) < 2.0f && Vector3.Distance(this.transform.position, dogMonsters[i].transform.position) != 0
    //            /*&& Vector3.Dot((dogMonsters[i].transform.position - this.transform.position), this.transform.forward) > 0 && Mathf.Sqrt(monsterDis * monsterDis - forwardDis * forwardDis) < 1.5f*/)
    //        {
    //            monsteCollision = false;
    //        }
    //        else
    //        { 
    //            monsteCollision = true; 
    //        }
    //        //else
    //        //{
    //        //    gameObject.transform.LookAt(objPlayer.transform.position);//面向主角
    //        //}
    //    }
    //}


    void DogNAtkLightOpen()
    {
        swordLight.SetActive(true);
    }

    void DogNAtkLightClose()
    {
        swordLight.SetActive(false);
    }
    //private void OnDrawGizmos()
    //{
    //    Vector3 playrerAtkPosition;
    //    float dogMonsterkDistance;

    //    playrerAtkPosition = objPlayer.transform.position + objPlayer.transform.forward * 2.0f;
    //    dogMonsterkDistance = Vector3.Distance(playrerAtkPosition, objMonster.transform.position);

    //    Gizmos.color = Color.blue;

    //    Gizmos.DrawWireSphere(playrerAtkPosition, 1.0f);
    //}

}
