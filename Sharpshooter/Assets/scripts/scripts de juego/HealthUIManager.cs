using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public HealthComponent healthComponent;
    public Slider healthSlider;

    private void Start()
    {
        if (healthComponent == null)
            healthComponent = FindObjectOfType<HealthComponent>();

        if (healthSlider == null)
            Debug.LogError("Health Slider no asignado en HealthUI");

        
        UpdateHealthBar();

        
        healthComponent.OnTookDamage.AddListener(UpdateHealthBar);
        healthComponent.OnHealed.AddListener(UpdateHealthBar);
    }

    private void UpdateHealthBar(float _ = 0f)
    {
        if (healthSlider != null && healthComponent != null)
        {
            healthSlider.value = healthComponent.HealthPercent;
        }
    }
}
