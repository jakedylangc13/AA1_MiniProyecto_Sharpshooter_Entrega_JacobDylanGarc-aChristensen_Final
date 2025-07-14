using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;
    public float raycastDistance = 1f;

    private Rigidbody rb;
    private LayerMask hitMask;
    private Action<Bullet> onDeactivate;
    private float spawnTime;

    [SerializeField] private ParticleSystem particulas;

    [SerializeField] private MeshRenderer _renderer;

    public void Initialize(LayerMask mask, Action<Bullet> deactivateCallback)
    {
        rb = GetComponent<Rigidbody>();
        _renderer.enabled = true;
        hitMask = mask;
        onDeactivate = deactivateCallback;
    }

    public void Launch(Vector3 velocity)
    {
        rb.linearVelocity = velocity;
        spawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        
        Vector3 direction = rb.linearVelocity.normalized;
        float distance = rb.linearVelocity.magnitude * Time.fixedDeltaTime + raycastDistance;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, hitMask))
        {
            Debug.DrawRay(transform.position, direction * distance, Color.red, 1f);
            HandleImpact(hit.collider);
        }
        else if (Time.time - spawnTime > lifetime)
        {
            onDeactivate?.Invoke(this);
        }
    }

    public float damage = 10f;

    private void HandleImpact(Collider hitCollider)
    {
        rb.linearVelocity = Vector2.zero;
        _renderer.enabled = false;
        particulas.gameObject.SetActive(true);
        particulas.Play();
        StartCoroutine(DeactivationTimer());


        if (hitCollider.GetComponent<HealthComponent>())
        {
            hitCollider.GetComponent<HealthComponent>().Damage(damage);
        }
    }

    private IEnumerator DeactivationTimer()
    {
        yield return new WaitForSeconds(1f);
  
        onDeactivate?.Invoke(this);

    }
    private void OnTriggerEnter(Collider other)
    {
        // Aplica daño si es enemigo
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(1); // Cada bala hace 1 de daño
        }

       
    }
}