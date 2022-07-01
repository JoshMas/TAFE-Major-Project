using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public delegate void Paused(bool _isPaused);
    public event Paused IsPaused;

    public delegate void PlayerLost();
    public event PlayerLost HasLost;

    public delegate void PlayerWon();
    public event PlayerWon HasWon;

    private Vector3 spawnPosition;

    private void Singleton()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        Singleton();
        Debug.Log(spawnPosition);
    }

    public void Pause()
    {
        if (Time.timeScale != 0)
        {
            IsPaused?.Invoke(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
        }
        else
        {
            IsPaused?.Invoke(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            SceneManager.UnloadSceneAsync("PauseMenu");
        }
    }

    public void Win()
    {
        HasWon?.Invoke();
        SetSpawnpoint(Vector3.zero);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync("Win", LoadSceneMode.Additive);
    }

    public void Lose()
    {
        HasLost?.Invoke();
        Invoke(nameof(GameOverScreen), 1);
    }

    private void GameOverScreen()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync("GameOver", LoadSceneMode.Additive);
    }

    public void PlaceAtSpawnpoint(Transform _objectToPlace)
    {
        _objectToPlace.position = spawnPosition;
    }

    public void SetSpawnpoint(Vector3 _newPosition)
    {
        spawnPosition = _newPosition;
    }
}
