using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetSceneAt(0).buildIndex);
    }

    public void Return()
    {
        GameManager.Instance.ResetLevelProgress();
        SceneManager.LoadSceneAsync("LevelSelect");
    }
}
