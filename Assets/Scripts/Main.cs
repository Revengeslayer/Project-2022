using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public float speed;
    public static bool canMove;
    bool isAttack;
    bool isJump;
    bool isRun;
    private List<GameObject> monsterPrefabIns;
    private List<GameObject> terrainPrefabIns;
    public static  GameObject player;

    private Animator playerAnimator;
    private Animation playerAnimation;

    private Rigidbody playerRigidbody;
    public float jumpForce = 200;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;

    private float firstClickTime;
    private float secClickTime;
    private float doubleSpacing=0.2f;

    private float atkLastTime;
    private float NextDash;

    public float playerHp; 
    public static int zAtack;

    //public static int reBirth = 0;
    private void Awake()
    {
        canMove = true;
        Terrain();
        //Mobs();
        Player();
        SetCamera();
       


    }
    void Start()
    {
    }
    void Update()
    {
        playerHp = PlayerInfo.playerHp;//紀錄玩家血量

        isAttack = playerAnimator.GetBool("isAttack");
        isJump = playerAnimator.GetBool("isJump");
        isRun = playerAnimator.GetBool("isRun");
        zAtack = 0;
        if (playerHp > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && !isRun)
            {
                playerAnimator.SetBool("isRun", true);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) && isRun)
            {
                playerAnimator.SetBool("isRun", false);
            }
            if (!playerAnimator.GetBool("isWalkF") && !playerAnimator.GetBool("isWalkB") && !playerAnimator.GetBool("isWalkL") && !playerAnimator.GetBool("isWalkR"))
            {
                playerAnimator.SetBool("isRun", false);
            }


            //if (Input.GetKeyDown(KeyCode.Z) && !isJump)
            //{
            //    //playerAnimator.SetTrigger("Attack");
            //    //playerAnimator.SetBool("isAttack", true);

            //    playerAnimator.SetInteger("atkCount", playerAnimator.GetInteger("atkCount") + 1);
            //    if (playerAnimator.GetInteger("atkCount") == 1)
            //    {
            //        //playerAnimator.SetBool("hit1", true);
            //        playerAnimator.Play("Atk1");
            //        //playerAnimator.SetBool("hit2", true);
            //        atkLastTime = Time.time;
            //        Debug.Log(1);

            //        zAtack = 1;
            //    }
            //    if (playerAnimator.GetInteger("atkCount") == 2)
            //    {
            //        if (Time.time - atkLastTime < 1.2f)
            //        {
            //            Debug.Log("聯集");
            //            //playerAnimator.SetBool("hit2", true);
            //            playerAnimator.Play("Atk2");

            //            zAtack = 2;
            //        }
            //        //playerAnimator.SetBool("hit2", true);
            //        //playerAnimator.SetBool("hit1", false);

            //        atkLastTime = Time.time;
            //        Debug.Log(2);
            //    }

            //    if (playerAnimator.GetInteger("atkCount") == 3)
            //    {
            //        //playerAnimator.SetBool("hit3", true);
            //        //playerAnimator.SetBool("hit2", false);
            //        playerAnimator.Play("Atk3");
            //        Debug.Log(3);
            //        playerAnimator.SetInteger("atkCount", 0);

            //        zAtack = 3;
            //    }



            //    playerAnimator.SetInteger("atkCount", Mathf.Clamp(playerAnimator.GetInteger("atkCount"), 0, 3));
            //    //playerAnimator.applyRootMotion = (true);
            //    //player.transform.position += player.transform.forward * Time.deltaTime * speed;
            //}
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Application.LoadLevel(0);
        }

        if (Time.time - atkLastTime > 1.2f)
        {
            playerAnimator.SetInteger("atkCount", 0);
        }
        
        /*
        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
            playerAnimator.SetTrigger("Jump");
            playerRigidbody.AddForce(0, jumpForce, 0);
            canJump = Time.time + timeBeforeNextJump;
            if (Input.GetKeyDown(KeyCode.Z))
            {
                playerAnimator.SetBool("isJumpAtk", true);
            }

        }
        if (Input.GetKeyDown(KeyCode.C) && !isJump && Time.time > NextDash)
        {
            playerAnimator.applyRootMotion = (true);
            playerAnimator.SetTrigger("Dodge");
            //player.transform.position += player.transform.forward * Time.deltaTime * speed*50;
            NextDash = Time.time + 2f;
        }*/

    }

    //private void LateUpdate()
    //{
    //    zAtack = 0;
    //}
    // Update is called once per frame
    void FixedUpdate()
    {
        //Bye();
        //if (playerRigidbody.velocity.x < 1 && playerRigidbody.velocity.z < 1)
        //{
        //    playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, -9.8f, playerRigidbody.velocity.z);
        //}
        if (playerHp >0)
        {
            //MoveFunc(isAttack, isJump, isRun);
        }        
    }

    void MoveFunc(bool isAttack, bool isJump , bool isRun)
    {
        float moveSpeed = 0;
        if (isRun)
        {
            DirControl(isAttack, ref moveSpeed, 2.8f);
            if (canMove)
            {
                Move(moveSpeed);
            }
        }
        else
        {
            DirControl(isAttack, ref moveSpeed, 2.8f);
            if (canMove)
            {
                Move(moveSpeed);
            }
        }     
    }

    void Bye()
    {
        if (Input.GetKey(KeyCode.B))
        {
            secClickTime = Time.time - firstClickTime;
            Debug.Log(secClickTime);
            if (secClickTime< doubleSpacing)
            {
                playerAnimator.SetBool("isRun",true);
            }
            else
            {
                playerAnimator.SetBool("isBye", true);
            }         
            firstClickTime = Time.time;
        }
        else
        {
            playerAnimator.SetBool("isBye", false);
        }
        if (Input.GetKeyUp(KeyCode.B) && playerAnimator.GetBool("isRun"))
        {
            playerAnimator.SetBool("isRun", false);
        }
    }

    void Mobs()
    {
        monsterPrefabIns = LoadMonster.LoadData();
        Mobsposition();
    }
    void Mobsposition()
    {
        int count = 0;
        for(int i=0; i< monsterPrefabIns.Count;i++)
        {
            monsterPrefabIns[i].transform.position = new Vector3(5.0f, 1.0f, 0.0f + count);
            count+=8;
        }
    }
    void SetCamera()
    {
        FlowPlayer.playerPos = player.transform;
    }
    void Player()
    {
        player = LoadCharacter.LoadData();

        playerAnimator = player.GetComponent<Animator>();
        
        playerAnimation = player.GetComponent<Animation>();
        playerRigidbody = player.GetComponent<Rigidbody>();


    }
    void Terrain()
    {
        terrainPrefabIns = LoadTerrain.LoadData();
    }

   Vector3 CheckForWard()
    {
        var x = -Input.GetAxis("Vertical");
        var z = Input.GetAxis("Horizontal");

        var a = -Camera.main.transform.forward * x;
        a.y = 0;
        var b = Camera.main.transform.right * z;
        b.y = 0;


        player.transform.forward = Vector3.Lerp(player.transform.forward, new Vector3(a.x, 0, b.z), 1f);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {

                player.transform.forward = Vector3.Lerp(player.transform.forward, new Vector3(-a.x, 0, b.z), 1f);
            }
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                player.transform.forward = Vector3.Lerp(player.transform.forward, new Vector3(a.x, 0, -b.z), 1f);
            }
        }
        return player.transform.forward;
    }

    void DirControl(bool isAttack, ref float moveSpeed,float speed)
    {
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
        {
            if(isAttack)
            {
                return;
            }
            else
            {
                playerAnimator.SetBool("isWalkF", true);
                moveSpeed = speed;
            }
        }
        else
        {
            playerAnimator.SetBool("isWalkF", false);
            moveSpeed = 0;
        }
    }
    void Move(float n)
    {
        
        if (n != 0)
        {
            player.transform.position += CheckForWard() * Time.deltaTime * Accel() * n;
            //playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, -9.8f, playerRigidbody.velocity.z);
        }
    }
    float Accel()
    {
        float move = Mathf.Lerp(0, speed, 0.3f);
        return move;
    }

    
}

