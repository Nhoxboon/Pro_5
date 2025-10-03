using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : NhoxBehaviour
{
    protected string bulletName = "Bullet";

    [SerializeField] protected Weapon currentWeapon;
    public Weapon CurrentWeapon => currentWeapon;

    [SerializeField] protected float bulletSpeed = 20f;
    [SerializeField] protected Transform gunPoint;
    public Transform GunPoint => gunPoint;
    [SerializeField] protected Transform weaponHolder;

    [Header("Inventory")] protected int maxSlots = 2;
    [SerializeField] protected List<Weapon> weaponSlots;

    protected override void Awake()
    {
        base.Awake();
        Invoke(nameof(EquipStartingWeapon), 0.1f);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponHolder();
    }

    protected void LoadWeaponHolder()
    {
        if (weaponHolder != null) return;
        weaponHolder = GameObject.Find("WeaponHolder").transform;
        Debug.Log(transform.name + " LoadWeaponHolder", gameObject);
    }

    public void ShootBullet()
    {
        var playerAnim = PlayerCtrl.Instance.PlayerAnim;
        if (playerAnim.IsEquipingWeapon) return;
        if (!currentWeapon.CanShoot()) return;

        var prefab =
            BulletSpawner.Instance.Spawn(bulletName, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        prefab.gameObject.SetActive(true);

        if (prefab.TryGetComponent(out Bullet bullet))
            bullet.Fire(BulletDirection(), bulletSpeed);
        playerAnim.ShootAnim();
    }

    public Vector3 BulletDirection()
    {
        var playerAim = PlayerCtrl.Instance.PlayerAim.Aim;
        Vector3 dir = (playerAim.AimPoint.position - gunPoint.position).normalized;

        if (!playerAim.IsAimingPrecisely && playerAim.Target() is null)
            dir.y = 0;

        //TODO: Fix and find a better place to put this
        // weaponHolder.LookAt(aim);
        // gunPoint.LookAt(aim);

        return dir;
    }

    protected void EquipStartingWeapon() => EquipWeapon(0);

    public void EquipWeapon(int i)
    {
        if (i > weaponSlots.Count - 1) return;
        
        currentWeapon = weaponSlots[i];
        PlayerCtrl.Instance.PlayerAnim.WeaponEquipAnim();
    }

    public void PickupWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots) return;
        weaponSlots.Add(newWeapon);
        PlayerCtrl.Instance.PlayerWeaponVisual.SwitchOnBackupWeapon();
    }

    public Weapon BackupWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if(weapon != currentWeapon)
                return weapon;
        }
        return null;
    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    public void DropWeapon()
    {
        if (HasOnlyOneWeapon()) return;
        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }
}