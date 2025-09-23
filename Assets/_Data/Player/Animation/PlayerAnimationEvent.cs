using UnityEngine;

public class PlayerAnimationEvent : NhoxBehaviour
{
    public void ReloadIsOver() => PlayerCtrl.Instance.PlayerAnim.RestoreRigWeight();
    
    public void RestoreWeight()
    {
        PlayerCtrl.Instance.PlayerAnim.RestoreRigWeight();
        PlayerCtrl.Instance.PlayerAnim.RestoreLeftHandIKWeight();
    }
    
    public void WeaponGrabIsOver() => PlayerCtrl.Instance.PlayerAnim.NotBusyGrab();
}
