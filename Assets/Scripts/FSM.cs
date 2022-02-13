using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
	//檢查狀態的delegate
	private delegate void CheckState();
	private CheckState mCheckState;
	//做狀態的delegate
	private delegate void DoState();
	private CheckState mDoState;

	//角色動作
	private Animator anim;


	private FSMState mCurrentState;
	// Start is called before the first frame update
	public enum FSMState
	{
		NONE = -1,
		Idle,
		Chase,
		Attack,
	}
	private void Start()
	{
		mCurrentState = FSMState.Idle;
		mCheckState = CheckIdleState;
		mDoState = DoIdleState;
		anim = GetComponent<Animator>();
	}

	#region check
	private void CheckIdleState()
	{
		anim.SetBool("Attack", false);
		anim.SetBool("Chase", false);

		if (Input.GetKeyDown(KeyCode.A))
		{
			anim.SetBool("Attack", true);

			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			anim.SetBool("Chase", true);

			mCurrentState = FSMState.Chase;
			mCheckState = CheckChaseState;
			mDoState = DoChaseState;
		}
		
		Debug.Log("CheckIdleState()");
	}
	private void CheckAttackState()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			anim.SetBool("Attack", false);


			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			anim.SetBool("Attack", false);
			anim.SetBool("Chase", true);

			mCurrentState = FSMState.Chase;
			mCheckState = CheckChaseState;
			mDoState = DoChaseState;
		}
		Debug.Log("CheckAttackState()");
	}
	private void CheckChaseState()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			anim.SetBool("Chase", false);
			anim.SetBool("Attack", true);
			
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			anim.SetBool("Chase", false);

			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
		Debug.Log("CheckChaseState()");
	}
	#endregion

	#region Do
	private void DoIdleState()
	{
		//anim.Play("Idle_Battle");
	}
	private void DoAttackState()
	{
		//anim.Play("Attack01");
	}
	private void DoChaseState()
	{
		//anim.Play("WalkForwardBattle");
	}
	#endregion

	
	// Update is called once per frame
	void Update()
    {
		//偵測狀態
		mCheckState();

		//狀態做甚麼
		mDoState();
    }

	
}
