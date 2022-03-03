using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource []Audios;
    AudioSource Attack;
    AudioSource Skill;
    void Start()
    {
        Audios = gameObject.GetComponents<AudioSource>();
        Attack = Audios[0];
        Skill = Audios[1];
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void PlayAudioZ()
    {
        Debug.Log("77777777777");
        Attack.Play();
    }

    void PlayAudioSkill2()
    {
        Skill.Play();
    }
}
