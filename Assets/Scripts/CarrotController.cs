using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotController : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameObject carrot;
    private static Vector3 SetSpawnPos;
    private static Vector3 SetTargetVec;
    private GameObject Target;
    private float MaxTimer;


    public static void InsCarrot(Vector3 SpawnPos , Vector3 TargetVec)
    {
        GameObject carrotIns = Instantiate(Resources.Load("Weapons/carrotarrow")) as GameObject;
        carrot = carrotIns;
        SetSpawnPos = SpawnPos;
        SetTargetVec = TargetVec;
    }
    private void Recycle()
    {
        var Dist = (Target.transform.position - gameObject.transform.position).magnitude;
        var ToTargetVec = Target.transform.position - gameObject.transform.position;
        var HitCheckDot = Vector3.Dot(ToTargetVec, SetTargetVec);
        MaxTimer += Time.deltaTime;
        if (Dist < 0.3)
        {
            PlayerInfo.CarrotArrowDamage();
            Destroy(gameObject);
        }
        else if (Dist < 0.8 && HitCheckDot < 0)
        {
            PlayerInfo.CarrotArrowDamage();
            Destroy(gameObject);
        }
        else if(MaxTimer > 3)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Target = GameObject.Find("Character(Clone)");
        gameObject.transform.position = SetSpawnPos;
        gameObject.transform.forward = -SetTargetVec;
        MaxTimer = 0;
    }
    
    private void FixedUpdate()
    {
        gameObject.transform.position -= gameObject.transform.forward * Time.deltaTime * 8;
        Recycle();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.3f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, 0.8f);
    }
}
