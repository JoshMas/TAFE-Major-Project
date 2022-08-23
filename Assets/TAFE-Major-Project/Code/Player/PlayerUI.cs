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

    public static float timer = 0;
    [SerializeField] private Text timerText;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
        width = healthBarImage.rectTransform.rect.width;
        height = healthBarImage.rectTransform.rect.height;
        timer = LevelManager.savedTimer;
        timerText.text = timer + "";
    }

    private void OnEnable()
    {
        health.HealthHasUpdated += UpdateHealthBar;
    }

    private void OnDisable()
    {
        health.HealthHasUpdated -= UpdateHealthBar;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        timerText.text = timer.ToString("F2");
    }

    private void UpdateHealthBar(float _current, float _maximum)
    {
        float fraction = _current / _maximum;
        healthBarImage.rectTransform.sizeDelta = new Vector2(fraction * width, height);
    }
}
