using UnityEngine;

public class EnemyRangeWeaponModel : NhoxBehaviour
{
    public EnemyRangeWeaponType weaponType;
    public EnemyRangeHoldType holdType;
    public Transform gunPoint;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGunPoint();
    }

    protected void LoadGunPoint()
    {
        if (gunPoint != null) return;
        gunPoint = transform.Find("gunPoint");
        Debug.Log(transform.name + " LoadGunPoint", gameObject);
    }
}