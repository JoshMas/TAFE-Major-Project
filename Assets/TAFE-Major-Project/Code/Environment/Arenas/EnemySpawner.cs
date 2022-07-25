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

    [SerializeField] private WaveTriggerPair[] waves;

    private Transform[] spawnPositions;

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
        List<Transform> children = new List<Transform>(GetComponentsInChildren<Transform>());
        children.Remove(transform);
        spawnPositions = children.ToArray();

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

    private void SpawnNextWave()
    {
        if(waveMarker >= waves.Length)
        {
            return;
        }

        foreach(GameObject enemy in enemiesToSpawn[waveMarker])
        {
            currentEnemies.Add(Instantiate(enemy, GetSpawnPosition(), Quaternion.identity));
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 position = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
        return position += Random.insideUnitSphere;
    }
}
