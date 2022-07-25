using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public delegate void TowerDestroyed();
    public static event TowerDestroyed OnTowerDestroyed;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.HealthIsEmpty += HealthEmpty;
    }

    private void OnDisable()
    {
        health.HealthIsEmpty -= HealthEmpty;
    }

    private void HealthEmpty()
    {
        OnTowerDestroyed?.Invoke();
        transform.root.gameObject.SetActive(false);
    }
}
