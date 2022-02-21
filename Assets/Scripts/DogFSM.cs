using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFSM : MonoBehaviour
{
    //�ؼ�
    private GameObject player;
    /// <summary>
    /// Dog����
    /// </summary>
    //�Ǫ���l��m
    private Vector3 initDic;
    [Header("�����t��")]
    public float dogWalkSpeed;
    [Header("�]�B�t��")]
    public float dogRunSpeed;
    /// <summary>
    /// Dog �ʵe����
    /// </summary>
    private Animator anim;
    /// <summary>
    /// ���e���A
    /// </summary>
    public static DogFSMState mCurrentState;
    /// <summary>
    /// �U�ذ����Z��
    /// </summary>
    [Header("�����Z��")]
    public float dogAtkDic;
    [Header("�l�v�Z��")]
    public float dogChaseDic;
    [Header("ĵ�ٶZ��")]
    public float dogWarnDic;
    [Header("�^�k�Z��")]
    public float dogBackToInitDic;
    [Header("�󴫫ݾ����O�����j�ɶ�")]
    public float actRestTme;
    [Header("�l��ζW�X�d�򪺶��j�ɶ�")]
    public float reRestTme;
    //�̫�ʧ@���ɶ�
    private float lastActTime;
    //�H���ʧ@�v��
    private float[] actionWeight = { 3000, 4000 };
    //�Ǫ����ؼд¦V
    private Quaternion targetRotation;
    //isRuning?
    private bool is_Running = false;



    public enum DogFSMState
    {
        NONE = -1,
        Idle_Battle,
        Attack01,
        Attack02,
        Block,
        Wander,
        Chase,
        GetHit,
        Die,
        Return
    }

    void Start()
    {
        initDic = this.transform.position;
        player =GameObject.Find("Character");
        //player=GameObject.Find("Character(Clone)");
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
        if (mCurrentState == DogFSMState.Attack01)
        {

        }
        if (mCurrentState == DogFSMState.Attack02)
        {

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
            //�¦V���a��m
            targetRotation = Quaternion.LookRotation(player.transform.position - gameObject.transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            transform.Translate(Vector3.forward * Time.deltaTime * dogRunSpeed);
            ChaseCCancelCheck();
        }
        if (mCurrentState == DogFSMState.GetHit)
        {

        }
        if (mCurrentState == DogFSMState.Die)
        {

        }
        if(mCurrentState == DogFSMState.Return)
        {
            targetRotation = Quaternion.LookRotation(initDic - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            transform.Translate(Vector3.forward * Time.deltaTime * dogRunSpeed);
            ReturnCheck();
        }
    }

    private void WanderRadiusCheck()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        var diatanceToInitial = Vector3.Distance(transform.position, initDic);

        if (diatanceToPlayer < dogChaseDic)
        {
            anim.SetBool("Wander", false);
            is_Running = true;
            anim.SetBool("Chase", true);
            mCurrentState = DogFSMState.Chase;
        }

        if (diatanceToInitial> dogBackToInitDic)
        {
            targetRotation = Quaternion.LookRotation(initDic - transform.position, Vector3.up);
        }
    }
    /// <summary>
    /// �l�v�W�X�d��/�l�v��᪺�ˬd
    /// </summary>
    private void ReturnCheck()
    {
        var diatanceToInitial = Vector3.Distance(gameObject.transform.position, initDic);
        //�p�G�w�g�����l��m�A�h�H���@�ӫݾ����A
        if (diatanceToInitial < 0.5f)
        {
            is_Running = false;
            anim.SetBool("Chase", false);
            RandomAction();
        }
    }

    private void ChaseCCancelCheck()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);
        var diatanceToInitial = Vector3.Distance(gameObject.transform.position, initDic);

        if (diatanceToInitial > dogBackToInitDic || diatanceToPlayer > dogChaseDic)
        {
            StartCoroutine(IdleAmoment());
            mCurrentState = DogFSMState.Return;
        }
    }
    /// <summary>
    /// �O�_�n�l�v
    /// </summary>
    private void TargetDicChenk()
    {
        var dicToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if(dicToPlayer< dogChaseDic)
        {
            anim.SetBool("Idle", false);
            is_Running = true;
            anim.SetBool("Chase", true);
            mCurrentState = DogFSMState.Chase;
            
        }
    }
    /// <summary>
    /// ��ܫݾ�/�C��
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
        CheckNowState();
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

    IEnumerator IdleAmoment()
    {
        anim.Play("Idle_Battle");
        yield return new WaitForSeconds(reRestTme);
    }
}