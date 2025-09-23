
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimator : NhoxBehaviour
{
    [SerializeField] protected Animator anim;
    protected bool busyGrabWeapon;
    
    [Header("Rig")]
    [SerializeField] protected Rig rig;
    [SerializeField] protected float rigIncreaseRate = 2.75f;
    protected bool rigShouldIncrease;
    
    [Header("Left Hand IK")]
    [SerializeField] protected TwoBoneIKConstraint leftHandIK;
    [SerializeField] protected float leftHandIKIncreaseRate = 2.5f;
    protected bool leftHandIKShouldIncrease;

    protected void Update()
    {
        AnimatorCtrl();
        AdjustRigWeight();
        AdjustLeftHandIKWeight();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAnimator();
        LoadRig();
        LoadLeftHandIKConstraint();
    }

    protected void LoadAnimator()
    {
        if(anim != null) return;
        anim = transform.parent.GetComponentInChildren<Animator>();
        Debug.Log(transform.name + " LoadAnimator", gameObject);
    }
    
    protected void LoadRig()
    {
        if(rig != null) return;
        rig = transform.parent.GetComponentInChildren<Rig>();
        Debug.Log(transform.name + "LoadRig", gameObject);
    }
    
    protected void LoadLeftHandIKConstraint()
    {
        if(leftHandIK != null) return;
        leftHandIK = transform.parent.GetComponentInChildren<TwoBoneIKConstraint>();
        Debug.Log(transform.name + "LoadLeftHandIKConstraint", gameObject);
    }
    
    protected void AnimatorCtrl()
    {
        var movement = PlayerCtrl.Instance.PlayerMovement;
        
        float xVelocity = Vector3.Dot(movement.MovementDirection.normalized, transform.parent.right);
        float zVelocity = Vector3.Dot(movement.MovementDirection.normalized, transform.parent.forward);
        
        anim.SetFloat("xVelocity", xVelocity, 0.1f, Time.deltaTime);
        anim.SetFloat("zVelocity", zVelocity, 0.1f, Time.deltaTime);
        
        bool playRunAnimation = movement.IsRunning && movement.MovementDirection.magnitude > 0;
        anim.SetBool("isRunning", playRunAnimation);
    }

    public void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
            anim.SetLayerWeight(i, 0);
        
        anim.SetLayerWeight(layerIndex, 1);
    }
    
    protected void AdjustRigWeight()
    {
        if (!rigShouldIncrease) return;
        rig.weight += rigIncreaseRate * Time.deltaTime;
        if (rig.weight >= 1)
            rigShouldIncrease = false;
    }
    
    protected void AdjustLeftHandIKWeight()
    {
        if (!leftHandIKShouldIncrease) return;
        leftHandIK.weight += leftHandIKIncreaseRate * Time.deltaTime;
        if (leftHandIK.weight >= 1)
            leftHandIKShouldIncrease = false;
    }
    
    protected void ReduceRigWeight() => rig.weight = 0.15f;
    
    public void ReloadAnim()
    {
        if(busyGrabWeapon) return;
        ReduceRigWeight();
        anim.SetTrigger("reload");
    }
    
    public void RestoreRigWeight() => rigShouldIncrease = true;
    public void RestoreLeftHandIKWeight() => leftHandIKShouldIncrease = true;
    public void NotBusyGrab() => SetBusyGrabWeapon(false);

    public void WeaponGrabAnim(GrabType grabType)
    {
        leftHandIK.weight = 0;
        ReduceRigWeight();
        anim.SetFloat("grabType", (float)grabType);
        anim.SetTrigger("weaponGrab");
        
        SetBusyGrabWeapon(true);
    }

    protected void SetBusyGrabWeapon(bool value)
    {
        busyGrabWeapon = value;
        anim.SetBool("busyGrab", busyGrabWeapon);
    }

    public void ShootAnim() => anim.SetTrigger("fire");
}
