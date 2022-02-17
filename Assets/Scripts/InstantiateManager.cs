using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateManager : MonoBehaviour
{
    public Object Target;
    public Object prefab;
    public int Nums;
    //private List<GameObject> SpawnPosContainer;
    private GameObject[] SpawnPosContainer;
    private Vector3 rabaSpawn;

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
    private void Awake()
    {
        InitGoData(prefab, Nums);
        LoadedGo = new List<GameObject>();
        SpawnPosContainer = GameObject.FindGameObjectsWithTag("rabaSpawn");
        //rabaSpawn = GameObject.FindGameObjectWithTag("rabaSpawn").transform.position;
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            int iCount = InsGoDataContainer.Count;
            if (Nums > LoadedGo.Count)
            {
                int SpawnNum = Random.Range(0, 7);
                var go = LoadGoData();
                go.SetActive(true);
                go = SpawnPosContainer[SpawnNum];
                LoadedGo.Add(go);
                Debug.Log("123");
            }
            Debug.Log("456");
        }
    }
    
}
