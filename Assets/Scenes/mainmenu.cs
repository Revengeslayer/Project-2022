using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    public static void GameExit()
    {
       // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
