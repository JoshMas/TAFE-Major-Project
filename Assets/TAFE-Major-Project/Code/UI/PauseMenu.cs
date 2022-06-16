using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.Pause();
    }

    public void Options()
    {
        Debug.Log("Options menu under construction.");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
