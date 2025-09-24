
using UnityEngine;

public class AimComponent : NhoxBehaviour
{
    [SerializeField] protected PlayerAimCtrl aimCtrl;
    public PlayerAimCtrl AimCtrl => aimCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAimCtrl();
    }
    
    protected void LoadAimCtrl()
    {
        if (aimCtrl != null) return;
        aimCtrl = GetComponentInParent<PlayerAimCtrl>();
        Debug.Log(transform.name + " LoadAimCtrl", gameObject);
    }
}
