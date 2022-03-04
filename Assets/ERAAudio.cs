using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERAAudio : MonoBehaviour
{
    AudioSource[] Audios;
    AudioSource Attack;
    AudioSource PAttack;
    AudioSource GetHit;
    AudioSource Die;
    void Start()
    {
        Audios = gameObject.GetComponents<AudioSource>();
        Attack = Audios[0];
        PAttack = Audios[1];
        GetHit = Audios[2];
        Die = Audios[3];
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayAudioAtk()
    {
        Attack.Play();
    }

    void PlayAudioPAtk()
    {
        PAttack.Play();
    }
    void PlayAudioGetHit()
    {
        GetHit.Play();
    }
    void PlayAudioDie()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        Die.Play();
    }
}
