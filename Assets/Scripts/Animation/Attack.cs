using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
          animator.SetBool("isAttack", true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (animator.GetInteger("atkCount") >= 2)
        //{         
        //    if (stateInfo.IsName("Atk1"))
        //    {
        //        animator.SetBool("hit1", false);
        //        animator.SetBool("hit2", true);
        //    }
        //}
        //if (stateInfo.normalizedTime > 0.7f && stateInfo.IsName("Atk2"))
        //{
        //    animator.SetBool("hit2", false);
        //    if (animator.GetInteger("atkCount") >= 3)
        //    {
        //        animator.SetBool("hit3", true);
        //    }
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetInteger("atkCount", 0);
        animator.SetBool("isAttack", false);
        //animator.applyRootMotion = (false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
