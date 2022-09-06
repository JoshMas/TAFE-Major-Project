using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class opition : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
    }

    public void Play()
    {
       
    }

    public void Options()
    {
        SceneManager.LoadSceneAsync("OptionsMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
