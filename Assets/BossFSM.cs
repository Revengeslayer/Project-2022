using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSM : MonoBehaviour
{
    /// <summary>
    /// BOSS 動畫相關
    /// </summary>
    private Animator anim;
    public GameObject DieAnim;
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
    public static BossFSMState mCurrentState;

    public enum BossFSMState
    {
        NONE = -1,
        Idle,
        Active,
        Attack1,
        Attack2,
        Stand,
        Walk,
        Roll,
        Die
    }

    void Start()
    {
        mCurrentState = BossFSMState.Idle;
        anim = GetComponent<Animator>();
    }


    private void CheckNowState()
    {
        if(mCurrentState== BossFSMState.Idle)
        {
            anim.Play("Idleactivate");
        }
        if (mCurrentState == BossFSMState.Active)
        {
            anim.Play("Activate");
        }
        if (mCurrentState == BossFSMState.Attack1)
        {
            anim.Play("Attack01");
        }
        if (mCurrentState == BossFSMState.Attack2)
        {
            anim.Play("Attack02");
        }
        if (mCurrentState == BossFSMState.Stand)
        {
            anim.Play("Idle");
        }
        if (mCurrentState == BossFSMState.Walk)
        {
            anim.Play("Walk");
        }
        if (mCurrentState == BossFSMState.Roll)
        {
            anim.Play("Roll");
        }
        if (mCurrentState == BossFSMState.Die)
        {
            anim.Play("Die");
        }
    }
    void Update()
    {
        CheckNowState();
    }

	void DieEvent()
	{
		GameObject G = (GameObject)Instantiate(DieAnim, gameObject.transform.position, gameObject.transform.rotation);	
	    gameObject.SetActive(false);
        G.SetActive(true);
    }

}
