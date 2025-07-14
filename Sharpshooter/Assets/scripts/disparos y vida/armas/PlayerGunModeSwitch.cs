using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunModeSwitch : Gun
{
    public Camera playerCamera;
    [SerializeField] private ShooterRobot shooterRobot;
    [SerializeField] public PlayerInputHandler inputHandler;

    [SerializeField] private Gun primaryGun;
    [SerializeField] private Gun secondaryGun;

    private int secondaryAmmoReserve = 150;
    

    [Header("Primary Fire (infinite ammo)")]
    public float primaryCooldown = 0.3f;
    public int primaryClipSize = 10;
    private int primaryCurrentClip;
    private bool primaryReloading;

    [Header("Secondary Fire (limited ammo)")]
    public float secondaryCooldown = 0.1f;
    public int secondaryClipSize = 30;
    public int secondaryMaxReserve = 120;
    private int secondaryCurrentClip;
    private int secondaryCurrentReserve;
    private bool secondaryReloading;

    private enum FireMode { Primary, Secondary }
    private FireMode currentMode = FireMode.Primary;

    

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        primaryCurrentClip = primaryClipSize;

        secondaryCurrentClip = secondaryClipSize;
        secondaryCurrentReserve = secondaryMaxReserve;

        Time.timeScale = 1f;
        Physics.simulationMode = SimulationMode.FixedUpdate;
        Cursor.lockState = CursorLockMode.Confined;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (inputHandler == null || shooterRobot == null || playerCamera == null)
            return;

        
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            currentMode = currentMode == FireMode.Primary ? FireMode.Secondary : FireMode.Primary;
            Debug.Log("Modo de disparo: " + currentMode);
        }

        
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 aimTarget = GetAimPoint(cameraForward);
        shooterRobot.AimInDirection(aimTarget - shooterRobot.transform.position);

        
        bool fireInput = Mouse.current.leftButton.isPressed || Gamepad.current?.rightTrigger.ReadValue() > 0.1f;
        if (fireInput)
        {
            TryShoot(aimTarget);
        }

        
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            TryReload();
        }
    }

    private Vector3 GetAimPoint(Vector3 aimDirection)
    {
        Ray ray = new Ray(playerCamera.transform.position, aimDirection);
        return Physics.Raycast(ray, out RaycastHit hit, 100f, ~0, QueryTriggerInteraction.Ignore)
            ? hit.point
            : ray.GetPoint(100f);
    }

    private void TryShoot(Vector3 targetPoint)
    {
        if (Time.time - lastFireTime < GetCurrentCooldown()) return;

        if (currentMode == FireMode.Primary)
        {
            if (primaryCurrentClip <= 0 || primaryReloading) return;
            primaryCurrentClip--;
        }
        else 
        {
            if (secondaryCurrentClip <= 0 || secondaryReloading) return;
            secondaryCurrentClip--;
        }

        Bullet bullet = GetPooledBullet();
        if (bullet == null) return;

        lastFireTime = Time.time;

        Vector3 firePos = shooterRobot.GetFirePoint();
        Vector3 fireDir = (targetPoint - firePos).normalized;

        if (Physics.Raycast(firePos, fireDir, out RaycastHit confirmHit, 100f, hitMask))
        {
            fireDir = (confirmHit.point - firePos).normalized;
        }

        bullet.transform.position = firePos;
        bullet.transform.rotation = Quaternion.LookRotation(fireDir);
        bullet.gameObject.SetActive(true);
        bullet.Launch(fireDir * bulletSpeed);

        shooterRobot.ApplyRecoil();
    }

    private void TryReload()
    {
        if (currentMode == FireMode.Primary)
        {
            if (primaryReloading || primaryCurrentClip == primaryClipSize) return;
            StartCoroutine(ReloadPrimary());
        }
        else
        {
            if (secondaryReloading || secondaryCurrentClip == secondaryClipSize || secondaryCurrentReserve <= 0) return;
            StartCoroutine(ReloadSecondary());
        }
    }

    private System.Collections.IEnumerator ReloadPrimary()
    {
        primaryReloading = true;
        yield return new WaitForSeconds(reloadTime);
        primaryCurrentClip = primaryClipSize;
        primaryReloading = false;
    }

    private System.Collections.IEnumerator ReloadSecondary()
    {
        secondaryReloading = true;
        yield return new WaitForSeconds(reloadTime);

        int bulletsNeeded = secondaryClipSize - secondaryCurrentClip;
        int bulletsToLoad = Mathf.Min(bulletsNeeded, secondaryCurrentReserve);

        secondaryCurrentClip += bulletsToLoad;
        secondaryCurrentReserve -= bulletsToLoad;

        secondaryReloading = false;
    }

    private float GetCurrentCooldown()
    {
        return currentMode == FireMode.Primary ? primaryCooldown : secondaryCooldown;
    }

    
    public new void AddAmmo(int amount)
    {
        secondaryCurrentReserve = Mathf.Min(secondaryCurrentReserve + amount, secondaryMaxReserve);
    }

    public int PrimaryCurrentAmmo => primaryCurrentClip;
    public int SecondaryCurrentAmmo => secondaryCurrentClip;
    public int SecondaryTotalAmmo => secondaryCurrentReserve;

    public bool IsReloading =>
        currentMode == FireMode.Primary ? primaryReloading : secondaryReloading;


}
