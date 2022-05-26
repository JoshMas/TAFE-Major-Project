using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Health health;
    [SerializeField] private Image healthBarImage;
    private float width;
    private float height;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
        width = healthBarImage.rectTransform.rect.width;
        height = healthBarImage.rectTransform.rect.height;
    }

    private void OnEnable()
    {
        health.healthUpdated += UpdateHealthBar;
    }

    private void OnDisable()
    {
        health.healthUpdated -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float _current, float _maximum)
    {
        float fraction = _current / _maximum;
        healthBarImage.rectTransform.sizeDelta = new Vector2(fraction * width, height);
    }
}
