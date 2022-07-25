using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private GameObject barrier;
    private Collider trigger;
    private bool active;

    private void Awake()
    {
        active = false;
        trigger = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        EnemySpawner.OnArenaClear += ArenaCleared;
        Tower.OnTowerDestroyed += ArenaCleared;
    }

    private void OnDisable()
    {
        EnemySpawner.OnArenaClear -= ArenaCleared;
        Tower.OnTowerDestroyed -= ArenaCleared;
    }

    private void Start()
    {
        barrier.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        barrier.SetActive(true);
        trigger.enabled = false;
        active = true;
    }

    private void ArenaCleared()
    {
        if (!active)
            return;
        barrier.SetActive(false);
        gameObject.SetActive(false);
    }
}
