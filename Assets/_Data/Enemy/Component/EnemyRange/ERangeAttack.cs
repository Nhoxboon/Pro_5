using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ERangeAttack : EnemyAttack
{
    protected EnemyRange enemyRange;

    [Header("Weapon Details")] 
    [SerializeField] protected EnemyRangeWeaponType weaponType;
    public EnemyRangeWeaponType WeaponType => weaponType;
    [SerializeField] protected float attackDelay = 1f;
    public float AttackDelay => attackDelay;

    public ERangeWeaponDataSO WeaponData { get; protected set; }
    public Transform GunPoint { get; protected set; }
    protected string bulletPrefab = "EnemyBullet";
    [SerializeField] protected List<ERangeWeaponDataSO> availableWeaponsData;
    
    protected override void Awake()
    {
        base.Awake();
        enemyRange = enemy as EnemyRange;
    }
    
    public void SetWeaponType(EnemyRangeWeaponType newType) => weaponType = newType;

    public void SelectRandomWeaponType()
    {
        if (weaponType != EnemyRangeWeaponType.Random) return;
        List<EnemyRangeWeaponType> validTypes = new List<EnemyRangeWeaponType>();

        foreach (EnemyRangeWeaponType value in Enum.GetValues(typeof(EnemyRangeWeaponType)))
        {
            if(value != EnemyRangeWeaponType.Random && value != EnemyRangeWeaponType.Rifle)
                validTypes.Add(value);
        }
        
        int randomIndex = Random.Range(0, validTypes.Count);
        weaponType = validTypes[randomIndex];
    }
    
    protected void GetGunPoint()
    {
        if (enemyRange.EnemyVisuals.CurrentWeaponModel.TryGetComponent(out EnemyRangeWeaponModel weapon) &&
            GunPoint != weapon.gunPoint)
            GunPoint = weapon.gunPoint;
    }

    public override void UpdateAttackData()
    {
        List<ERangeWeaponDataSO> filterData =
            availableWeaponsData.Where(wpData => wpData.weaponType == weaponType).ToList();

        if (filterData.Count == 0) return;
        int random = Random.Range(0, filterData.Count);
        WeaponData = filterData[random];

        GetGunPoint();
    }

    public void FireSingleBullet()
    {
        enemyRange.EnemyAnimator.AnimationTrigger("attack");
        Vector3 bulletDir = (enemyRange.EnemyAim.aim.position - GunPoint.position).normalized;
        Transform newBullet = ProjectileSpawner.Instance.Spawn(bulletPrefab, GunPoint.position,
            Quaternion.LookRotation(GunPoint.forward));
        newBullet.gameObject.SetActive(true);

        if (!newBullet.TryGetComponent(out Bullet bullet)) return;
        bullet.BulletSetup(WeaponData.bulletDamage);
        bullet.Fire(WeaponData.ApplyWeaponSpread(bulletDir), WeaponData.bulletSpeed);
    }
}