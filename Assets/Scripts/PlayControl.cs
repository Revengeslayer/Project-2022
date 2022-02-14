using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : MonoBehaviour
{
    private static float speed = 4;
    bool isAttack;
    bool isJump;
    bool isRun;

    public static GameObject player;
    private static Animator playerAnimator;


    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        playerAnimator = player.GetComponent<Animator>();      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //Bye();
        //if (playerRigidbody.velocity.x < 1 && playerRigidbody.velocity.z < 1)
        //{
        //    playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, -9.8f, playerRigidbody.velocity.z);
        //}
    }
    #region Move
    static float Accel()
    {
        float move = Mathf.Lerp(0, speed, 0.3f);
        return move;
    }
    static Vector3 CheckForWard()
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
    public static void Move(float n)
    {

        if (n != 0)
        {
           
            player.transform.position += CheckForWard() * Time.deltaTime * Accel() * n;
            //playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, -9.8f, playerRigidbody.velocity.z);
        }
    }
    #endregion
}
