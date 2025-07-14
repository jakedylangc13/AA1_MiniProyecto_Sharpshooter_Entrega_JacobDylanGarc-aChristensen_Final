using UnityEngine;

public class ShooterRobot : MonoBehaviour
{
    public Transform player;
    public Transform firePoint;

    [Header("Movement")]
    public float orbitRadius = 2f;
    public float hoverAmplitude = 0.2f;
    public float hoverSpeed = 2f;

    [Header("Recoil")]
    public float recoilStrength = 0.3f;
    public float recoilReturnSpeed = 3f;

    private Vector3 baseOffset;
    private Vector3 recoilOffset;
    private float hoverOffset;
    private float hoverTimer;

    private void Start()
    {
        baseOffset = Vector3.forward * orbitRadius;
    }

    [SerializeField] private float moveSpeed;

    private void FixedUpdate()
    {
        // Position orbiting around player's forward
        Vector3 targetOffset = player.position + player.forward * orbitRadius;
        transform.position = Vector3.Lerp(transform.position, targetOffset + Vector3.up * hoverOffset + recoilOffset, Time.deltaTime * moveSpeed);

        // Hover animation
        hoverTimer += Time.deltaTime * hoverSpeed;
        hoverOffset = Mathf.Sin(hoverTimer) * hoverAmplitude;

        // Reset recoil
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
    }

    public void AimInDirection(Vector3 direction)
    {
        direction.y = 0; // Optional: lock vertical rotation
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    public void ApplyRecoil()
    {
        recoilOffset -= transform.forward * recoilStrength;
    }

    public Vector3 GetFirePoint() => firePoint.position;
    public Quaternion GetFireRotation() => firePoint.rotation;
}
