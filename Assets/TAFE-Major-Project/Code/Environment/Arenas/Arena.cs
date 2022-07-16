using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private GameObject barrier;
    private Collider trigger;

    private void Awake()
    {
        trigger = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        EnemySpawner.OnArenaClear += ArenaCleared;
    }

    private void OnDisable()
    {
        EnemySpawner.OnArenaClear -= ArenaCleared;
    }

    private void Start()
    {
        barrier.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        barrier.SetActive(true);
        trigger.enabled = false;
    }

    private void ArenaCleared()
    {
        barrier.SetActive(false);
        gameObject.SetActive(false);
    }
}
