using System;
using UnityEngine;

[Serializable]
public class Weapon
{
    public WeaponType weaponType;
    
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1, 2)]
    public float reloadSpeed = 1;
    [Range(1, 2)]
    public float equipmentSpeed = 1;

    public bool CanShoot() => HasEnoughBullet();
    
    private bool HasEnoughBullet()
    {
        if (bulletsInMagazine <= 0) return false;
        bulletsInMagazine--;
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
}