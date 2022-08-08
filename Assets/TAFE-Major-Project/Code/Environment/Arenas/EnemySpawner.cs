using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveTriggerPair
{
    [SerializeField]
    private EnemyWave wave;
    [SerializeField, Tooltip("triggers the next wave when: -1 = when this wave is killed, >0 = this many seconds pass")]
    private float nextWaveTrigger;

    public EnemyWave GetWave()
    {
        return wave;
    }

    public bool ShouldSpawn(int currentWaveCount, float _time)
    {
        if(currentWaveCount == 0)
        {
            return true;
        }

        if(nextWaveTrigger == -1)
        {
            return false;
        }

        return _time >= nextWaveTrigger;
    }
}

public class EnemySpawner : MonoBehaviour
{
    public delegate void ArenaCleared();
    /// <summary>
    /// Invoke this event when the enemies are all defeated
    /// OnArenaClear?.Invoke();
    /// </summary>
    public static event ArenaCleared OnArenaClear;

    [SerializeField] private bool isEndless = false;
    [SerializeField] private WaveTriggerPair[] waves;

    private Transform[] groundSpawnPositions;
    private Transform[] airSpawnPositions;

    private List<List<GameObject>> enemiesToSpawn;
    private List<GameObject> currentEnemies;
    private int waveMarker = 0;
    private float waveTimer = 0;

    private void Awake()
    {
        enemiesToSpawn = new List<List<GameObject>>();
        currentEnemies = new List<GameObject>();
    }

    private void Start()
    {
        List<Transform> groundTransforms = new List<Transform>(GetComponentsInChildren<Transform>());
        groundTransforms.Remove(transform);
        List<Transform> airTransforms = new List<Transform>();

        foreach(Transform transform in groundTransforms)
        {
            if (transform.CompareTag("Air"))
            {
                airTransforms.Add(transform);
            }
        }
        foreach(Transform transform in airTransforms)
        {
            groundTransforms.Remove(transform);
        }

        groundSpawnPositions = groundTransforms.ToArray();
        airSpawnPositions = airTransforms.ToArray();

        foreach(WaveTriggerPair wave in waves)
        {
            List<GameObject> newWave = wave.GetWave().GetEnemies();
            enemiesToSpawn.Add(newWave);
        }

        SpawnNextWave();
    }

    private void Update()
    {
        currentEnemies.RemoveAll(item => item == null);
        if(waveMarker < waves.Length)
        {
            waveTimer += Time.deltaTime;

            if (waves[waveMarker].ShouldSpawn(currentEnemies.Count, waveTimer))
            {
                waveMarker++;
                if (isEndless)
                {
                    waveMarker %= waves.Length;
                }
                SpawnNextWave();
                waveTimer = 0;
            }
        }
        else
        {
            if(currentEnemies.Count == 0)
            {
                OnArenaClear?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }

    //private bool IsSpawning()
    //{
    //    return isEndless ? true : waveMarker < waves.Length;
    //}

    private void SpawnNextWave()
    {
        if(waveMarker >= waves.Length)
        {
            return;
        }

        foreach(GameObject enemy in enemiesToSpawn[waveMarker])
        {
            currentEnemies.Add(Instantiate(enemy, GetSpawnPosition(enemy), Quaternion.identity));
        }
    }

    private Vector3 GetSpawnPosition(GameObject _enemy)
    {
        Vector3 position = groundSpawnPositions[Random.Range(0, groundSpawnPositions.Length)].position;
        return position += Random.insideUnitSphere;
    }
}
