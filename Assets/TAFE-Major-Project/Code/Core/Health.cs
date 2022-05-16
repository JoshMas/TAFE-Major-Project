using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maximumHealth;
    private float currentHealth;

    /// <summary>
    /// called when health changes
    /// </summary>
    /// <param name="_amount"></param>
    public delegate void HealthUpdated(float _amount);
    public event HealthUpdated healthUpdated;

    /// <summary>
    /// called when health runs out
    /// </summary>
    public delegate void HealthEmpty();
    public event HealthEmpty healthEmpty;

    public void UpdateHealth(float _amount)
    {
        float newHealth = Mathf.Clamp(0, maximumHealth, currentHealth + _amount);
        healthUpdated?.Invoke(newHealth - currentHealth);
        currentHealth = newHealth;
        if (currentHealth <= 0)
            healthEmpty?.Invoke();
    }
}
