using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ERangeVisuals : EnemyVisuals
{
    [Header("Rig References")] 
    [SerializeField] protected Transform leftHandIK;
    [SerializeField] protected Transform leftElbowIK;
    [SerializeField] protected TwoBoneIKConstraint leftHandIKConstraint;
    [SerializeField] protected MultiAimConstraint weaponAimConstraint;

    [SerializeField] protected EnemySecondWeaponModel[] secondWeaponModels;
    [SerializeField] protected GameObject grenadeModel;
    
    protected float leftHandTargetWeight;
    protected float weaponAimTargetWeight;
    protected float rigChangeRate;

    protected void Update()
    {
        leftHandIKConstraint.weight = AdjustIKWeight(leftHandIKConstraint.weight, leftHandTargetWeight);
        weaponAimConstraint.weight = AdjustIKWeight(weaponAimConstraint.weight, weaponAimTargetWeight);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTwoBoneIKConstraint();
        LoadMultiAimConstraint();
        LoadSecondWeaponModel();
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

    protected void LoadSecondWeaponModel()
    {
        if(secondWeaponModels is { Length: > 0 }) return;
        secondWeaponModels = transform.parent.GetComponentsInChildren<EnemySecondWeaponModel>(true);
        Debug.Log(transform.name + " LoadSecondWeaponModel", gameObject);
    }

    protected void SetupLeftHandIK(Transform leftHandTarget, Transform leftElbowTarget)
    {
        leftHandIK.localPosition = leftHandTarget.localPosition;
        leftHandIK.localRotation = leftHandTarget.localRotation;

        leftElbowIK.localPosition = leftElbowTarget.localPosition;
        leftElbowIK.localRotation = leftElbowTarget.localRotation;
    }
    
    public void EnableGrenadeModel(bool active) => grenadeModel?.SetActive(active);

    public void EnableSecondaryWeaponModel(bool active) => FindSecondaryWeaponModel()?.SetActive(active);

    public void EnableIK(bool enableLeftHand, bool enableAim, float changeRate = 10f)
    {
        rigChangeRate = changeRate;
        leftHandTargetWeight = enableLeftHand ? 1 : 0;
        weaponAimTargetWeight = enableAim ? 1 : 0;
    }

    protected float AdjustIKWeight(float currentWeight, float targetWeight)
    {
        return Mathf.Abs(currentWeight - targetWeight) > 0.05f
            ? Mathf.Lerp(currentWeight, targetWeight, rigChangeRate * Time.deltaTime)
            : targetWeight;
    }

    protected override GameObject FindRangeWeaponModel()
    {
        if (enemy is not EnemyRange { EnemyAttack: ERangeAttack rangeAttack } enemyRange) return null;
        
        EnemyRangeWeaponModel[] weaponModels = transform.parent.GetComponentsInChildren<EnemyRangeWeaponModel>(true);

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType != rangeAttack.WeaponType) continue;
            enemyRange.EnemyAnimator.SwitchAnimationLayer((int)weaponModel.holdType);
            SetupLeftHandIK(weaponModel.leftHandTarget, weaponModel.leftElbowTarget);
            return weaponModel.gameObject;
        }

        return base.FindRangeWeaponModel();
    }

    protected GameObject FindSecondaryWeaponModel()
    {
        if (enemy.EnemyAttack is not ERangeAttack enemyAttack) return null;
    
        EnemyRangeWeaponType weaponType = enemyAttack.WeaponType;
        
        foreach(var weaponModel in secondWeaponModels)
        {
            if (weaponModel.weaponType != weaponType) continue;
            return weaponModel.gameObject;
        }
        return null;
    }
}