using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadSceneAsync("PlaytestScene");
    }

    public void Options()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
