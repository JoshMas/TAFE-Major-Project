using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(SceneManager.GetSceneAt(0).buildIndex);
    }

    public void Return()
    {
        GameManager.Instance.SetSpawnpoint(Vector3.zero);
        SceneManager.LoadSceneAsync("LevelSelect");
    }
}
