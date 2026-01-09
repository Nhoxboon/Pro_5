using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Weapon
{
    public event Action OnAmmoChanged;
    public void NotifyAmmoChanged() => OnAmmoChanged?.Invoke();

    public WeaponType weaponType;
    
    public int bulletDamage;

    #region Regular Mode Variables

    public ShootType shootType;
    public int BulletsPerShot { get; private set; }
    private float defaultFireRate;
    public float fireRate;
    private float lastShootTime;

    #endregion

    #region Burst Mode Variables

    private bool burstAvailable;
    public bool burstActive;
    private int bulletsPerBurst;
    private float burstFireRate;
    public float BurstFireDelay { get; private set; }

    #endregion

    #region Magazine Variables

    public int bulletsInMagazine;
    private int magazineCapacity;
    public int totalReserveAmmo;

    #endregion

    #region Weapon Generics Variables

    public float ReloadSpeed { get; private set; }
    public float EquipmentSpeed { get; private set; }
    public float GunDistance { get; private set; }
    public float CameraDistance { get; private set; }

    #endregion

    #region Weapon Spread Variables

    private float baseSpread = 1;
    private float maximumSpread = 3;
    private float currentSpread = 2;
    private float spreadIncreaseRate = 0.15f;
    private float spreadCooldown = 0.5f;
    private float lastSpreadUpdateTime;

    #endregion

    public WeaponDataSO WeaponData { get; private set; } //Serves as default weapon data

    public Weapon(WeaponDataSO weaponData)
    {
        weaponType = weaponData.weaponType;
        bulletDamage = weaponData.bulletDamage;
        shootType = weaponData.shootType;
        BulletsPerShot = weaponData.bulletsPerShot;

        bulletsInMagazine = weaponData.bulletsInMagazine;
        magazineCapacity = weaponData.magazineCapacity;
        totalReserveAmmo = weaponData.totalReserveAmmo;

        fireRate = weaponData.fireRate;
        defaultFireRate = fireRate;

        burstAvailable = weaponData.burstAvailable;
        burstActive = weaponData.burstActive;
        bulletsPerBurst = weaponData.bulletsPerBurst;
        burstFireRate = weaponData.burstFireRate;
        BurstFireDelay = weaponData.burstFireDelay;

        baseSpread = weaponData.baseSpread;
        maximumSpread = weaponData.maximumSpread;
        spreadIncreaseRate = weaponData.spreadIncreaseRate;

        ReloadSpeed = weaponData.reloadSpeed;
        EquipmentSpeed = weaponData.equipmentSpeed;
        GunDistance = weaponData.gunDistance;
        CameraDistance = weaponData.cameraDistance;

        WeaponData = weaponData;
    }

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
        Quaternion spreadRotation = Quaternion.Euler(randomizeValue, randomizeValue / 2, randomizeValue);
        return spreadRotation * originalDirection;
    }

    private void UpdateSpread()
    {
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
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
        BurstFireDelay = 0;
        return true;
    }

    public void ToggleBurst()
    {
        if (!burstAvailable) return;
        burstActive = !burstActive;

        if (burstActive)
        {
            BulletsPerShot = bulletsPerBurst;
            fireRate = burstFireRate;
        }
        else
        {
            BulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }
}