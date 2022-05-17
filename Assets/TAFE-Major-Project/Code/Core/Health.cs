using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maximumHealth;
    private float currentHealth;

    /// <summary>
    /// Called when health changes
    /// </summary>
    /// <param name="_current">current health</param>
    /// <param name="_maximum">maximum health</param>
    public delegate void HealthUpdated(float _current, float _maximum);
    public event HealthUpdated healthUpdated;

    public delegate void HitTaken();
    public event HitTaken hitTaken;

    /// <summary>
    /// Called when health runs out
    /// </summary>
    public delegate void HealthEmpty();
    public event HealthEmpty healthEmpty;

    private void Awake()
    {
        currentHealth = maximumHealth;
    }

    /// <summary>
    /// Updates the health value
    /// </summary>
    /// <param name="_amount">The amount of health added</param>
    public void UpdateHealth(float _amount)
    {
        if (_amount < 0)
            hitTaken?.Invoke();
        currentHealth = Mathf.Clamp(currentHealth + _amount, 0, maximumHealth);
        Debug.Log(currentHealth);
        healthUpdated?.Invoke(currentHealth, maximumHealth);
        if (currentHealth <= 0)
            healthEmpty?.Invoke();
    }
}
