using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ERangeVisuals : EnemyVisuals
{
    [Header("Rig References")] 
    [SerializeField] protected Transform leftHandIK;
    [SerializeField] protected Transform leftElbowIK;
    [SerializeField] protected TwoBoneIKConstraint leftHandIKConstraint;
    [SerializeField] protected MultiAimConstraint weaponAimConstraint;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTwoBoneIKConstraint();
        LoadMultiAimConstraint();
    }

    protected void LoadTwoBoneIKConstraint()
    {
        if (leftHandIKConstraint != null) return;
        leftHandIKConstraint = transform.parent.GetComponentInChildren<TwoBoneIKConstraint>();
        Debug.Log(transform.name + " LoadTwoBoneIKConstraint", gameObject);
    }
    
    protected void LoadMultiAimConstraint()
    {
        if (weaponAimConstraint != null) return;
        weaponAimConstraint = transform.parent.GetComponentInChildren<MultiAimConstraint>();
        Debug.Log(transform.name + " LoadMultiAimConstraint", gameObject);
    }

    protected void SetupLeftHandIK(Transform leftHandTarget, Transform leftElbowTarget)
    {
        leftHandIK.localPosition = leftHandTarget.localPosition;
        leftHandIK.localRotation = leftHandTarget.localRotation;

        leftElbowIK.localPosition = leftElbowTarget.localPosition;
        leftElbowIK.localRotation = leftElbowTarget.localRotation;
    }

    public void EnableIK(bool enableLeftHand, bool enableAim)
    {
        leftHandIKConstraint.weight = enableLeftHand ? 1 : 0;
        weaponAimConstraint.weight = enableAim ? 1 : 0;
    }

    protected override GameObject FindRangeWeaponModel()
    {
        if (enemy is not EnemyRange enemyRange) return null;
        EnemyRangeWeaponModel[] weaponModels = transform.parent.GetComponentsInChildren<EnemyRangeWeaponModel>(true);

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType != enemyRange.weaponType) continue;
            enemyRange.EnemyAnimator.SwitchAnimationLayer((int)weaponModel.holdType);
            SetupLeftHandIK(weaponModel.leftHandTarget, weaponModel.leftElbowTarget);
            return weaponModel.gameObject;
        }

        return base.FindRangeWeaponModel();
    }
}