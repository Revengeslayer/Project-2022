using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDmg : MonoBehaviour
{
    public GameObject monsterHpbar;
    // Start is called before the first frame update
    private  int zAtack;


    public  GameObject objPlayer;
    public  GameObject objMonster;

    public  Image hpImage;
    float monsterHp;
    float monsterDistance;//人物與怪物的距離

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
    // private bool  

    /// <summary>
    /// getHit動畫計時用
    /// </summary>
    private float hitAnimTime = 0;
    private bool hertAnimDelay = false;
    private bool hertAnimWait = false;

    void Start()
    {
        monsterHp = 300.0f;
        objPlayer = GameObject.Find("Character(Clone)");
        objMonster = this.gameObject;
        dogAnimator = gameObject.GetComponent<Animator>();
    //Debug.Log(objPlayer.transform.position);
}

    // Update is called once per frame
    void Update()
    {
        
        zAtack = Main.zAtack;
       

        monsterDistance = Vector3.Distance(objMonster.transform.position, objPlayer.transform.position);

        
        if (monsterDistance <= 6.0f && hpImage.fillAmount > 0)
        {
            //攻擊間隔判斷
            if (atkStatus == false)
            {
                dogAnimator.SetBool("Attack01", false);
                nowTimeAtk = Time.time;
                atkStatus = true;              
            }
            atkCd = Timer(2.0f,nowTimeAtk);
            //攻擊間隔判斷

            gameObject.transform.LookAt(objPlayer.transform.position);//面向主角

            if (monsterDistance >= 2.0f)
            {  
                gameObject.transform.position += gameObject.transform.forward * 3.0f * Time.deltaTime;                  
                dogAnimator.SetBool("Attack01", false);
                dogAnimator.SetBool("chase", true);
            }
            else if (monsterDistance < 2.0f)
            {   
                dogAnimator.SetBool("chase", false);
                if (atkCd)
                {
                    gameObject.transform.position += gameObject.transform.forward * 2.0f * Time.deltaTime; //攻擊往前移動
                    dogAnimator.SetBool("Attack01", true);

                    //讓傷害延遲計算 與動畫動作合拍
                    if (hertDelay == false)
                    {
                        nowTimeHurt = Time.time;
                        hertDelay = true;
                    }
                    //讓傷害延遲計算 與動畫動作合拍

                    atkStatus = false;
                }

                //讓傷害延遲計算 與動畫動作合拍
                if (nowTimeHurt != 0) //判斷有沒有獲取到nowTimeHurt的值
                {
                    hertWait = Timer(0.3f, nowTimeHurt);
                }
                if (hertWait)
                {
                    PlayerInfo.PlayerHpCal();
                    hertDelay = false;
                    hertWait = false;
                    nowTimeHurt = 0;
                }
                //讓傷害延遲計算 與動畫動作合拍
               
            }
        }
        else if(monsterDistance >= 6.0f && hpImage.fillAmount > 0)
        {
            dogAnimator.SetBool("chase", false);            
        }

        if (zAtack != 0)
        {
            PlayerAttack(monsterDistance);
        }

        monsterHpbar.transform.forward = GameObject.Find("Main Camera").transform.forward * -1; //怪物Hp條面向攝影機
        if(hpImage.fillAmount<=0)
        {
            dogAnimator.Play("Die");
        }
    }

    public void PlayerAttack(float monsterDistance)
    {
        
        float a;//算角度分子

        float b;//算角度分母

        float cosValue;//cos值

        a = Vector3.Dot((objMonster.transform.position - objPlayer.transform.position), objPlayer.transform.forward * 2);
        b = Vector3.Distance(objMonster.transform.position, objPlayer.transform.position) * (objPlayer.transform.forward * 2).magnitude;
        cosValue = a / b;

        dogAnimator.SetBool("gethit", false);
        if (zAtack == 1)
        {
            
            if (cosValue >= 0.7 && monsterDistance <= 2.3f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (40.0f/ monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("造成傷害 40");
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移
            }
            //else if (cosValue < 0.7 || monsterDistance > 2.3f)
            //{
            //    Debug.Log("沒打到");
            //    dogAnimator.SetBool("gethit", false);
            //}
        }
        else if (zAtack == 2)
        {
          
            if (cosValue >= 0.7 && monsterDistance <= 2.3f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (60.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("造成傷害 60");
            }
            //else if (cosValue < 0.7 || monsterDistance > 2.3f)
            //{
            //    Debug.Log("沒打到");
            //    dogAnimator.SetBool("gethit", false);
            //}
        }
        else if (zAtack == 3)
        {
           
            if (monsterDistance <= 2.3f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (20.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("造成傷害 20");
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移
            }
            //else if (monsterDistance > 2.3f)
            //{
            //    Debug.Log("沒打到");
            //    dogAnimator.SetBool("gethit", false);
            //}
        }
        if(hpImage.fillAmount<0.3)
        {
            hpImage.fillAmount = 1;
        }
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

}
