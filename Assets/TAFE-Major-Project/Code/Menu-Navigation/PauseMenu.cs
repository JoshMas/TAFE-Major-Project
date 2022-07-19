using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.Pause(false);
    }

    public void Options()
    {
        Debug.Log("Options menu under construction.");
    }

    public void ReturnToTitle()
    {
        GameManager.Instance.ResetLevelProgress();
        SceneManager.LoadSceneAsync("TitleScreen");
    }

    public void Quit()
    {
        //Add something here for saving when I get around to it

        Application.Quit();
    }
}
