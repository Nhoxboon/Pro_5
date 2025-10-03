using UnityEngine;

public class PlayerAnimationEvent : NhoxBehaviour
{
    public void ReloadIsOver()
    {
        PlayerCtrl.Instance.PlayerAnim.RestoreRigWeight();
        PlayerCtrl.Instance.PlayerAttack.CurrentWeapon.RefillBullets();
    }

    public void RestoreWeight()
    {
        PlayerCtrl.Instance.PlayerAnim.RestoreRigWeight();
        PlayerCtrl.Instance.PlayerAnim.RestoreLeftHandIKWeight();
    }

    public void WeaponGrabIsOver() => PlayerCtrl.Instance.PlayerAnim.NotBusyGrab();
    
    public void SwitchOnWeaponModel() => PlayerCtrl.Instance.PlayerWeaponVisual.SwitchOnCurrentWeapon();
}