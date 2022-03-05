using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedPotionText : MonoBehaviour
{
    public static int Nums;
    private float playerHp;
    private float playerMaxHp;
    // Start is called before the first frame update
    void Start()
    {
        Nums = 2;
        GetComponent<Text>().text = "x " + Nums;
        playerHp = PlayerInfo.playerHp;
        playerMaxHp = PlayerInfo.playerMaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        playerHp = PlayerInfo.playerHp;
        GetComponent<Text>().text = "x " + Nums;
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(playerHp <= 0 || playerHp >= playerMaxHp)
            {
                return;
            }
            if(Nums > 0 )
            {
                PlayerInfo.playerHp += 300;
                Nums--;
            }
        }
    }
}
