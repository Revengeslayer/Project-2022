using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource []Audios;
    AudioSource Attack;
    AudioSource Skill1_Voice;
    AudioSource Skill1_First;
    AudioSource Skill1_Second;

    AudioSource Skill2;

    AudioSource Skill3_Voice;
    AudioSource Skill3;
    void Start()
    {
        Audios = gameObject.GetComponents<AudioSource>();
        Attack = Audios[0];
        Skill1_Voice = Audios[1];
        Skill1_First = Audios[2];
        Skill1_Second = Audios[3];
        Skill2 = Audios[4];
        Skill3_Voice= Audios[5];
        Skill3 = Audios[6];
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void PlayAudioZ()
    {
        Attack.Play();
    }
    void PlayAudioSkill1_Voice()
    {
        Skill1_Voice.Play();
    }
    void PlayAudioSkill1_First()
    {
        Skill1_First.Play();
    }

    void PlayAudioSkill1_Second()
    {
        Skill1_Second.Play();
    }
    void PlayAudioSkill2()
    {
        Skill2.Play();
    }

    void PlayAudioSkill3_Voice()
    {
        Skill3_Voice.Play();
    }
    void PlayAudioSkill3()
    {
        Skill3.Play();
    }
}
