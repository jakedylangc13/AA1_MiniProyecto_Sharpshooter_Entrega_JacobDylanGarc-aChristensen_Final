using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float healAmount = 25f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has a PlayerController
        var playerController = other.GetComponent<CharacterController>();
        if (playerController == null)
            return;

        // Look for HealthComponent on the player
        var health = playerController.GetComponent<HealthComponent>();
        if (health == null)
            return;

        // Heal and disable the pickup
        health.Heal(healAmount);
        gameObject.SetActive(false);
    }
}