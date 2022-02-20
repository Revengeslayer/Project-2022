using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
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
	private FSMState mCurrentState;
	/// <summary>
	/// 角色相關
	/// </summary>
	//角色動作
	private Animator anim;
	//角色的武士刀
	private GameObject katana;
	/// <summary>
	/// 攻擊相關
	/// </summary>
	//is Battle?
	public bool isBattle;
    //is Attack?
    public bool isAttack;
	//is Skill?
	public bool isSkill;
	//Attack Count
	int atkCount;
	//最後按下時間
	private float lastClick;
	//回傳
	public static int zAttack;
	/// <summary>
	/// 受傷相關
	/// </summary>
	public static bool isGitHit;
	public static int gitHitCount;
	/// <summary>
	/// 移動相關
	/// </summary>
	//is Move?
	public bool isMove;
	public float moveSpeed;
	//is Dodge?
	public bool isDodge;
	/// <summary>
	/// 死亡相關
	/// </summary>
	public static bool isDeath;
 
	// Start is called before the first frame update
	public enum FSMState
	{
		NONE = -1,
		Idle,
		BattleIdle,
		Move,
		Attack,
		Skill,
		Dodge,
		GetHit,
		Die
	}

	private void Start()
	{
		isGitHit = false;
		isDeath = false;
		katana = GameObject.Find("Hoshi_katana");
		katana.SetActive(false);
		mCurrentState = FSMState.Idle;
		mCheckState = CheckIdleState;
		mDoState = DoIdleState;
		anim = GetComponent<Animator>();
	}

	#region check
	private void CheckIdleState()
	{
		//Idle->Skill
		if (isSkill ==true)
		{		
			anim.SetBool("isSkill", true);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
		//Idle->Attack
		if (isAttack == true)
		{
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//Idle->Move
		if (isMove ==true)
        {
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
		//Idle ->GetHit
		if (isGitHit == true)
		{
			anim.Play("GetHit");
			isGitHit = false;
			anim.SetBool("isGetHit", true);
			mCurrentState = FSMState.GetHit;
			mCheckState = CheckGetHitState;
			mDoState = DoGetHitState;
		}
	}
	private void CheckBattleIdleState()
	{
		//BIdle->Skill
		if (isSkill == true)
		{
			anim.SetBool("isSkill", true);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
		//BIdle->Attack
		if (isAttack == true)
		{
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//BIdle->Move
		if (isMove == true)
		{
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
		//BIdle ->GetHit
		if (isGitHit == true)
		{
			anim.Play("GetHit");
			isGitHit = false;
			anim.SetBool("isGetHit", true);
			mCurrentState = FSMState.GetHit;
			mCheckState = CheckGetHitState;
			mDoState = DoGetHitState;
		}
	}	
	private void CheckAttackState()
    {
		if (Input.GetKeyDown(KeyCode.Q))
		{ 
			anim.SetBool("isAttack", false);
			atkCount = 0;
			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
		//不攻擊回歸BI
		if(isAttack==false && isBattle==true)
        {
			anim.SetBool("isAttack", false);
			anim.SetBool("isBattle", true);
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
		}
		//不攻擊回歸I
		if (isAttack == false && isBattle == false)
		{
			anim.SetBool("isAttack", false);
			anim.SetBool("isBattle", false);
			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
		//攻擊轉移動
		if (isAttack == false && isMove == true)
		{
			anim.SetBool("isAttack", false);
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
		////攻擊中轉Skill
		if(isSkill==true)
		{
			//isAttack = false;
			anim.SetBool("isAttack", false);
			anim.SetBool("isSkill", true);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
	}
	private void CheckSkillState()
	{
		if(isSkill==false &&isAttack ==true)
		{
			anim.SetBool("isSkill", false);
			anim.SetBool("Skill1", false);
			anim.SetBool("Skill2", false);
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		if (isSkill == false && isMove ==true)
		{
			anim.SetBool("isSkill", false);
			anim.SetBool("Skill1", false);
			anim.SetBool("Skill2", false);
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
		if (isSkill == false)
		{
			anim.SetBool("isSkill", false);
			anim.SetBool("Skill1", false);
			anim.SetBool("Skill2", false);
			anim.SetBool("isBattle", true);
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
		}
	}
	private void CheckDodgeState()
	{
		
	}
	private void CheckMoveState()
	{
		//不移動且戰鬥回BI
		if(isMove==false && isBattle == true)
		{
			anim.SetBool("isWalkF", false);
			anim.SetBool("isBattle", true);
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
		}
		//不移動且非戰鬥回I
		if (isMove == false && isBattle == false)
		{
			anim.SetBool("isWalkF", false);
			anim.SetBool("isBattle", false);
			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
		//從移動中轉攻擊
		if(isMove==true && isAttack==true)
        {
			anim.SetBool("isAttack", true);
			isMove = false;
			anim.SetBool("isWalkF", false);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//從移動中轉技能
		if (isSkill==true)
		{
			isMove = false;
			anim.SetBool("isSkill", true);
			anim.SetBool("isWalkF", false);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
		
	}
	private void CheckGetHitState()
	{
		if(isBattle==true && isGitHit == false)
        {
			anim.SetBool("isBattle", true);
			anim.SetBool("isGetHit", false);
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
		}
		if(isGitHit==true &&isMove==true)
        {
			isGitHit = false;
			anim.SetBool("isGetHit", false);
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
	}
	private void CheckDieState()
    {

    }
	#endregion

	#region Do
	private void DoIdleState()
	{		
		anim.SetInteger("combo2", 0);
		anim.SetInteger("combo3", 0);
		//強切BI
		if (Input.GetKeyDown(KeyCode.B))
		{
			isBattle = true;
			anim.SetBool("isBattle", true);
			isBattle = true;
			atkCount = 0;
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
		}
		//按下攻擊
		if (Input.GetKeyDown(KeyCode.Z))
		{
			//var lastClick = Time.time;
			isAttack = true;
			isBattle = true;
			zAttack = 1;
			atkCount = 1;
		}
		//按下方向鍵
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;
			isAttack = false;
			zAttack = 0;
			atkCount = 0;
		}
		//觸發Skill
		if(Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C))
		{
			isBattle = true;
			isSkill = true;
			if(Input.GetKey(KeyCode.X))
			{
				isAttack = false;
				anim.SetBool("Skill1", true);
			}
			if (Input.GetKey(KeyCode.C))
			{
				isAttack = false;
				anim.SetBool("Skill2", true);
			}
		}
	}
	private void DoBattleIdleState()
	{
		
		anim.SetInteger("combo2", 0);
		anim.SetInteger("combo3", 0);
		//強切Idle
		if (Input.GetKeyDown(KeyCode.B))
		{
			isBattle = false;
			anim.SetBool("isBattle", false);
			isBattle = false;
			atkCount = 0;
			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
		//按下攻擊
		if (Input.GetKeyDown(KeyCode.Z))
		{
			isBattle = true;
			isAttack = true;
			zAttack = 1;
			atkCount = 1;
		}
		//按下方向鍵
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;
			isAttack = false;
			zAttack = 0;
			atkCount = 0;
		}
		//觸發Skill
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C))
		{
			isBattle = true;
			isSkill = true;
			if (Input.GetKey(KeyCode.X))
			{
				isAttack = false;
				anim.SetBool("Skill1", true);
			}
			if (Input.GetKey(KeyCode.C))
			{
				isAttack = false;
				anim.SetBool("Skill2", true);
			}
		}
	}
	private void DoAttackState()
	{
		isGitHit = false;
		#region 先不用
		//判斷第二下
		if (Input.GetKeyDown(KeyCode.Z) && anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK01"))
		{
			anim.SetInteger("combo2", anim.GetInteger("combo2") + 1);
			if (anim.GetInteger("combo2") <= 1)
			{
				zAttack = 2;
			}
			atkCount = 2;
		}
		//判斷第三下
		if (Input.GetKeyDown(KeyCode.Z) && anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK02"))
		{
			anim.SetInteger("combo3", anim.GetInteger("combo3") + 1);

			if (anim.GetInteger("combo3") <= 1)
			{
				zAttack = 3;
			}
			atkCount = 3;
		}
		//攻擊清除回歸
		//第一下清除
		if (atkCount == 1
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK01")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
		{
			atkCount = 0;
			isAttack = false;
			//判斷有沒有攻擊中按住方向鍵
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			{
				isMove = true;
			}
		}
		//第二下清除
		if (atkCount == 2
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK02")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
		{
			atkCount = 0;
			anim.SetInteger("combo2", 0);
			isAttack = false;
			//判斷有沒有攻擊中按住方向鍵
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			{
				isMove = true;
			}
		}
		//第三下清除
		if (atkCount == 3
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK03")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
		{
			atkCount = 0;
			anim.SetInteger("combo3", 0);
			isAttack = false;
			//判斷有沒有攻擊中按住方向鍵
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			{
				isMove = true;
			}
		}
		////觸發Skill
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C))
		{
			isBattle = true;
			isSkill = true;
			if (Input.GetKey(KeyCode.X))
			{
				isAttack = false;
				anim.SetBool("Skill1", true);
			}
			if (Input.GetKey(KeyCode.C))
			{
				isAttack = false;
				anim.SetBool("Skill2", true);
			}
		}
		#endregion
		#region 測試
		//if (Input.GetKeyDown(KeyCode.Z) && CheckCombo(1, lastClick))
		//{
		//	lastClick = Time.time;
		//	Debug.Log("連及觸發");
		//}
		//Debug.Log("atkCount               "+ atkCount);
		//Debug.Log("zAtack                 "+ zAtack);
		#endregion
	}
    private void DoSkillState()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime>1 &&(anim.GetCurrentAnimatorStateInfo(0).IsName("Skill1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Skill2")))
		{
			isSkill = false;			
		}

		//按下攻擊
		if (Input.GetKeyDown(KeyCode.Z))
		{
			//var lastClick = Time.time;
			isAttack = true;
			isBattle = true;
			zAttack = 1;
			atkCount = 1;
		}
		//都不按方向鍵
		if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) == true)
		{
			isMove = false;
		}
		//按下方向鍵
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
        {
            isMove = true;
            isAttack = false;
            zAttack = 0;
            atkCount = 0;
        }

    }
	private void DoDodgeState()
	{

	}
	private void DoMoveState()
	{
		isGitHit = false;
		//都不按方向鍵
		if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) == true)
		{
			isMove = false;
		}
		//移動中觸發攻擊
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{				
				isAttack = true;
				isBattle = true;
				zAttack = 1;
				atkCount = 1;
			}
		}
		//觸發Skill
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C))
		{			
			if (Input.GetKey(KeyCode.X))
			{
				
				isAttack = false;
				isBattle = true;
				isSkill = true;
				anim.SetBool("Skill1", true);
			}
			if (Input.GetKey(KeyCode.C))
			{
			
				isAttack = false; 
				isBattle = true;
				isSkill = true;
				anim.SetBool("Skill2", true);
			}
		}
	}
	private void DoGetHitState()
	{
		isBattle = true;
		
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			isGitHit = false;			
			atkCount = 0;			
		}
		////判斷有沒有攻擊中按住方向鍵
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;			
		}
	}
	private void DoDieState()
	{
		isDeath = false;
		isMove = false;
		isAttack = false;
		isGitHit = false;
		isSkill = false;
		
		anim.Play("Die");
	}
	#endregion


	// Update is called once per frame
	void Update()
	{
		//偵測狀態
		//Debug.Log("目前狀態          " + mCurrentState);
		//Debug.Log(isGitHit);
		//判斷哪一個Attack
		zAttack = 0;
		//如果死亡了
		if(isDeath)
        {
			mCurrentState = FSMState.Die;
			mCheckState = CheckDieState;
			mDoState = DoDieState;
		}
		//要不要拔刀
		if (isBattle || isAttack)
		{
			katana.SetActive(true);
		}
        else 
		{
			katana.SetActive(false);
		}
		mCheckState();
		//狀態做甚麼
		mDoState();
	}

	private void FixedUpdate()
	{
		if (isMove == true)
		{
			PlayControl.Move(moveSpeed);
		}
	}

	private bool CheckCombo(float cdTime, float lastClickTime)
	{

		if (Time.time - lastClickTime < cdTime)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

