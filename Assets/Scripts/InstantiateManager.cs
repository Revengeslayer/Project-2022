using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateManager : MonoBehaviour
{
    public Object Target;
    public Object prefab;
    public int Nums;
    private List<SpawnData> SpawnPosList;
    private List<SpawnData> SpawnedList;
    private GameObject[] SpawnPosContainer;
    private Vector3 rabaSpawn;
    public static bool Spawn;

    #region ForINS
    public class InstantiateData
    {
        public GameObject insGo;
        public bool setOn;
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
    /// <param name="iCount"></param>
    /// 生成prefab後開一個class來存 , 再把class加進List容器
    public void InitGoData(Object prefab, int iCount)
    {
        InsGoDataContainer = new List<InstantiateData>();
        for (int i = 0; i < iCount; i++)
        {
            GameObject go = GameObject.Instantiate(prefab) as GameObject;
            go.SetActive(false);
            InstantiateData insData = new InstantiateData();
            insData.setOn = false;
            insData.insGo = go;
            InsGoDataContainer.Add(insData);
        }
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
        public bool Spwaned;
    }
    private void SpawnPosInitializer()
    {
        SpawnPosContainer = GameObject.FindGameObjectsWithTag("rabaSpawn");
        foreach (GameObject SpawnPos in SpawnPosContainer)
        {
            //for(int i = 0; i < SpawnPosContainer.Length; i ++)
            {
                SpawnData SD = new SpawnData();
                SD.Pos = SpawnPos;
                SpawnPosList.Add(SD);
                Debug.Log(SpawnPosList.Count + "count");
            }
        }
    }
    private Vector3 SetSpawnPos()
    {
        //SpawnData SD = new SpawnData();
        GameObject a;
        //for (int i = 0; i < SpawnPosList.Count; i ++)
        foreach (SpawnData SD in SpawnPosList)
        {
            //SD = SpawnPosList[i];
            if (!SD.Spwaned)
            {
                a = SD.Pos;
                rabaSpawn = a.transform.position;
                SD.Spwaned = true;
                SpawnedList.Add(SD);
                SD.Pos.SetActive(false);
                //SpawnPosList.RemoveAt(i);
                break;
            }
        }
        return rabaSpawn;
    }
    private void Awake()
    {
        InitGoData(prefab, Nums);
        LoadedGo = new List<GameObject>();
        SpawnPosList = new List<SpawnData>();
        SpawnedList = new List<SpawnData>();
        //SpawnPosContainer = GameObject.FindGameObjectsWithTag("rabaSpawn");
        SpawnPosInitializer();
        //rabaSpawn = GameObject.FindGameObjectWithTag("rabaSpawn").transform.position;
        Debug.Log(SpawnPosList.ToString());
        Debug.Log(SpawnPosContainer.Length);

    }
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        if(Spawn)
        {
            int iCount = InsGoDataContainer.Count;
            for (int i = 0; i < Nums; i++)
            {
                if (Nums > LoadedGo.Count)
                {
                    //int SpawnNum = Random.Range(0, SpawnPosList.Count);
                    var go = LoadGoData();
                    go.SetActive(true);

                    go.transform.position = SetSpawnPos();
                    LoadedGo.Add(go);
                }
            }
            Spawn = false;
            Debug.Log("456");
        }
    }

}
