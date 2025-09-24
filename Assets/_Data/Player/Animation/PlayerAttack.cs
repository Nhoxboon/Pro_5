using System;
using UnityEngine;

public class PlayerAttack : NhoxBehaviour
{
    protected string bulletName = "Bullet";

    [SerializeField] protected float bulletSpeed = 10f;
    [SerializeField] protected Transform gunPoint;
    public Transform GunPoint => gunPoint;

    [SerializeField] protected Transform weaponHolder;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponHolder();
    }

    protected void LoadWeaponHolder()
    {
        if(weaponHolder != null) return;
        weaponHolder = GameObject.Find("WeaponHolder").transform;
        Debug.Log(transform.name + " LoadWeaponHolder", gameObject);
    }

    public void ShootBullet()
    {
        Transform prefab =
            BulletSpawner.Instance.Spawn(bulletName, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        prefab.gameObject.SetActive(true);
        
        if(prefab.TryGetComponent(out Bullet bullet))
            bullet.Fire(BulletDirection(), bulletSpeed);
    }

    public Vector3 BulletDirection()
    {
        var playerAim = PlayerCtrl.Instance.PlayerAim.Aim;
        Vector3 dir = (playerAim.AimPoint.position - gunPoint.position).normalized;

        if(!playerAim.IsAimingPrecisely && playerAim.Target() is null)
            dir.y = 0;
        
        //TODO: Fix and find a better place to put this
        // weaponHolder.LookAt(aim);
        // gunPoint.LookAt(aim);
        
        return dir;
    }
}
