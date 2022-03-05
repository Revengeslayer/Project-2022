using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    GameObject audioObject;
    AudioSource[] audios;
    AudioSource Menu;
    AudioSource Selection;
    private void Start()
    {
        audioObject = GameObject.Find("GameObject");
        audios = audioObject.GetComponents<AudioSource>();

        Menu = audios[0];
        Selection = audios[1];
    }
    // Start is called before the first frame update
    public  void GameStart()
    {
        Selection.Play();
        StartCoroutine(FadeOut());
    }

    public void GameExit()
    {

        // UnityEditor.EditorApplication.isPlaying = false;
        Selection.Play();
        Application.Quit();
    }
    IEnumerator FadeOut()
    {       
        yield return new WaitUntil(MenuDown);
        SceneManager.LoadScene(1);

    }
    bool MenuDown()
    {
        Menu.volume -= Time.deltaTime * 0.25f;
        Menu.volume = Mathf.Clamp(Menu.volume, 0f, 0.5f);
        if (Menu.volume == 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
