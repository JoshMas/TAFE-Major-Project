using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public EnemyWeightPair[] enemyTable;
    private List<GameObject> enemies;
    [Min(0)] public int enemyMinNum = 1;
    [Min(1)] public int enemyMaxNum = 3;

    private void OnEnable()
    {
        enemies = new List<GameObject>();
        foreach (EnemyWeightPair pair in enemyTable)
        {
            pair.AddEnemies(ref enemies);
        }
    }

    public List<GameObject> GetEnemies()
    {
        List<GameObject> enemiesToSpawn = new List<GameObject>();
        int numToSpawn = Random.Range(enemyMinNum, enemyMaxNum + 1);
        for(int i = 0; i < numToSpawn; ++i)
        {
            enemiesToSpawn.Add(enemies[Random.Range(0, enemies.Count)]);
        }
        return enemiesToSpawn;
    }
}
