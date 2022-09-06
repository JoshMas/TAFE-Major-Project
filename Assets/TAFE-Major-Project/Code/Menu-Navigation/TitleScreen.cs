using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
    }

    public void Play()
    {
        SceneManager.LoadSceneAsync("LevelSelect");
    }

    public void menu ()
    {
        SceneManager.LoadSceneAsync("TitleScreen");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
