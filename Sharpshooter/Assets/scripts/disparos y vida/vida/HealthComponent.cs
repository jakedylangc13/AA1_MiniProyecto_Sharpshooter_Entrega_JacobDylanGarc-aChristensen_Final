using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthComponent : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Events")]
    public UnityEvent<float> OnTookDamage;   
    public UnityEvent<float> OnHealed;       
    public UnityEvent OnDeath;

    private bool isDead = false;
    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float amount)
    {
        if (isDead || amount <= 0f)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        OnTookDamage?.Invoke(amount);

        if (currentHealth <= 0f && !isDead)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (isDead || amount <= 0f)
            return;

        float previousHealth = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        float actualHealed = currentHealth - previousHealth;

        if (actualHealed > 0f)
        {
            OnHealed?.Invoke(actualHealed);
        }
    }

    public void RestoreFull(bool revive = false)
    {
        currentHealth = maxHealth;
        if (revive)
            isDead = false;
    }

    public bool IsDead => isDead;
    public float HealthPercent => currentHealth / maxHealth;
}