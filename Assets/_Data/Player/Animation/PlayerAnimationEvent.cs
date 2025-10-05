using UnityEngine;

public class PlayerAnimationEvent : NhoxBehaviour
{
    public void ReloadIsOver()
    {
        var instance = PlayerCtrl.Instance;
        instance.PlayerAnim.RestoreRigWeight();
        instance.PlayerAttack.CurrentWeapon.RefillBullets();
        instance.PlayerAttack.SetWeaponReady(true);
    }

    public void RestoreWeight()
    {
        PlayerCtrl.Instance.PlayerAnim.RestoreRigWeight();
        PlayerCtrl.Instance.PlayerAnim.RestoreLeftHandIKWeight();
    }

    public void WeaponEquipIsOver() => PlayerCtrl.Instance.PlayerAttack.SetWeaponReady(true);
    
    public void SwitchOnWeaponModel() => PlayerCtrl.Instance.PlayerWeaponVisual.SwitchOnCurrentWeapon();
}