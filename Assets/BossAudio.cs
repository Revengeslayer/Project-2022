using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudio : MonoBehaviour
{
    AudioSource[] Audios;
    AudioSource Attack;
    AudioSource Debut;
    AudioSource Jump;
    AudioSource Roll;
    AudioSource Track;
    AudioSource Walk;
    AudioSource Die;
    // Start is called before the first frame update
    void Start()
    {
        Audios = gameObject.GetComponents<AudioSource>();
        Attack = Audios[0];
        Debut = Audios[1];
        Jump = Audios[2];
        Roll = Audios[3];
        Track = Audios[4];
        Walk = Audios[5];
        Die = Audios[6];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayAudioAtk()
    {
        Attack.Play();
    }
    void PlayAudioDebut()
    {
        Debut.Play();
    }
    void PlayAudioJump()
    {
        Jump.Play();
    }
    void PlayAudioRoll()
    {
        Roll.Play();
    }
    void CloseAudioRoll()
    {
        Roll.Stop();
    }
    void PlayAudioTrack()
    {
        Track.Play();
    }
    void PlayAudioWalk()
    {
        Walk.Play();
    }
    void PlayAudioDie()
    {
        Die.Play();
    }
}
