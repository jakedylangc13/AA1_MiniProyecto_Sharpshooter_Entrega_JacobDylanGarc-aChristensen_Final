using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float delayBeforeFall = 0.5f;
    public float destroyDelay = 2f;

    private Rigidbody rb;
    private bool triggered = false;

    private void Start()
    {
        // Agregamos Rigidbody si no existe
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true; // No se cae hasta que lo activamos
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        triggered = true;
        Invoke(nameof(ActivateFalling), delayBeforeFall);
    }

    private void ActivateFalling()
    {
        rb.isKinematic = false; // Empieza a caer
        Destroy(gameObject, destroyDelay); // Se destruye después
    }
}
