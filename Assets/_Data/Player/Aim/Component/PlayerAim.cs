using System;
using UnityEngine;

public class PlayerAim : AimComponent
{
    [Header("Aim Info")] 
    [SerializeField] protected Camera mainCamera;

    [SerializeField] protected bool isAimingPrecisely;
    public bool IsAimingPrecisely => isAimingPrecisely;
    [SerializeField] protected bool isLockTarget;
    [SerializeField] protected Transform aimPoint;
    public Transform AimPoint => aimPoint;

    [SerializeField] protected LayerMask aimLayerMask;
    protected RaycastHit lastKnownMouseHit;

    protected void Update()
    {
        //Note: Test purpose
        if(Input.GetKeyDown(KeyCode.P)) isAimingPrecisely = !isAimingPrecisely;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMainCamera();
        LoadAimLayer();
        LoadAimPoint();
    }

    protected void LoadMainCamera()
    {
        if (mainCamera != null) return;
        mainCamera = Camera.main;
        Debug.Log(transform.name + " LoadMainCamera", gameObject);
    }

    protected void LoadAimLayer()
    {
        if (aimLayerMask != 0) return;
        aimLayerMask = LayerMask.GetMask("Ground", "Obstacles", "Enemy");
        Debug.Log(transform.name + " LoadAimLayer", gameObject);
    }

    protected void LoadAimPoint()
    {
        if (aimPoint != null) return;
        aimPoint = GameObject.Find("AimPoint").transform;
        Debug.Log(transform.name + " LoadAimPoint", gameObject);
    }

    public Transform Target()
    {
        Transform target = null;
        if (GetMouseHitInfo().transform.TryGetComponent(out Target type))
            target = GetMouseHitInfo().transform;
        return target;
    }
    
    public void UpdateAimPos()
    {
        var target = Target();
        if (target is not null && isLockTarget)
        {
            aimPoint.position = target.position;
            return;
        }
        
        aimPoint.position = GetMouseHitInfo().point;
        if (!isAimingPrecisely)
            aimPoint.position = new Vector3(aimPoint.position.x, aimCtrl.transform.parent.position.y + 1f, aimPoint.position.z);
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = mainCamera.ScreenPointToRay(PlayerCtrl.Instance.PlayerInput.MouseInput);

        if (!Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
            return lastKnownMouseHit;
        lastKnownMouseHit = hitInfo;
        return hitInfo;
    }
    
    public void SwitchLockTarget() => isLockTarget = !isLockTarget;
}