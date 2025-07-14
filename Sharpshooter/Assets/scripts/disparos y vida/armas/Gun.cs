using UnityEngine;
using System.Collections.Generic;
using System;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public Transform firePoint;
    public Bullet bulletPrefab;
    public int poolSize = 20;
    public float fireCooldown = 0.2f;
    public int maxAmmo = 10;

    public int CurrentAmmo => currentAmmo;

    public virtual void AddAmmo(int ammoAmount)
    {
        throw new NotImplementedException();
    }

    public float reloadTime = 2f;

    [Header("Bullet Settings")]
    public float bulletSpeed = 50f;
    public LayerMask hitMask;

    protected List<Bullet> bulletPool = new List<Bullet>();
    protected float lastFireTime;
    protected int currentAmmo;
    protected bool isReloading;

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
        InitializeBulletPool();
    }

    protected virtual void InitializeBulletPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab, transform);
            bullet.Initialize(hitMask, DeactivateBullet);
            bullet.gameObject.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    protected virtual Bullet GetPooledBullet()
    {
        foreach (var bullet in bulletPool)
        {
            if (!bullet.gameObject.activeInHierarchy)
                return bullet;
        }
        return null;
    }

    public virtual void TryShoot()
    {
        if (Time.time - lastFireTime < fireCooldown || currentAmmo <= 0 || isReloading)
            return;

        Bullet bullet = GetPooledBullet();
        if (bullet != null)
        {
            lastFireTime = Time.time;
            currentAmmo--;

            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.gameObject.SetActive(true);
            bullet.Launch(firePoint.forward * bulletSpeed);
        }
        else
        {
            Debug.Log("No bullet available in pool.");
        }
    }

    public virtual void Reload()
    {
        if (!isReloading)
            StartCoroutine(ReloadRoutine());
    }

    protected virtual System.Collections.IEnumerator ReloadRoutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    protected virtual void DeactivateBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}
