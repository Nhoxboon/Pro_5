using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisual : NhoxBehaviour
{
    [SerializeField] protected WeaponModel[] weaponModels;
    [SerializeField] protected BackupWeaponModel[] backupWeaponModels;

    [Header("Left Hand IK")] 
    [SerializeField] protected Transform leftHandIK_Target;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponModel();
        LoadBackupWeapon();
        LoadLeftHandIKTarget();
    }

    protected void LoadWeaponModel()
    {
        if (weaponModels is { Length: > 0 }) return;
        weaponModels = transform.parent.GetComponentsInChildren<WeaponModel>(true);
        Debug.Log(transform.name + "LoadWeaponModel", gameObject);
    }
    
    
    protected void LoadBackupWeapon()
    {
        if(backupWeaponModels is {Length: > 0}) return;
        backupWeaponModels = transform.parent.GetComponentsInChildren<BackupWeaponModel>(true);
        Debug.Log(transform.name + "LoadBackupWeapon", gameObject);
    }

    protected void LoadLeftHandIKTarget()
    {
        if (leftHandIK_Target != null) return;
        leftHandIK_Target = weaponModels[0].transform.parent.parent.Find("LeftHand_IK_target");
        Debug.Log(transform.name + "LoadLeftHandIK", gameObject);
    }

    public void SwitchOnCurrentWeapon()
    {
        PlayerCtrl.Instance.PlayerAnim.SwitchAnimationLayer((int)CurrentWeaponModel().holdType);
        
        SwitchOffWeaponModels();
        
        SwitchOffBackupWeapons();
        if(!PlayerCtrl.Instance.PlayerAttack.HasOnlyOneWeapon())
            SwitchOnBackupWeapon();
        
        CurrentWeaponModel().gameObject.SetActive(true);
        AttachLeftHand();
    }

    protected void SwitchOffWeaponModels()
    {
        foreach (var gun in weaponModels)
            gun.gameObject.SetActive(false);
    }

    protected void SwitchOffBackupWeapons()
    {
        foreach(BackupWeaponModel weapon in backupWeaponModels)
            weapon.Activate(false);
    }

    public void SwitchOnBackupWeapon()
    {
        SwitchOffBackupWeapons();
        BackupWeaponModel lowHangWeapon = null;
        BackupWeaponModel backHangWeapon = null;
        BackupWeaponModel sideHangWeapon = null;

        foreach (BackupWeaponModel weapon in backupWeaponModels)
        {
            if(weapon.weaponType == PlayerCtrl.Instance.PlayerAttack.CurrentWeapon.weaponType) 
                continue;
            
            if (PlayerCtrl.Instance.PlayerAttack.HasWeaponTypeInInventory(weapon.weaponType))
            {
                if(weapon.HangTypeIs(HangType.LowBackHang))
                    lowHangWeapon = weapon;
                if (weapon.HangTypeIs(HangType.BackHang))
                    backHangWeapon = weapon;
                if(weapon.HangTypeIs(HangType.SideHang))
                    sideHangWeapon = weapon;
            }
        }
        lowHangWeapon?.Activate(true);
        backHangWeapon?.Activate(true);
        sideHangWeapon?.Activate(true);
    }

    protected void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;
        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;
    }
    
    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;
        WeaponType weaponType = PlayerCtrl.Instance.PlayerAttack.CurrentWeapon.weaponType;

        foreach (var weapon in weaponModels)
        {
            if (weapon.weaponType == weaponType) 
                weaponModel = weapon;
        }
        return weaponModel;
    }
}