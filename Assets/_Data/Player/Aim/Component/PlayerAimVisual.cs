
using System;
using UnityEngine;

public class PlayerAimVisual : AimComponent
{
    [SerializeField] protected LineRenderer aimLaser;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLineRenderer();
    }
    
    protected void LoadLineRenderer()
    {
        if (aimLaser != null) return;
        aimLaser = GetComponent<LineRenderer>();
        Debug.Log(transform.name + " LoadLineRenderer", gameObject);
    }
    
    public void UpdateAimVisuals()
    {
        var playerAtk = PlayerCtrl.Instance.PlayerAttack;
        aimLaser.enabled = playerAtk.WeaponReady;
        
        if(!aimLaser.enabled) return;
        
        WeaponModel weaponModel = PlayerCtrl.Instance.PlayerWeaponVisual.CurrentWeaponModel();
        weaponModel.transform.LookAt(aimCtrl.Aim.AimPoint);
        weaponModel.gunPoint.LookAt(aimCtrl.Aim.AimPoint);
        
        Transform gunPoint = playerAtk.GunPoint();
        Vector3 laserDir = playerAtk.BulletDirection();
        
        float laserTipLength = 0.5f;
        float gunDistance = playerAtk.CurrentWeapon.GunDistance;
        
        Vector3 endPoint = gunPoint.position + laserDir * gunDistance;
        if(Physics.Raycast(gunPoint.position, laserDir, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            laserTipLength = 0;
        }
        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDir * laserTipLength);
    }
}
