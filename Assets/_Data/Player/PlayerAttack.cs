using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : NhoxBehaviour
{
    protected string bulletName = "Bullet";

    public Weapon CurrentWeapon => PlayerCtrl.Instance.PlayerInventory.CurrentWeapon;
    
    protected bool weaponReady;
    public bool WeaponReady => weaponReady;
    protected bool isShooting;

    [SerializeField] protected float bulletSpeed = 20f;

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
        if (!CurrentWeapon.CanShoot()) return;

        if (CurrentWeapon.shootType == ShootType.Single)
            isShooting = false;

        playerAnim.ShootAnim();
        if (CurrentWeapon.BurstActivated())
        {
            StartCoroutine(BurstFire());
            return;
        }

        SpawnBullet();
    }

    protected void SpawnBullet()
    {
        CurrentWeapon.bulletsInMagazine--;

        var prefab =
            BulletSpawner.Instance.Spawn(bulletName, GunPoint().position, Quaternion.LookRotation(GunPoint().forward));
        prefab.gameObject.SetActive(true);

        Vector3 bulletDir = CurrentWeapon.ApplySpread(BulletDirection());

        if (!prefab.TryGetComponent(out Bullet bullet)) return;
        bullet.BulletSetup(CurrentWeapon.GunDistance);
        bullet.Fire(bulletDir, bulletSpeed);
    }

    protected IEnumerator BurstFire()
    {
        weaponReady = false;
        for (int i = 1; i <= CurrentWeapon.BulletsPerShot; i++)
        {
            SpawnBullet();
            yield return new WaitForSeconds(CurrentWeapon.BurstFireDelay);
            if (i >= CurrentWeapon.BulletsPerShot)
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

    public void SetWeaponReady(bool ready) => weaponReady = ready;
}