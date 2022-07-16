using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWeightPair
{
    public GameObject enemy;
    public int weight;

    public void AddEnemies(ref List<GameObject> _list)
    {
        for (int i = 0; i < weight; ++i)
        {
            _list.Add(enemy);
        }
    }
}
