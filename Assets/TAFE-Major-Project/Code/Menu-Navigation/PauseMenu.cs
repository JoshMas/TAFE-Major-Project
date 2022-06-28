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

    public void ReturnToTitle()
    {
        GameManager.Instance.SetSpawnpoint(Vector3.zero);
        GameManager.Instance.Pause();
        SceneManager.LoadSceneAsync("TitleScreen");
    }

    public void Quit()
    {
        //Add something here for saving when I get around to it

        Application.Quit();
    }
}
