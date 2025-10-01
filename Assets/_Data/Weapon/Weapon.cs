using System;

[Serializable]
public class Weapon
{
    public WeaponType weaponType;
    public int ammo;
    public int maxAmmo;

    public bool CanShoot()
    {
        return HasEnoughBullet();
    }

    private bool HasEnoughBullet()
    {
        if (ammo <= 0) return false;
        ammo--;
        return true;
    }
}