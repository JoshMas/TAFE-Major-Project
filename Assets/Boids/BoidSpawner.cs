using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [SerializeField] private Boid boidPrefab;
    [SerializeField] private BoidSettings settings;
    [SerializeField] private Transform target;
    [SerializeField] private float spawnRadius;
    [SerializeField] private int spawnCount;

    private void Awake()
    {
        for(int i = 0; i < spawnCount; ++i)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Boid boid = Instantiate(boidPrefab, pos, Quaternion.LookRotation(Random.onUnitSphere));
            boid.Initialise(target, settings);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, .75f, 0, .5f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
}
