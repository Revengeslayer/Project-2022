using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Idle;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Active;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Attack1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Attack2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Stand;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Walk;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Roll;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Die;
        }
    }
}
