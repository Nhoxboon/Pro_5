using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Player Data/Weapon System/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    public string weaponName;
    public WeaponType weaponType;
    [Header("Bullet Info")]
    public int bulletDamage = 2;

    [Header("Magazine Details")] public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Header("Regular Fire")] public ShootType shootType;
    public int bulletsPerShot = 1;
    public float fireRate = 1; //Bullets per second

    [Header("Burst Fire")] public bool burstAvailable;
    public bool burstActive;
    public int bulletsPerBurst = 1;
    public float burstFireRate = 1;
    public float burstFireDelay = 0.1f;

    [Header("Weapon Spread")] public float baseSpread = 1;
    public float maximumSpread = 3;
    public float spreadIncreaseRate = 0.15f;

    [Header("Weapon Generics")] [Range(1, 3)]
    public float reloadSpeed = 1;

    [Range(1, 3)] public float equipmentSpeed = 1;
    [Range(4, 25)] public float gunDistance = 4;
    [Range(4, 8)] public float cameraDistance = 6;

    [Header("UI Icons")]
    public Sprite weaponIcon;
}