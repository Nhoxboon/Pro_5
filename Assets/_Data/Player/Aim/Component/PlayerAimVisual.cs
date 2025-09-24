
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
        aimLaser = transform.parent.parent.GetComponentInChildren<LineRenderer>();
        Debug.Log(transform.name + " LoadLineRenderer", gameObject);
    }
    
    public void UpdateAimVisuals()
    {
        if (!aimLaser.enabled) aimLaser.enabled = true;
        
        var playerAtk = PlayerCtrl.Instance.PlayerAttack;
        Transform gunPoint = playerAtk.GunPoint;
        Vector3 laserDir = playerAtk.BulletDirection();
        
        float laserTipLength = 0.5f;
        float gunDistance = 4f;
        
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
