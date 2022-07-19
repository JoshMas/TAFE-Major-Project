using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public void Return()
    {
        SceneManager.LoadSceneAsync("LevelSelect");
    }

    public void Replay()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetSceneAt(0).buildIndex);
    }
}
