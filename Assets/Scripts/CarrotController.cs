using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotController : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameObject carrot;
    private static Vector3 SetSpawnPos;
    private static List<Vector3> SetTargetVecList;
    private GameObject Target;
    private float MaxTimer;


    public static void InsCarrot(Vector3 SpawnPos , List<Vector3> TargetVecList , string ATKtype)
    {
        var a = TargetVecList.Count;
        List<GameObject> Basket = new List<GameObject>();
        SetTargetVecList = new List<Vector3>();
        for (int i = 0; i < a; i++)
        {
            GameObject carrotIns = Instantiate(Resources.Load("Weapons/carrotarrow")) as GameObject;
            carrot = carrotIns;
            Basket.Add(carrotIns);
            SetSpawnPos = SpawnPos;
            SetTargetVecList.Add(TargetVecList[i]);
        }
        for (int n =0; n < a; n++)
        {
            Basket[n].transform.forward = SetTargetVecList[n];
            Basket[n].transform.position = SetSpawnPos;
        }
        
    }
    private void Recycle()
    {
        var TargetPos = Target.transform.position + new Vector3(0, 0.79f, 0);
        var Dist = (TargetPos- gameObject.transform.position).magnitude;
        var ToTargetVec = TargetPos- gameObject.transform.position;
        var HitCheckDot = Vector3.Dot(ToTargetVec, gameObject.transform.forward);
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
        gameObject.transform.forward *= -1;
        //gameObject.transform.localScale = new Vector3(3, 3, 1);
        MaxTimer = 0;
    }

    private void Update()
    {
        Recycle();
    }
    private void FixedUpdate()
    {
        gameObject.transform.position -= gameObject.transform.forward * Time.deltaTime * 8;
        //Recycle();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.3f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, 0.8f);
    }
}
