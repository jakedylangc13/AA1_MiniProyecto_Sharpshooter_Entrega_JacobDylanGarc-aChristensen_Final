using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : Gun
{
    public Camera playerCamera;
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private ShooterRobot shooterRobot;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        Time.timeScale = 1f;
        Physics.simulationMode = SimulationMode.FixedUpdate;
        Cursor.lockState = CursorLockMode.Confined;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (inputHandler == null || shooterRobot == null || playerCamera == null)
            return;

        // 1. Get where the camera is pointing
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 aimTarget = GetAimPoint(cameraForward);

        // 2. Aim the shooter robot toward that point
        shooterRobot.AimInDirection(aimTarget - shooterRobot.transform.position);

        // 3. Handle input: shoot or reload
        if (Mouse.current.leftButton.isPressed || Gamepad.current?.rightTrigger.ReadValue() > 0.1f)
        {
            FireFromShooterRobot(aimTarget);
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Reload();
        }
    }

    /// <summary>
    /// Casts a ray from the camera forward and returns the collision point or a fallback distant point.
    /// </summary>
    private Vector3 GetAimPoint(Vector3 aimDirection)
    {
        Ray ray = new Ray(playerCamera.transform.position, aimDirection);
        return Physics.Raycast(ray, out RaycastHit hit, 100f, ~0, QueryTriggerInteraction.Ignore)
            ? hit.point
            : ray.GetPoint(100f); // Fallback if nothing hit
    }

    /// <summary>
    /// Fires a bullet from the shooter robot toward the given target point.
    /// </summary>
    private void FireFromShooterRobot(Vector3 targetPoint)
    {
        if (Time.time - lastFireTime < fireCooldown || currentAmmo <= 0 || isReloading)
            return;

        Bullet bullet = GetPooledBullet();
        if (bullet == null)
            return;

        lastFireTime = Time.time;
        currentAmmo--;

        Vector3 firePos = shooterRobot.GetFirePoint();
        Vector3 fireDir = (targetPoint - firePos).normalized;

        
        Ray ray = new Ray(firePos, fireDir);
        if (Physics.Raycast(ray, out RaycastHit confirmHit, 100f, hitMask, QueryTriggerInteraction.Ignore))
        {
            fireDir = (confirmHit.point - firePos).normalized;
        }

        bullet.transform.position = firePos;
        bullet.transform.rotation = Quaternion.LookRotation(fireDir);
        bullet.gameObject.SetActive(true);
        bullet.Launch(fireDir * bulletSpeed);

        shooterRobot.ApplyRecoil();
    }
    

}
