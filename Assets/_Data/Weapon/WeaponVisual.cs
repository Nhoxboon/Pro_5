
using System;
using UnityEngine;

public class WeaponVisual : NhoxBehaviour
{
    [SerializeField] protected Transform[] gunTransforms;
    [SerializeField] protected Transform[] leftHandTargets;

    protected Transform currentGun;
    [SerializeField] protected Transform leftHand;

    protected override void Start()
    {
        base.Start();
        SwitchGun(0);
    }

    protected void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            if (!Input.GetKeyDown(KeyCode.Alpha1 + i)) continue;
            SwitchGun(i);
            int layer = i < 3 ? 1 : i - 1;
            PlayerCtrl.Instance.PlayerAnim.SwitchAnimationLayer(layer);
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGunTransform();
        LoadLeftHandTargetTrans();
        LoadLeftHandIK();
    }

    protected void LoadGunTransform()
    {
        if (gunTransforms is { Length: > 0 }) return;
        var weapons = transform.parent.GetComponentsInChildren<Weapon>(true);
        gunTransforms = new Transform[weapons.Length];
        for (int i = 0; i < weapons.Length; i++)
            gunTransforms[i] = weapons[i].transform;
        Debug.Log(transform.name + "LoadGunTransform", gameObject);
    }

    protected void LoadLeftHandTargetTrans()
    {
        if (leftHandTargets is { Length: > 0 }) return;
        leftHandTargets = new Transform[gunTransforms.Length];
        for(int i = 0; i < gunTransforms.Length; i++)
            leftHandTargets[i] = gunTransforms[i].Find("LeftHand_TargetTrans");
        
        Debug.Log(transform.name + "LoadLeftHandTargetTrans", gameObject);
    }

    protected void LoadLeftHandIK()
    {
        if(leftHand != null) return;
        leftHand = gunTransforms[0].parent.parent.Find("LeftHand_IK_target");
        Debug.Log(transform.name + "LoadLeftHandIK", gameObject);
    }

    protected void SwitchOn(Transform gun)
    {
        SwitchOffGuns();
        gun.gameObject.SetActive(true);
        
        currentGun = gun;
        int index = Array.IndexOf(gunTransforms, gun);
        if (index >= 0) AttachLeftHand(index);
    }

    protected void SwitchOffGuns()
    {
        foreach (var gun in gunTransforms)
            gun.gameObject.SetActive(false);
    }
    
    public void SwitchGun(int index)
    {
        if (index < 0 || index >= gunTransforms.Length) return;
        SwitchOn(gunTransforms[index]);
    }
    
    protected void AttachLeftHand(int index)
    {
        Transform targetTransform = leftHandTargets[index];
        leftHand.localPosition = targetTransform.localPosition;
        leftHand.localRotation = targetTransform.localRotation;
    }
}
