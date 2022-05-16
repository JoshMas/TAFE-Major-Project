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
    /// <param name="_amount">The amount of health added</param>
    public delegate void HealthUpdated(float _amount);
    public event HealthUpdated healthUpdated;

    /// <summary>
    /// Called when health runs out
    /// </summary>
    public delegate void HealthEmpty();
    public event HealthEmpty healthEmpty;

    /// <summary>
    /// Updates the health value
    /// </summary>
    /// <param name="_amount">The amount of health added</param>
    public void UpdateHealth(float _amount)
    {
        float newHealth = Mathf.Clamp(0, maximumHealth, currentHealth + _amount);
        healthUpdated?.Invoke(newHealth - currentHealth);
        currentHealth = newHealth;
        if (currentHealth <= 0)
            healthEmpty?.Invoke();
    }
}
