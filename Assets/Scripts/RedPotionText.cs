using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedPotionText : MonoBehaviour
{
    public static int Nums;
    public static int currentNums;
    private GameObject[] redPotions;
    private float playerHp;
    private float playerMaxHp;
    AudioSource[] Audios;
    AudioSource Heal;
    public GameObject HealEffect;
    // Start is called before the first frame update

    private void Awake()
    {
        Audios = gameObject.GetComponents<AudioSource>();
        Heal = Audios[10];
        redPotions = GameObject.FindGameObjectsWithTag("redPotion");
        redPotions[0] = GameObject.Find("redPotion");
        redPotions[1] = GameObject.Find("redPotion (1)");
        redPotions[2] = GameObject.Find("redPotion (2)");
        redPotions[3] = GameObject.Find("redPotion (3)");
        redPotions[4] = GameObject.Find("redPotion (4)");
        redPotions[5] = GameObject.Find("redPotion (5)");
        redPotions[6] = GameObject.Find("redPotion (6)");
        redPotions[7] = GameObject.Find("redPotion (7)");
        redPotions[8] = GameObject.Find("redPotion (8)");
        redPotions[9] = GameObject.Find("redPotion (9)");
        redPotions[10] = GameObject.Find("redPotion (10)");
        redPotions[11] = GameObject.Find("redPotion (11)");
        redPotions[12] = GameObject.Find("redPotion (12)");
        redPotions[13] = GameObject.Find("redPotion (13)");

    }
    void Start()
    {
        //redPotions = GameObject.FindGameObjectsWithTag("redPotion") ;
        Nums = redPotions.Length;
        currentNums = 2;
        SetCurrentPotions();
        playerHp = PlayerInfo.playerHp;
        playerMaxHp = PlayerInfo.playerMaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        playerHp = PlayerInfo.playerHp;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (playerHp <= 0 || playerHp >= playerMaxHp)
            {
                return;
            }
            if (currentNums > 0)
            {
                HealEffect.SetActive(true);
                StartCoroutine(Timer());
                PlayerInfo.playerHp += 300;
                Heal.Play();
                redPotions[currentNums-1].SetActive(false);
                currentNums--;
            }
        }
        ShowCurrentPotions();
    }
    private void SetCurrentPotions()
    {
        for (int i = Nums-1; i >= currentNums; i--)
        {
            redPotions[i].SetActive(false);
        }
    }

    private void ShowCurrentPotions()
    {
        for (int i = 0 ; i < currentNums; i++)
        {
            redPotions[i].SetActive(true);
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.5f);
        HealEffect.SetActive(false);
    }

}