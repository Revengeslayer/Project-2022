using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour
{
    private static  GameObject playerHpbar;
    
    private int[] getHit;

    private List<GameObject> dogMonsters; 
    //public Image hpImage;
    static float playerHp;
    float playerDistance;//人物與怪物的距離


    void Start()
    {
        dogMonsters = new List<GameObject>();
        

        playerHpbar = GameObject.Find("PlayerHpBar");
        //foreach (var a in GameObject.FindGameObjectsWithTag("Monster"))
        //{
        //    dogMonsters.Add(a);
        //}
        //getHit = new int[dogMonsters.Count];
    }

    // Update is called once per frame
    void Update()
    {
        
        //for (int a = 0; a < dogMonsters.Count; a++)
        //{
        //    getHit[a] = MonsterDmg.mDogAtk;
        //    MonsterAtack(dogMonsters[a],a);
        //    for (int i = 0; i < 4 ; i++ )
        //    {
        //        Debug.Log(i + "    "+ getHit[i]);
        //    }
        //}
      // Debug.Log("QQQQQQQQQQQQ" + getHit);
    }

    //public void MonsterAtack(GameObject objMonster,int i)
    //{
        //float a;//算角度分子

        //float b;//算角度分母

        //float cosValue;//cos值

        //a = Vector3.Dot((this.transform.position - objMonster.transform.position), objMonster.transform.forward * 2);
        //b = Vector3.Distance(this.transform.position, objMonster.transform.position) * (objMonster.transform.forward * 2).magnitude;
        //cosValue = a / b;
       // Debug.Log("...0.0.0......"+cosValue);

        //if (getHit[i] == 1 && cosValue >0.7f)
        //{           
        //    playerHpbar.GetComponent<Image>().fillAmount = playerHpbar.GetComponent<Image>().fillAmount- 0.0001f;
        //}
        
    //}
    public static void PlayerHpCal()
    {
        playerHpbar.GetComponent<Image>().fillAmount = playerHpbar.GetComponent<Image>().fillAmount - 0.03f;
        if(playerHpbar.GetComponent<Image>().fillAmount < 0.3)
        {
            playerHpbar.GetComponent<Image>().fillAmount = 1;
        }
    }
}
