using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichTrigger : MonoBehaviour
{

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name== "Character(Clone)")
        {
            Debug.Log("+++++++++++++++++++++++¶i¤J");
            Camera.main.depth = -5;
            StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(2.0f);
        Camera.main.depth = -1;
    }
}
