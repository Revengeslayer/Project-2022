using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotController : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameObject carrot;
    private static GameObject Smoke;
    private static GameObject Flash;
    private static GameObject Fire;
    private static GameObject Green;
    private static GameObject Thunder;
    private static Vector3 SetSpawnPos;
    private static List<Vector3> SetTargetVecList;
    private GameObject Target;
    private float MaxTimer;
    private static string ForATKtype;
    private static string ECarrotType;
    //09 add
    public static float skillAttack;


    public static void InsCarrot(Vector3 SpawnPos , List<Vector3> TargetVecList , string ATKtype , Vector3 scale , string CarrotType)
    {
        var a = TargetVecList.Count;
        List<GameObject> Basket = new List<GameObject>();
        SetTargetVecList = new List<Vector3>();
        for (int i = 0; i < a; i++)
        {
            GameObject carrotIns = Instantiate(Resources.Load(CarrotType)) as GameObject;
            
            carrot = carrotIns;
            Smoke = carrot.transform.Find("smoke_thick").gameObject;
            Flash = carrot.transform.Find("MuzzleFlash").gameObject;
            Fire = carrot.transform.Find("Fire_A").gameObject;

            ECarrotType = CheckCarrotType(CarrotType);

            carrot.transform.localScale = scale;
            Basket.Add(carrotIns);
            SetSpawnPos = SpawnPos;
            SetTargetVecList.Add(TargetVecList[i]);
            ForATKtype = ATKtype;
            if (ForATKtype == "A")
            {
                Smoke.SetActive(false);
                Flash.SetActive(false);
                Fire.SetActive(false);
                ECarrotType = "Noraml";
            }
            else if (ForATKtype == "B")
            {
                Smoke.SetActive(true);
                Flash.SetActive(false);
                Fire.SetActive(true);
            }
            else if (ForATKtype == "C")
            {
                Smoke.SetActive(true);
                Flash.SetActive(false);
                Fire.SetActive(true);
            }
            else if (ForATKtype == "D")
            {
                Smoke.SetActive(true);
                Flash.SetActive(true);
                Fire.SetActive(false);
            }
        }
        for (int n =0; n < a; n++)
        {
            Basket[n].transform.forward = SetTargetVecList[n];
            Basket[n].transform.position = SetSpawnPos;
        }
        
    }
    private static string CheckCarrotType(string CarrotType)
    {
        if (CarrotType == ("Weapons/carrotarrow_Variant"))
        {
            return "Green";
        }
        else if (CarrotType == ("Weapons/carrotarrow_Variant_1"))
        {
            return "Blue";
        }
        else if (CarrotType == ("Weapons/carrotarrow"))
        {
            return "Red";
        }
        else
        {
            return "Normal";
        }
    }
    public static void InsEliteArrow(Vector3 SpawnPos, List<Vector3> TargetVecList, string ATKtype, Vector3 scale)
    {
        var a = TargetVecList.Count;
        List<GameObject> Basket = new List<GameObject>();
        SetTargetVecList = new List<Vector3>();
        for (int i = 0; i < a; i++)
        {
            GameObject carrotIns = Instantiate(Resources.Load("Weapons/EliteArrow")) as GameObject;
            carrot = carrotIns;
            Smoke = carrot.transform.Find("smoke_thick").gameObject;
            //Flash = carrot.transform.Find("MuzzleFlash").gameObject;
            //Fire = carrot.transform.Find("Fire_A").gameObject;
            Green = carrot.transform.Find("ArrowAOE").gameObject;
            Thunder = carrot.transform.Find("EnergyAccumulation").gameObject;
            //Smoke.transform.localScale = scale;
            //Flash.transform.localScale = scale;
            //Fire.transform.localScale = scale;

            carrot.transform.localScale = scale;
            Basket.Add(carrotIns);
            SetSpawnPos = SpawnPos;
            SetTargetVecList.Add(TargetVecList[i]);
            ForATKtype = ATKtype;
            if (ForATKtype == "A")
            {
                Smoke.SetActive(false);
                Flash.SetActive(false);
                Fire.SetActive(false);
            }
            else if (ForATKtype == "B")
            {
                Smoke.SetActive(true);
                Flash.SetActive(false);
                Fire.SetActive(true);
            }
            else if (ForATKtype == "D")
            {
                Smoke.SetActive(false);
                //Flash.SetActive(false);
                //Fire.SetActive(false);
                Green.SetActive(true);
                Thunder.SetActive(false);
            }
            else if (ForATKtype == "C")
            {
                Smoke.SetActive(true);
                //Flash.SetActive(false);
                //Fire.SetActive(false);
                Green.SetActive(false);
                Thunder.SetActive(false);
            }

        }
        for (int n = 0; n < a; n++)
        {
            Basket[n].transform.forward = SetTargetVecList[n];
            Basket[n].transform.position = SetSpawnPos;
        }
    }

    private void Recycle(float skillAttack)
    {
        var TargetPos = Target.transform.position + new Vector3(0, 0.79f, 0);
        var Dist = (TargetPos- gameObject.transform.position).magnitude;
        var ToTargetVec = TargetPos- gameObject.transform.position;
        var HitCheckDot = Vector3.Dot(ToTargetVec, gameObject.transform.forward);
        MaxTimer += Time.deltaTime;
        if (Dist <= 4f && skillAttack == 3)
        {
            Destroy(gameObject);
        }
        else if (Dist < 0.3)
        {
            PlayerInfo.CarrotArrowDamage(ForATKtype , ECarrotType);
            Destroy(gameObject);
            if (ECarrotType == "Green")
            {
                PlayerInfo.GetArr++;
            }
        }
        else if (Dist < 0.8 && HitCheckDot < 0)
        {
            PlayerInfo.CarrotArrowDamage(ForATKtype , ECarrotType);
            Destroy(gameObject);
            if (ECarrotType == "Green")
            {
                PlayerInfo.GetArr++;
            }
        }
        
        else if(MaxTimer > 3)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Target = GameObject.Find("Character(Clone)");
        //gameObject.transform.position = SetSpawnPos;
        gameObject.transform.forward *= -1;
        MaxTimer = 0;
    }

    private void Update()
    {
        Recycle(skillAttack);
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
