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


        if (monsterDistance <= 5.0f && hpImage.fillAmount > 0)
        {
            if (monsterDistance >= 2.0f)
            {
                dogAnimator.SetBool("chase", true);
                gameObject.transform.LookAt(objPlayer.transform.position);
                gameObject.transform.position += gameObject.transform.forward * 2.0f * Time.deltaTime;
            }
        }
        else
        {
            dogAnimator.SetBool("chase", false);
        }


        if (zAtack != 0)
        {
            PlayerAttack(monsterDistance);
        }
        monsterHpbar.transform.forward = GameObject.Find("Main Camera").transform.forward * -1;
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

        if (zAtack == 1)
        {
            if (cosValue >= 0.7 && monsterDistance <= 2.0f)
            {
                hpImage.fillAmount = hpImage.fillAmount - (40.0f/ monsterHp);
                dogAnimator.Play("GetHit");
                Debug.Log("造成傷害 40");
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移
            }
            else if (cosValue < 0.7 || monsterDistance > 2.0f)
            {
                Debug.Log("沒打到");
            }
        }
        else if (zAtack == 2)
        {
            if (cosValue >= 0.7 && monsterDistance <= 2.0f)
            {
                hpImage.fillAmount = hpImage.fillAmount - (60.0f / monsterHp);
                dogAnimator.Play("GetHit");
                Debug.Log("造成傷害 60");
            }
            else if (cosValue < 0.7 || monsterDistance > 2.0f)
            {
                Debug.Log("沒打到");
            }
        }
        else if (zAtack == 3)
        {
            if (monsterDistance <= 2.0f)
            {
                hpImage.fillAmount = hpImage.fillAmount - (20.0f / monsterHp);
                dogAnimator.Play("GetHit");
                Debug.Log("造成傷害 20");
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //受擊位移
            }
            else if (monsterDistance > 2.0f)
            {
                Debug.Log("沒打到");
            }
        }
    }
}
