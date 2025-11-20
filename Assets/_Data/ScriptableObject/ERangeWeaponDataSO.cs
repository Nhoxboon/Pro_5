using UnityEngine;

[CreateAssetMenu(fileName = "ERangeWeaponDataSO", menuName = "Enemy Data/Range Weapon Data")]
public class ERangeWeaponDataSO : ScriptableObject
{
    [Header("Weapon Details")] public EnemyRangeWeaponType weaponType;
    public float fireRate = 2f;
    public int minBulletsPerAttack = 1;
    public int maxBulletsPerAttack = 3;

    public float minWeaponCooldown = 1.5f;
    public float maxWeaponCooldown = 3f;

    [Header("Bullet Details")] public float bulletSpeed = 20f;
    public float weaponSpread = 0.1f;

    public int GetBulletsPerAttack() => Random.Range(minBulletsPerAttack, maxBulletsPerAttack);
    public float GetWeaponCooldown() => Random.Range(minWeaponCooldown, maxWeaponCooldown);

    public Vector3 ApplyWeaponSpread(Vector3 originalDirection)
    {
        float randomizeValue = Random.Range(-weaponSpread, weaponSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizeValue, randomizeValue, randomizeValue);
        return spreadRotation * originalDirection;
    }
}