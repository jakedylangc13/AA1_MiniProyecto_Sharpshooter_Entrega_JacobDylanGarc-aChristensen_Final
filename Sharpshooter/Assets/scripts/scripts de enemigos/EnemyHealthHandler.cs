using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class EnemyHealthHandler : MonoBehaviour
{
    private void Start()
    {
        EnemyManager.Instance?.RegisterEnemy();

        GetComponent<HealthComponent>().OnDeath.AddListener(OnDeath);
    }

    private void OnDeath()
    {
        EnemyManager.Instance?.UnregisterEnemy();
        Destroy(gameObject); // O animación, etc.
    }
}
