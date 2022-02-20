using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFSM : MonoBehaviour
{
    /// <summary>
    /// BOSS 動畫相關
    /// </summary>
    private Animator anim;
    /// <summary>
	/// 檢查狀態的delegate
	/// </summary>
	private delegate void CheckState();
    private CheckState mCheckState;
    /// <summary>
    /// 做狀態的delegate
    /// </summary>
    private delegate void DoState();
    private DoState mDoState;
    /// <summary>
    /// 當前狀態
    /// </summary>
    public static DogFSMState mCurrentState;

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
        Die
    }

    void Start()
    {
        mCurrentState = DogFSMState.Idle_Battle;
        anim = GetComponent<Animator>();
    }


    private void CheckNowState()
    {
        if (mCurrentState == DogFSMState.Idle_Battle)
        {
            anim.Play("Idle_Battle");
        }      
        if (mCurrentState == DogFSMState.Attack01)
        {
            anim.Play("Attack01");
        }
        if (mCurrentState == DogFSMState.Attack02)
        {
            anim.Play("Attack02");
        }
        if (mCurrentState == DogFSMState.Block)
        {
            anim.Play("Block");
        }
        if (mCurrentState == DogFSMState.Wander)
        {
            anim.Play("Wander");
        }
        if (mCurrentState == DogFSMState.Chase)
        {
            anim.Play("Chase");
        }
        if (mCurrentState == DogFSMState.GetHit)
        {
            anim.Play("Activate");
        }
        if (mCurrentState == DogFSMState.Die)
        {
            anim.Play("Die");
        }
    }
    void Update()
    {
        CheckNowState();
    }
}
