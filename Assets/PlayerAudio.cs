using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    int start = 0;
    private float time;
    // Start is called before the first frame update
    AudioSource[] BGMAudios;

    AudioSource Normal;
    AudioSource Battle;
    AudioSource Boss;

    AudioSource []Audios;
    AudioSource Attack;

    AudioSource Skill1_Voice;
    AudioSource Skill1_First;
    AudioSource Skill1_Second;

    AudioSource Skill2;

    AudioSource Skill3_Voice;
    AudioSource Skill3;

    AudioSource GetHit;

    AudioSource Dodge;

    AudioSource Die;

    void Start()
    {
        start = 0;
        BGMAudios = GameObject.Find("Main").GetComponents<AudioSource>();
        Normal = BGMAudios[0];
        Battle = BGMAudios[1];
        Boss = BGMAudios[2];

        Audios = gameObject.GetComponents<AudioSource>();
        Attack = Audios[0];
        Skill1_Voice = Audios[1];
        Skill1_First = Audios[2];
        Skill1_Second = Audios[3];
        Skill2 = Audios[4];
        Skill3_Voice= Audios[5];
        Skill3 = Audios[6];
        GetHit = Audios[7];
        Dodge = Audios[8];
        Die = Audios[9];
    }

    // Update is called once per frame
    void Update()
    {
        CheckBGM();
    }

    private void CheckBGM()
    {
        if (start == 0)
        {
            StartCoroutine(Wait3());
        }
        if (InstantiateManager.aliveCount==0 && Normal.isPlaying ==true)
        {
            
            return;
        }
        else if(InstantiateManager.aliveCount == 0 && Battle.isPlaying == true)
        {
            Normal.volume = 0f;
            StartCoroutine(BattleStop());
            StartCoroutine(ChangeToNormal());         
        }

    }
    IEnumerator NormalPause()
    {
        yield return new WaitUntil(NormalVolumeDown);
        Normal.Pause();
    }
    IEnumerator BattleStop()
    {
        yield return new WaitUntil(BattleVolumeDown);
        Battle.Stop();
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        Battle.volume = 0f;
        StartCoroutine(BattleFadeIn());
    }
    IEnumerator Wait2()
    {
        yield return new WaitForSeconds(0.1f);
        Boss.volume = 0f;
        StartCoroutine(BossFadeIn());
    }

    IEnumerator Wait3()
    {
        yield return new WaitUntil(StartVolumeUp);
        start++;
    }
    IEnumerator ChangeToNormal()
    {
        Normal.Play();
        yield return new WaitUntil(NormalVolumeUP);
    }

    IEnumerator BattleFadeIn()
    {
        Battle.Play();
        yield return new WaitUntil(BattleVolumeUP);
    }
    IEnumerator BossFadeIn()
    {
        Boss.Play();
        yield return new WaitUntil(BossVolumeUP);
    }
    bool NormalVolumeUP()
    {
        Normal.volume += Time.deltaTime*0.1f;
        Normal.volume = Mathf.Clamp(Normal.volume, 0f, 0.5f);
        if (Normal.volume>=0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool BattleVolumeUP()
    {
        Battle.volume += Time.deltaTime * 0.5f;
        Battle.volume = Mathf.Clamp(Battle.volume, 0f, 0.5f);
        if (Battle.volume >= 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool BossVolumeUP()
    {
        Boss.volume += Time.deltaTime * 0.2f;
        Boss.volume = Mathf.Clamp(Boss.volume, 0f, 0.5f);
        if (Boss.volume >= 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    bool NormalVolumeDown()
    {
        Normal.volume -= Time.deltaTime * 0.5f;
        Normal.volume = Mathf.Clamp(Normal.volume, 0f, 0.5f);
        if (Normal.volume == 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool BattleVolumeDown()
    {
        Battle.volume -= Time.deltaTime * 0.5f;
        Battle.volume = Mathf.Clamp(Battle.volume, 0f, 0.5f);
        if (Battle.volume == 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool StartVolumeUp()
    {
        Normal.volume += Time.deltaTime * 0.01f;
        Normal.volume = Mathf.Clamp(Normal.volume, 0f, 0.5f);
        if(Normal.volume>=0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.name == "SpawnA"|| other.name == "SpawnB"|| other.name == "SpawnC"|| other.name == "SpawnD"|| other.name == "SpawnE"))
        {
            start++;
            StartCoroutine(NormalPause());
            StartCoroutine(Wait());
        }
        
        if (other.name == "AreaBoss")
        {
            StartCoroutine(NormalPause());
            StartCoroutine(Wait2());
        }
    }
    #region Audio
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
    void PlayAudioGetHit()
    {
        GetHit.Play();
    }
    void PlayAudioDodge()
    {
        Dodge.Play();
    }

    void PlayAudiDie()
    {
        Die.Play();
    }
    #endregion

}
