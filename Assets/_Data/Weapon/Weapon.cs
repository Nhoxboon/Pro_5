using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Weapon
{
    public WeaponType weaponType;
    
    [Header("Shooting Specifics")]
    public ShootType shootType;
    public int bulletsPerShot = 1;
    public float defaultFireRate = 1;
    public float fireRate = 1; //Bullets per second
    private float lastShootTime;

    [Header("Burst Fire")] 
    public bool burstAvailable;
    public bool burstActive;
    public int bulletsPerBurst = 1;
    public float burstFireRate = 1;
    public float burstFireDelay = 0.1f;
    
    [Header("Magazine Details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;
    [Range(1, 2)]
    public float reloadSpeed = 1;
    [Range(1, 2)]
    public float equipmentSpeed = 1;
    [Range(2, 12)] 
    public float gunDistance = 4;

    [Range(3, 8)] 
    public float cameraDistance = 6;

    [Header("Spread")] 
    public float baseSpread = 1;
    public float maximumSpread = 3;
    private float currentSpread = 2;
    public float spreadIncreaseRate = 0.15f;
    
    private float lastSpreadUpdateTime;
    private float spreadCooldown = 0.5f;
    
    public bool CanShoot() => HasEnoughBullet() && ReadyToFire();

    private bool ReadyToFire()
    {
        if (!(Time.time > lastShootTime + 1 / fireRate)) return false;
        lastShootTime = Time.time;
        return true;
    }

    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
            return false;
        return totalReserveAmmo > 0;
    }

    public void RefillBullets()
    {
        int bulletsToReload = magazineCapacity;

        if (bulletsToReload > totalReserveAmmo) bulletsToReload = totalReserveAmmo;

        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazine = bulletsToReload;

        if (totalReserveAmmo < 0) totalReserveAmmo = 0;
    }
    
    private bool HasEnoughBullet() => bulletsInMagazine > 0;
    
    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();
        float randomizeValue = Random.Range(-currentSpread, currentSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizeValue, randomizeValue, randomizeValue);
        return spreadRotation * originalDirection;
    }

    private void UpdateSpread()
    {
        if(Time.time > lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();
        
        lastSpreadUpdateTime = Time.time;
    }

    private void IncreaseSpread() =>
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);

    public bool BurstActivated()
    {
        if (weaponType != WeaponType.Shotgun) return burstActive;
        burstFireDelay = 0;
        return true;
    }
    
    public void ToggleBurst()
    {
        if(!burstAvailable) return;
        burstActive = !burstActive;
        
        if (burstActive)
        {
            bulletsPerShot = bulletsPerBurst;
            fireRate = burstFireRate;
        }
        else
        {
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }
}