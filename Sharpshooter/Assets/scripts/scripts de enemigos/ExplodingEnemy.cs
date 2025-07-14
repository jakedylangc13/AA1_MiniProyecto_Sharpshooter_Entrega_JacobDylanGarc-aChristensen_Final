using UnityEngine;

public class ExplodingEnemy : MonoBehaviour
{
    public float speed = 3f;
    public float detectionRadius = 5f;
    public float explosionRadius = 3f;
    public float explosionDelay = 1f;
    public float explosionDamage = 50f;
    public GameObject explosionVFX;

    private Transform player;
    private bool isExploding = false;
    private bool playerDetected = false;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!playerDetected && distanceToPlayer <= detectionRadius)
        {
            playerDetected = true;
        }

        if (playerDetected && !isExploding)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }

        if (distanceToPlayer <= explosionRadius && !isExploding)
        {
            isExploding = true;
            Invoke(nameof(Explode), explosionDelay);
        }
    }

    private void Explode()
    {
        if (explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            HealthComponent health = hit.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.Damage(explosionDamage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
