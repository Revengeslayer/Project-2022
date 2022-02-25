using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateManager : MonoBehaviour
{
    private Object Target;
    public Object RA01;
    public int RANums1;
    private List<InstantiateData> RAInsGoDataContainer1;
    public Object RA02;
    public int RANums2;
    private List<InstantiateData> RAInsGoDataContainer2;
    public Object RA03;
    public int RANums3;
    private List<InstantiateData> RAInsGoDataContainer3;
    public Object RA04;
    public int RANums4;
    private List<InstantiateData> RAInsGoDataContainer4;
    public Object RW01;
    public int RWNums1;
    private List<InstantiateData> RWInsGoDataContainer1;
    public Object RW02;
    public int RWNums2;
    private List<InstantiateData> RWInsGoDataContainer2;
    public Object RW03;
    public int RWNums3;
    private List<InstantiateData> RWInsGoDataContainer3;
    public Object RW04;
    public int RWNums4;
    private List<InstantiateData> RWInsGoDataContainer4;
    public static string stringTag;
    private List<SpawnData> SpawnedList;
    private GameObject[] SpawnPosContainer;
    private Vector3 rabaSpawn;
    public static bool Spawn;
    public static float GetSpawnNums;

    #region ForINS
    public class InstantiateData
    {
        public GameObject insGo;
        public bool setOn;
        public int goNums;
        public string goType;
    }
    private static InstantiateManager mInstance;

    private List<InstantiateData> InsGoDataContainer;
    private List<GameObject> LoadedGo;

    public InstantiateManager()
    {
        mInstance = this;
    }

    /// <summary>
    /// Initaite prefab (iCount) times 
    /// New a Container
    /// Instantiate a prefab as GO
    /// setFalse & Store GO into InstantiateData(class)
    /// Add InstantiateData(class) into Container List
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="Nums"></param>
    /// 生成prefab後開一個class來存 , 再把class加進List容器
    public List<InstantiateData> InitGoData(Object prefab, int Nums)
    {
        List<InstantiateData>  GoDataContainer = new List<InstantiateData>();
        string prefabName = prefab.name;
        for (int i = 0; i < Nums; i++)
        {
            GameObject go = GameObject.Instantiate(prefab) as GameObject;
            go.SetActive(false);
            InstantiateData insData = new InstantiateData();
            insData.setOn = false;
            insData.insGo = go;
            insData.goNums = i;
            insData.goType = prefabName;
            GoDataContainer.Add(insData);
        }
        return GoDataContainer;
    }
    /// <summary>
    /// Check ContainerLength and do forloop
    /// get & setActive a GameObject per called in forloop
    /// </summary>
    /// <returns></returns>
    /// 呼叫時會從List容器 以i為引數做loop來搜尋List內第一個未啟用(false)的column(Class)
    /// 再把Class中的GO存進LoadGO回傳
    public GameObject LoadGoData()
    {
        int iCount = InsGoDataContainer.Count;
        GameObject LoadGo = null;
        for (int i = 0; i < iCount; i++)
        {
            if (InsGoDataContainer[i].setOn == false)
            {
                LoadGo = InsGoDataContainer[i].insGo;
                InsGoDataContainer[i].setOn = true;
                break;
            }
        }
        return LoadGo;
    }
    /// <summary>
    /// Check ContainerLength and do forloop
    /// Use forloop to check go in random column
    /// Unload go by setFalse
    /// </summary>
    /// <param name="go"></param>
    /// 呼叫時給定隨機column中的GO作為引數
    /// 用forloop來搜尋引數指向的column , setFalse來作為Unload
    public void UnLoadObjData(GameObject go)
    {
        int iCount = InsGoDataContainer.Count;
        for (int i = 0; i < iCount; i++)
        {
            if (InsGoDataContainer[i].insGo == go)
            {
                InsGoDataContainer[i].insGo.SetActive(false);
                InsGoDataContainer[i].setOn = false;
                break;
            }

        }
    }
    #endregion
    //public List<GameObject> SetSpawnPos()
    //{
    //    GameObject Pos;
    //    for(int i = 0 ; i < SpawnPosContainer.Count ; i++ )
    //    {
    //        Pos = GameObject.FindGameObjectsWithTag("rabaSpawn");
    //        SpawnPosContainer.Add(Pos);
    //    }
    //    return SpawnPosContainer;
    //}
    public class SpawnData
    {
        public GameObject Pos = null;
        public bool Spawned;
        public string Area;
    }
    private List<SpawnData> SpawnPosInitializer(string SpawnPosTag)
    {
        var SpawnPosContainer = GameObject.FindGameObjectsWithTag(SpawnPosTag);
        List<SpawnData> SpawnPosList = new List<SpawnData>();
        foreach (GameObject SpawnPos in SpawnPosContainer)
        {
            {
                SpawnData SD = new SpawnData();
                SD.Pos = SpawnPos;
                SD.Area = SpawnPosTag;
                SD.Spawned = false;
                SpawnPosList.Add(SD);
            }
        }
        //Debug.Log(SpawnPosList.Count);
        return SpawnPosList;
    }
    //IEnumerator SpawnTimer(InstantiateData ID)
    //{
    //    int b = Random.Range(2, 10);
    //    float a = 1 / b;
    //    yield return new WaitForSeconds(a);
    //    ID.insGo.SetActive(true);
    //    yield break;
    //}
    private void SetSpawnPos(List<InstantiateData> InsGoDataContainer, List<SpawnData> SpawnPosList , int SpawnNums)
    {
        InstantiateData ID;
        SpawnData SD;
        for (int i = 0; i < SpawnNums; i ++)
        {
            int ranPos = Random.Range(0, SpawnPosList.Count);
            ID = InsGoDataContainer[i];
            SD = SpawnPosList[ranPos];
            ID.setOn = true;
            ID.insGo.transform.position = SD.Pos.transform.position;
            SpawnPosList.RemoveAt(ranPos);
            ID.insGo.SetActive(true);
            //Debug.Log(SpawnPosList.Count);
        }
    }
    private void Awake()
    {
        RAInsGoDataContainer1 = new List<InstantiateData>();
        RAInsGoDataContainer1 = InitGoData(RA01, RANums1);
        RAInsGoDataContainer2 = new List<InstantiateData>();
        RAInsGoDataContainer2 = InitGoData(RA02, RANums2);
        RAInsGoDataContainer3 = new List<InstantiateData>();
        RAInsGoDataContainer3 = InitGoData(RA03, RANums3);
        RAInsGoDataContainer4 = new List<InstantiateData>();
        RAInsGoDataContainer4 = InitGoData(RA04, RANums4);

        RWInsGoDataContainer1 = new List<InstantiateData>();
        RWInsGoDataContainer1 = InitGoData(RW01, RWNums1);
        RWInsGoDataContainer2 = new List<InstantiateData>();
        RWInsGoDataContainer2 = InitGoData(RW02, RWNums2);
        RWInsGoDataContainer3 = new List<InstantiateData>();
        RWInsGoDataContainer3 = InitGoData(RW03, RWNums4);
        RWInsGoDataContainer4 = new List<InstantiateData>();
        RWInsGoDataContainer4 = InitGoData(RW04, RWNums4);
        LoadedGo = new List<GameObject>();
        //SpawnPosList = new List<SpawnData>();
        SpawnedList = new List<SpawnData>();
        //SpawnPosContainer = GameObject.FindGameObjectsWithTag("rabaSpawn");
        //SpawnPosInitializer();
        //rabaSpawn = GameObject.FindGameObjectWithTag("rabaSpawn").transform.position;        
    }
    void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        ////if(Spawn)
        //{
        //    //GetSpawnNums = 1;
        //    int iCount = InsGoDataContainer.Count;
        //    for (int i = 0; i < 50; i++)
        //    {
        //        //if (Nums > LoadedGo.Count)
        //        {
        //            //int SpawnNum = Random.Range(0, SpawnPosList.Count);
        //            var go = LoadGoData();
        //            go.SetActive(true);
                                        
        //            go.transform.position = SetSpawnPos();
        //            LoadedGo.Add(go);
        //        }
        //    }
        //    Spawn = false;
        //}
        if (Spawn)
        {
            var SpawnPosList = SpawnPosInitializer(stringTag);
            //Debug.Log(stringTag);
            //Debug.Log(SpawnPosList.Count);
            var SpawnPos = SpawnPosList[0];
            //Debug.Log(SpawnPos.Area);
            if(SpawnPos.Area == "SpawnA")
            {
                RabbitArcherSteeringFSM.SpawnArea = SpawnPos.Area;
                SetSpawnPos(RAInsGoDataContainer1, SpawnPosList , 1);
                SetSpawnPos(RAInsGoDataContainer4, SpawnPosList , 3);
                SetSpawnPos(RAInsGoDataContainer2, SpawnPosList , 3);
            }
            else if (SpawnPos.Area == "SpawnB")
            {
                RabbitArcherSteeringFSM.SpawnArea = SpawnPos.Area;
                SetSpawnPos(RAInsGoDataContainer3, SpawnPosList, 2);
                SetSpawnPos(RAInsGoDataContainer4, SpawnPosList, 4);
                SetSpawnPos(RAInsGoDataContainer2, SpawnPosList, 3);
                //SetSpawnPos()
            }
            else if (SpawnPos.Area == "SpawnC")
            {
                RabbitArcherSteeringFSM.SpawnArea = SpawnPos.Area;
                SetSpawnPos(RAInsGoDataContainer3, SpawnPosList, 4);
                SetSpawnPos(RAInsGoDataContainer4, SpawnPosList, 2);
                SetSpawnPos(RAInsGoDataContainer2, SpawnPosList, 3);
                //SetSpawnPos
            }
            else if (SpawnPos.Area == "SpawnD")
            {
                RabbitArcherSteeringFSM.SpawnArea = SpawnPos.Area;
                SetSpawnPos(RAInsGoDataContainer3, SpawnPosList, 3);
                SetSpawnPos(RAInsGoDataContainer4, SpawnPosList, 2);
                SetSpawnPos(RAInsGoDataContainer2, SpawnPosList, 4);
                //SetSpawnPos
            }
            else if (SpawnPos.Area == "SpawnE")
            {
                RabbitArcherSteeringFSM.SpawnArea = SpawnPos.Area;
                SetSpawnPos(RAInsGoDataContainer3, SpawnPosList, 3);
                SetSpawnPos(RAInsGoDataContainer4, SpawnPosList, 3);
                SetSpawnPos(RAInsGoDataContainer2, SpawnPosList, 3);
                //SetSpawnPos
            }
            //SetSpawnPos()
            Spawn = false;
        }
    }
    
}
