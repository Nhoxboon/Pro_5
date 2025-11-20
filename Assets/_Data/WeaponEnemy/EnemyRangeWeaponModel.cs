using UnityEngine;

public class EnemyRangeWeaponModel : NhoxBehaviour
{
    public EnemyRangeWeaponType weaponType;
    public EnemyRangeHoldType holdType;
    public Transform gunPoint;

    public Transform leftHandTarget;
    public Transform leftElbowTarget;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGunPoint();
        LoadLeftHandTarget();
        LoadLeftElbowTarget();
    }

    protected void LoadGunPoint()
    {
        if (gunPoint != null) return;
        gunPoint = transform.Find("gunPoint");
        Debug.Log(transform.name + " LoadGunPoint", gameObject);
    }

    protected void LoadLeftHandTarget()
    {
        if (leftHandTarget != null) return;
        leftHandTarget = transform.Find("leftHand_Target");
        Debug.Log(transform.name + " LoadLeftHandTarget", gameObject);
    }

    protected void LoadLeftElbowTarget()
    {
        if (leftElbowTarget != null) return;
        leftElbowTarget = transform.Find("leftElbow_Target");
        Debug.Log(transform.name + " LoadLeftElbowTarget", gameObject);
    }
}