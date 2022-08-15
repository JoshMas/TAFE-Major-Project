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

    private Vector3 currentSpawnPosition;
    private bool isStartingLevel = true;
    private InputManager inputManager;

    [SerializeField] private KeybindObject defaultKeybinds;
    [SerializeField] private KeybindObject actualKeybinds;

    public KeybindObject DefaultKeybinds => defaultKeybinds;
    public KeybindObject ActualKeybinds => actualKeybinds;


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
        inputManager = GetComponent<InputManager>();
        actualKeybinds.LoadKeybinds();
    }

    public void UIMode(bool _uiMode)
    {
        inputManager.enabled = !_uiMode;
        Time.timeScale = _uiMode ? 0 : 1;
    }

    public void Pause(bool _isPaused)
    {
        if (_isPaused)
        {
            IsPaused?.Invoke(true);
            UIMode(true);
            SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
        }
        else
        {
            IsPaused?.Invoke(false);
            UIMode(false);
            SceneManager.UnloadSceneAsync("PauseMenu");
        }
    }

    public void Win()
    {
        HasWon?.Invoke();
        ResetLevelProgress();
        UIMode(true);
        SceneManager.LoadSceneAsync("Win", LoadSceneMode.Additive);
    }

    public void Lose()
    {
        HasLost?.Invoke();
        inputManager.enabled = false;
        Invoke(nameof(GameOverScreen), 1);
    }

    private void GameOverScreen()
    {
        UIMode(true);
        SceneManager.LoadSceneAsync("GameOver", LoadSceneMode.Additive);
    }

    public void PlaceAtSpawnpoint(Transform _objectToPlace)
    {
        _objectToPlace.position = currentSpawnPosition;
    }

    public void SetSpawnpoint(Vector3 _newPosition)
    {
        currentSpawnPosition = _newPosition;
    }

    public void StartLevel(Vector3 _startingSpawn)
    {
        if (isStartingLevel)
        {
            SetSpawnpoint(_startingSpawn);
            isStartingLevel = false;
        }
    }

    public void ResetKeybinds()
    {
        for(int i = 0; i < actualKeybinds.keybinds.Length; ++i)
        {
            actualKeybinds.keybinds[i] = DefaultKeybinds.keybinds[i];
        }
        actualKeybinds.SaveKeybinds();
    }

    public void SetKeybinds(KeybindObject _object)
    {
        actualKeybinds = _object;
        actualKeybinds.SaveKeybinds();
    }

    public void ResetLevelProgress()
    {
        isStartingLevel = true;
    }
}
