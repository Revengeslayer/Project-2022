using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource Audio;
    void Start()
    {
        Audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    void PlayAudioZ1()
    {
        Audio.Play();
    }
}
