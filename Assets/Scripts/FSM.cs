using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	public bool canMove;
	public static float moveSpeed;
	public bool ChangeForword =false;
	// is AtkToMove
	public bool isAtkToMove;
	//is Dodge?
	public bool isDodge;
	// is SkillToDodge?
	public bool isSkillToDodge;
	/// <summary>
	/// 死亡相關
	/// </summary>
	public static bool isDeath;
	//BItoI check
	private float BItoITime;

	public static float DizzyCount;

	private GameObject GameOver;
	private GameObject GameOverText;
	private GameObject GameOverText2;

	public static GameObject DeadBody;

	public static bool BossAlive;
	private float disappearTime;
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
		Die,
	}

	private void Start()
	{
		disappearTime = 0;

		isGitHit = false;
		isDeath = false;

		GameOver= GameObject.Find("GameOver");
		GameOver.SetActive(false);
		GameOverText = GameObject.Find("GameOverText");
		GameOverText.SetActive(false);

		BossAlive = true;
		GameOverText2 = GameObject.Find("GameOverText2");
		GameOverText2.SetActive(false);

		katana = GameObject.Find("Hoshi_katana");
		katana.SetActive(false);
		mCurrentState = FSMState.Idle;
		mCheckState = CheckIdleState;
		mDoState = DoIdleState;
		anim = GetComponent<Animator>();

		moveSpeed = 4;
	}

	#region check
	private void CheckIdleState()
	{	
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
		//Idle ->Dodge
		else if (isDodge ==true)
		{
			anim.SetBool("isDodge", true);
			mCurrentState = FSMState.Dodge;
			mCheckState = CheckDodgeState;
			mDoState = DoDodgeState;
		}
		//Idle->Skill
		else if (isSkill == true)
		{
			anim.SetBool("isSkill", true);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
		//Idle->Attack
		else if (isAttack == true)
		{
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//Idle->Move
		else if (isMove == true)
		{
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
	}
	private void CheckBattleIdleState()
	{
		
		//BIdle ->GetHit
		if (isGitHit == true)
		{
			BItoITime = 0;
			anim.Play("GetHit");
			isGitHit = false;
			anim.SetBool("isGetHit", true);
			mCurrentState = FSMState.GetHit;
			mCheckState = CheckGetHitState;
			mDoState = DoGetHitState;
		}
		//BIdle ->Dodge
		else if (isDodge == true)
		{
			BItoITime = 0;
			anim.SetBool("isDodge", true);
			mCurrentState = FSMState.Dodge;
			mCheckState = CheckDodgeState;
			mDoState = DoDodgeState;
		}
		//BIdle->Skill
		else if (isSkill == true)
		{
			BItoITime = 0;
			anim.SetBool("isSkill", true);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
		//BIdle->Attack
		else if (isAttack == true)
		{
			BItoITime = 0;
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//BIdle->Move
		else if (isMove == true)
		{
			BItoITime = 0;
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
		//BIt ->I
		if (CheckBItoI())
		{
			BItoITime = 0;
			anim.SetBool("isBattle", false);
			isBattle = false;
			atkCount = 0;
			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
	}	
	private void CheckAttackState()
    {
		
		if(isAttack==false )
        {
			if(isMove == true)//攻擊轉移動
			{
				anim.SetInteger("combo2", 0);
				anim.SetInteger("combo3", 0);
				anim.SetBool("isAttack", false);
				anim.SetBool("isWalkF", true);
				mCurrentState = FSMState.Move;
				mCheckState = CheckMoveState;
				mDoState = DoMoveState;
			}
			else if (isBattle == true)//不攻擊回歸BI
			{
				anim.SetInteger("combo2", 0);
				anim.SetInteger("combo3", 0);
				anim.SetBool("isAttack", false);
				anim.SetBool("isBattle", true);
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
		}
		if (isAttack == true)
		{	
			if(isGitHit==true &&(PlayerInfo.DizzyCount%3==1))
            {
				anim.SetInteger("combo2", 0);
				anim.SetInteger("combo3", 0);
				isAttack = false;
				anim.SetBool("isAttack", false);
				anim.Play("GetHit");
				isGitHit = false;
				anim.SetBool("isGetHit", true);
				mCurrentState = FSMState.GetHit;
				mCheckState = CheckGetHitState;
				mDoState = DoGetHitState;

			}
			//攻擊中轉Dodge
			else if (isDodge == true)
            {
				anim.SetInteger("combo2", 0);
				anim.SetInteger("combo3", 0);
				isAttack = false;
				anim.SetBool("isAttack", false);
				anim.SetBool("isDodge", true);
				mCurrentState = FSMState.Dodge;
				mCheckState = CheckDodgeState;
				mDoState = DoDodgeState;
			}
			//攻擊中轉Skill
			else if (isSkill == true)
			{
				anim.SetInteger("combo2", 0);
				anim.SetInteger("combo3", 0);
				isAttack = false;
				anim.SetBool("isAttack", false);
				anim.SetBool("isSkill", true);
				mCurrentState = FSMState.Skill;
				mCheckState = CheckSkillState;
				mDoState = DoSkillState;
			}
		}
	}
	private void CheckSkillState()
	{
		//Skill中轉翻滾
		if(isSkill == true)
        {
			if(isDodge == true)
            {
				isSkillToDodge = false;
				anim.SetBool("isSkill", false);
				anim.SetBool("Skill1", false);
				anim.SetBool("Skill2", false);
				anim.SetBool("Skill3", false);
				anim.SetBool("isDodge", true);
				mCurrentState = FSMState.Dodge;
				mCheckState = CheckDodgeState;
				mDoState = DoDodgeState;
			}
        }
		//完整播完技能
		if (isSkill == false)
		{
			//轉到移動
			if (isMove == true)
			{
				anim.SetBool("isSkill", false);
				anim.SetBool("Skill1", false);
				anim.SetBool("Skill2", false);
				anim.SetBool("Skill3", false);
				anim.SetBool("isWalkF", true);
				mCurrentState = FSMState.Move;
				mCheckState = CheckMoveState;
				mDoState = DoMoveState;
			}
			else//回到BI
			{
				anim.SetBool("isSkill", false);
				anim.SetBool("Skill1", false);
				anim.SetBool("Skill2", false);
				anim.SetBool("Skill3", false);
				anim.SetBool("isBattle", true);
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
		}
	}
	private void CheckDodgeState()
	{
		if(isDodge ==false)
		{
			if (isMove == true)
			{
				anim.SetBool("isDodge", false);
				anim.SetBool("isWalkF", true);
				mCurrentState = FSMState.Move;
				mCheckState = CheckMoveState;
				mDoState = DoMoveState;
			}
			else
			{
				anim.SetBool("isDodge", false);
				anim.SetBool("isBattle", true);
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
		}

		//if(isGitHit == true)
		//{
		//	isMove = false;
		//	canMove = false;
		//	anim.SetBool("isWalkF", false);
		//	anim.Play("GetHit");
		//	isGitHit = false;
		//	anim.SetBool("isGetHit", true);
		//	mCurrentState = FSMState.GetHit;
		//	mCheckState = CheckGetHitState;
		//	mDoState = DoGetHitState;
		//}
	}
	private void CheckMoveState()
	{
		//移動轉受傷
		if (isGitHit == true)
		{
			isMove = false;
			canMove = false;
			anim.SetBool("isWalkF", false);
			anim.Play("GetHit");
			isGitHit = false;
			anim.SetBool("isGetHit", true);
			mCurrentState = FSMState.GetHit;
			mCheckState = CheckGetHitState;
			mDoState = DoGetHitState;
		}
		//從移動中轉技能
		else if (isSkill == true)
		{
			isMove = false;
			anim.SetBool("isSkill", true);
			canMove = false;
			anim.SetBool("isWalkF", false);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
		else if (isMove == true )
		{			
			if(isDodge == true)//從移動中轉翻滾
			{
				anim.SetBool("isDodge", true);
				isMove = false;
				canMove = false;
				anim.SetBool("isWalkF", false);
				mCurrentState = FSMState.Dodge;
				mCheckState = CheckDodgeState;
				mDoState = DoDodgeState;
			}
			else if (isAttack == true)//從移動中轉攻擊
			{
				anim.SetBool("isAttack", true);
				isMove = false;
				canMove = false;
				anim.SetBool("isWalkF", false);
				mCurrentState = FSMState.Attack;
				mCheckState = CheckAttackState;
				mDoState = DoAttackState;
			}
		}
		else if (isMove==false)
		{
			if (isBattle == true)//不移動且戰鬥回BI
			{
				canMove = false;
				anim.SetBool("isWalkF", false);
				anim.SetBool("isBattle", true);
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
			else if(isBattle == false)//不移動且非戰鬥回I
            {
				canMove = false;
				anim.SetBool("isWalkF", false);
				anim.SetBool("isBattle", false);
				mCurrentState = FSMState.Idle;
				mCheckState = CheckIdleState;
				mDoState = DoIdleState;
			}
		}

		
		
	}
	private void CheckGetHitState()
	{
		if(isGitHit == false)
        {
			if (isBattle == true)
			{
				anim.SetBool("isBattle", true);
				anim.SetBool("isGetHit", false);
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
		}
		//if(isGitHit==true &&isMove==true)
		//{
		//	isGitHit = false;
		//	anim.SetBool("isGetHit", false);
		//	anim.SetBool("isWalkF", true);
		//	mCurrentState = FSMState.Move;
		//	mCheckState = CheckMoveState;
		//	mDoState = DoMoveState;
		//}
	}
	private void CheckDieState()
    {
		

	}
	#endregion

	#region Do
	private void DoIdleState()
	{
		isSkillToDodge = false;
		ChangeForword = false;
		isAtkToMove = false;

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
		if(Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C)|| Input.GetKey(KeyCode.V))
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
			if(Input.GetKey(KeyCode.V))
			{
				isAttack = false;
				anim.SetBool("Skill3", true);
			}
		}
		//觸發翻滾
		if (Input.GetKey(KeyCode.Space))
		{
			isDodge = true;
		}
	}
	private void DoBattleIdleState()
	{	
		isBattle = true;
		isSkillToDodge = false;
		ChangeForword = false;
		isAtkToMove = false;
		anim.SetInteger("combo2", 0);
		anim.SetInteger("combo3", 0);
		anim.SetBool("isAttack", false);
		//按下攻擊
		if (Input.GetKeyDown(KeyCode.Z))
		{			
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
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C)|| Input.GetKey(KeyCode.V))
		{			
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
			if (Input.GetKey(KeyCode.V))
			{
				isAttack = false;
				anim.SetBool("Skill3", true);
			}
		}
		//觸發翻滾
		if (Input.GetKey(KeyCode.Space))
		{
			isDodge = true;
		}
	}
	private bool CheckBItoI()
	{
		BItoITime += Time.deltaTime;
		if (BItoITime > 6)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	private void DoAttackState()
	{
		isGitHit = false;
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
			////判斷有沒有攻擊中按住方向鍵
			//if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			//{
			//	CheckForward();
			//	if (isAtkToMove)
			//	{
			//		isMove = true;
			//	}
			//}
		}
		//第二下清除
		if (atkCount == 2
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK02")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
		{
			atkCount = 0;
			anim.SetInteger("combo2", 0);
			isAttack = false;
			////判斷有沒有攻擊中按住方向鍵
			//if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			//{
			//	CheckForward();
			//	if (isAtkToMove)
			//	{
			//		isMove = true;
			//	}
			//}
		}
		//第三下清除
		if (atkCount == 3
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK03")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
		{
			atkCount = 0;
			anim.SetInteger("combo3", 0);
			isAttack = false;
			anim.SetBool("isAttack", false);
			////判斷有沒有攻擊中按住方向鍵
			//if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			//{
			//	CheckForward();
			//	if (isAtkToMove)
			//	{
			//		isMove = true;
			//	}
			//}
		}
		//觸發Skill
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C)|| Input.GetKey(KeyCode.V))
		{
			isBattle = true;
			isSkill = true;
			if (Input.GetKey(KeyCode.X))
			{
				anim.SetBool("Skill1", true);
			}
			if (Input.GetKey(KeyCode.C))
			{
				anim.SetBool("Skill2", true);
			}
			if (Input.GetKey(KeyCode.V))
			{
				isAttack = false;
				anim.SetBool("Skill3", true);
			}
		}
		//觸發翻滾
		if (Input.GetKey(KeyCode.Space))
		{
			isDodge = true;
		}
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
        {
			CheckForward();		
            //if (isAtkToMove)
            //{
                isMove = true;
            //}
        }

    }
	private void CheckForward()
    {
		var x = -Input.GetAxis("Vertical");
		var z = Input.GetAxis("Horizontal");

		var a = -Camera.main.transform.forward * x;
		a.y = 0;
		var b = Camera.main.transform.right * z;
		b.y = 0;

		gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, new Vector3(a.x, 0, b.z), 0.95f);
	}
    private void DoSkillState()
	{
		isSkillToDodge = false;
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime>1 &&(anim.GetCurrentAnimatorStateInfo(0).IsName("Skill1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Skill2") || anim.GetCurrentAnimatorStateInfo(0).IsName("Skill3")))
		{
			isSkill = false;			
		}

		////按下攻擊
		//if (Input.GetKeyDown(KeyCode.Z))
		//{
		//	//var lastClick = Time.time;
		//	isAttack = true;
		//	isBattle = true;
		//	zAttack = 1;
		//	atkCount = 1;
		//}
		//都不按方向鍵
		if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) == true)
		{
			isMove = false;
		}
		//按下方向鍵
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			if (ChangeForword)
			{
				CheckForward();
			}
			isMove = true;
			isAttack = false;
			zAttack = 0;
			atkCount = 0;
		}
		//觸發翻滾
		if (Input.GetKey(KeyCode.Space))
		{
			if (isSkillToDodge == true)
			{
				isDodge = true;
			}
		}

	}
	private void DoDodgeState()
	{
		isBattle = true;
		isAttack = false;
		isMove = false;
		isSkill = false;
		isSkillToDodge = false;
		ChangeForword = false;
		isAtkToMove = false;
		zAttack = 0;
		atkCount = 0;
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("DODGE"))
		{
			isDodge = false;
		}
		//按下方向鍵
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;
		}
	}
	private void DoMoveState()
	{
		//isGitHit = false;
		anim.SetBool("isAttack", false);
		canMove = true;
		//都不按方向鍵
		if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) == true)
		{
			isMove = false;
		}
		//移動中觸發攻擊
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;
			if (Input.GetKeyDown(KeyCode.Z))
			{				
				isAttack = true;
				isBattle = true;
				zAttack = 1;
				atkCount = 1;
			}
		}
		//觸發Skill
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.V))
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
			if (Input.GetKey(KeyCode.V))
			{
				isAttack = false;
				isBattle = true;
				isSkill = true;
				anim.SetBool("Skill3", true);
			}
		}
		//觸發翻滾
		if (Input.GetKey(KeyCode.Space))
		{
			isDodge = true;
		}
	}
	private void DoGetHitState()
	{
		isBattle = true;
		canMove = false;
		isSkillToDodge = false;
		ChangeForword = false;
		isAtkToMove = false;
		anim.SetBool("isAttack", false);
		if (PlayerInfo.DizzyCount>=50)
		{
			PlayerInfo.DizzyCount = 0;
		}
		//Debug.Log("GetHit  "+ isGitHit);
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			//Debug.Log("跑完");
			isGitHit = false;			
			atkCount = 0;			
		}
		//////判斷有沒有攻擊中按住方向鍵
		//if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		//{
		//	isMove = true;			
		//}
	}
	private void DoDieState()
	{
		isDeath = false;
		isMove = false;
		isAttack = false;
		isGitHit = false;
		isSkill = false;
		isDodge = false;
		canMove = false;
		isSkillToDodge = false;
		ChangeForword = false;
		isAtkToMove = false;
		anim.Play("Die");

		GameOver.SetActive(true);
		StartCoroutine(Wait());
		
	}
	#endregion


	// Update is called once per frame
	void Update()
	{
		//偵測狀態
		//Debug.Log("目前狀態          " + mCurrentState);
		Debug.Log("DI                      "+PlayerInfo.DizzyCount);
		//判斷哪一個Attack
		zAttack = 0;
		//如果死亡了
		if (isDeath)
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
		//Boss 死亡
		if(BossAlive == false)
		{
			StartCoroutine(Wait2());
			StartCoroutine(Wait3());
		}
		mCheckState();
		//狀態做甚麼
		mDoState();
	}

	private void FixedUpdate()
	{
		if (canMove == true)
		{
			PlayControl.Move(moveSpeed);
		}
	}
	private void SkillToDodge()
    {
		isSkillToDodge = true;
	}
	private void AtkToMove()
    {
		isAtkToMove = true;
    }

	void PlayerSkillChangeForword()
	{
		if (ChangeForword == false)
		{
			ChangeForword = true;
		}
		else if (ChangeForword == true)
		{
			ChangeForword = false;
		}
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(5.0f);
		GameOverText.SetActive(true);
		if (Input.anyKey)
		{
			SceneManager.LoadScene(0);
		}
	}

	IEnumerator Wait2()
	{
		yield return new WaitForSeconds(5.0f);
		GameOverText2.SetActive(true);
		if (Input.anyKey)
		{
			SceneManager.LoadScene(0);
		}
	}
	IEnumerator Wait3()
	{
		yield return new WaitUntil(AlphaToZero);

	}

	bool AlphaToZero()
	{
		if (true)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

