
using System;
using UnityEngine;

public class PlayerAnimator : NhoxBehaviour
{
    [SerializeField] protected Animator anim;

    protected void Update() => AnimatorCtrl();
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAnimator();
    }

    protected void LoadAnimator()
    {
        if(anim != null) return;
        anim = transform.parent.GetComponentInChildren<Animator>();
        Debug.Log(transform.name + " LoadAnimator", gameObject);
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

    public void ShootAnim() => anim.SetTrigger("fire");
}
