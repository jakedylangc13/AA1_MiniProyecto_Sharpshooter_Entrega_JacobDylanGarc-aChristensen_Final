using UnityEngine;

public class KillZone : MonoBehaviour
{
    public float killDamage = 9999f; // Un daño alto para asegurarte de que mata

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthComponent health = other.GetComponent<HealthComponent>();
            if (health != null && !health.IsDead)
            {
                health.Damage(killDamage);
            }
        }
    }
}
