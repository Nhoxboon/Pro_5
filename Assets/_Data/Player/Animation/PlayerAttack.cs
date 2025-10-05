using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : NhoxBehaviour
{
    protected string bulletName = "Bullet";

    [SerializeField] protected Weapon currentWeapon;
    public Weapon CurrentWeapon => currentWeapon;

    protected bool weaponReady;
    public bool WeaponReady => weaponReady;
    protected bool isShooting;

    [SerializeField] protected float bulletSpeed = 20f;

    [Header("Inventory")] [SerializeField] protected int maxSlots = 2;
    [SerializeField] protected List<Weapon> weaponSlots;

    protected override void Start()
    {
        base.Start();
        Invoke(nameof(EquipStartingWeapon), 0.1f);
    }

    protected void Update()
    {
        if (isShooting)
            ShootBullet();
    }

    public Transform GunPoint() => PlayerCtrl.Instance.PlayerWeaponVisual.CurrentWeaponModel().gunPoint;

    public void SetIsShooting(bool value) => isShooting = value;

    public void ShootBullet()
    {
        var playerAnim = PlayerCtrl.Instance.PlayerAnim;
        if (!weaponReady) return;
        if (!currentWeapon.CanShoot()) return;

        if (currentWeapon.shootType == ShootType.Single)
            isShooting = false;

        playerAnim.ShootAnim();
        if (currentWeapon.BurstActivated())
        {
            StartCoroutine(BurstFire());
            return;
        }

        SpawnBullet();
    }

    protected void SpawnBullet()
    {
        currentWeapon.bulletsInMagazine--;

        var prefab =
            BulletSpawner.Instance.Spawn(bulletName, GunPoint().position, Quaternion.LookRotation(GunPoint().forward));
        prefab.gameObject.SetActive(true);

        Vector3 bulletDir = currentWeapon.ApplySpread(BulletDirection());

        if (!prefab.TryGetComponent(out Bullet bullet)) return;
        bullet.BulletSetup(currentWeapon.gunDistance);
        bullet.Fire(bulletDir, bulletSpeed);
    }

    protected IEnumerator BurstFire()
    {
        weaponReady = false;
        for (int i = 1; i <= currentWeapon.bulletsPerShot; i++)
        {
            SpawnBullet();
            yield return new WaitForSeconds(currentWeapon.burstFireDelay);
            if (i >= currentWeapon.bulletsPerShot)
                weaponReady = true;
        }
    }

    public Vector3 BulletDirection()
    {
        var playerAim = PlayerCtrl.Instance.PlayerAim.Aim;
        Vector3 dir = (playerAim.AimPoint.position - GunPoint().position).normalized;

        if (!playerAim.IsAimingPrecisely && playerAim.Target() is null)
            dir.y = 0;

        return dir;
    }

    protected void EquipStartingWeapon() => EquipWeapon(0);

    public void EquipWeapon(int i)
    {
        if (i >= weaponSlots.Count) return;
        weaponReady = false;

        currentWeapon = weaponSlots[i];
        PlayerCtrl.Instance.PlayerAnim.WeaponEquipAnim();
        CameraManager.Instance.ChangeCameraDistance(currentWeapon.cameraDistance);
    }

    public void PickupWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots) return;
        weaponSlots.Add(newWeapon);
        PlayerCtrl.Instance.PlayerWeaponVisual.SwitchOnBackupWeapon();
    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;
    public bool HasWeaponTypeInInventory(WeaponType type)
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon.weaponType == type)
                return true;
        }
        return false;
    }

    public void DropWeapon()
    {
        if (HasOnlyOneWeapon()) return;
        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    public void SetWeaponReady(bool ready) => weaponReady = ready;
}