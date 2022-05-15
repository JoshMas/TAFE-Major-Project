using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public Transform[] spawnpoint;
    public GameObject[] enemys;
    public static bool spawnAllowed;

    private void Start()
    {
        spawnAllowed = true;
        InvokeRepeating("spawnaenemy", 0f, 20f);
    }
    //private IEnumerator Loop()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1);
    //        spawnaenemy();
    //    }
    //}
    void spawnaenemy ()

    {
        if (spawnAllowed)
        {
            int Randomspawnpoint = Random.Range(0, spawnpoint.Length);
            int Randomenemy = Random.Range(0, enemys.Length);
            Instantiate(enemys[Randomenemy], spawnpoint[Randomspawnpoint].position, Quaternion.identity);

        }
    }
}
