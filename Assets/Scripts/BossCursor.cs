using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCursor : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("´å¼Ð¹Ï¥Ü")]
    public GameObject prefab;

    public GameObject mapStart;
    public GameObject mapEnd;
    public GameObject minStart;
    public GameObject minEnd;

    public GameObject plane;

    private GameObject boss;
    void Start()
    {                                                                
        Vector3 mobMiniPos = new Vector3(((this.gameObject.transform.position.x - mapStart.transform.position.x) / (mapEnd.transform.position.x - mapStart.transform.position.x)) * (minEnd.transform.position.x - minStart.transform.position.x) + minStart.transform.position.x, plane.transform.position.y + 0.06f, ((this.gameObject.transform.position.z - mapStart.transform.position.z) / (mapEnd.transform.position.z - mapStart.transform.position.z)) * (minEnd.transform.position.z - minStart.transform.position.z) + minStart.transform.position.z);
        boss=Instantiate(prefab, mobMiniPos, this.gameObject.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        boss.transform.forward = this.gameObject.transform.forward;
        boss.transform.position = GameToMapPos(this.gameObject);
    }

    private Vector3 GameToMapPos(GameObject mob)
    {
        return new Vector3(((mob.transform.position.x - mapStart.transform.position.x) / (mapEnd.transform.position.x - mapStart.transform.position.x)) * (minEnd.transform.position.x - minStart.transform.position.x) + minStart.transform.position.x, plane.transform.position.y + 0.06f, ((mob.transform.position.z - mapStart.transform.position.z) / (mapEnd.transform.position.z - mapStart.transform.position.z)) * (minEnd.transform.position.z - minStart.transform.position.z) + minStart.transform.position.z); ;
    }
}
