
using System;
using UnityEngine;

public class PlayerAimCtrl : NhoxBehaviour
{
    [SerializeField] protected PlayerAim aim;
    public PlayerAim Aim => aim;
    
    [SerializeField] protected PlayerCamFollow camFollow;
    [SerializeField] protected PlayerAimVisual aimVisual;

    protected void Update()
    {
        aim.UpdateAimPos();
        camFollow.UpdateCamPos();
        aimVisual.UpdateAimVisuals();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayerAim();
        LoadPlayerCamFollow();
        LoadPlayerAimVisual();
    }

    protected void LoadPlayerAim()
    {
        if (aim != null) return;
        aim = GetComponentInChildren<PlayerAim>();
        Debug.Log(transform.name + " LoadPlayerAim", gameObject);
    }
    
    protected void LoadPlayerCamFollow()
    {
        if (camFollow != null) return;
        camFollow = GetComponentInChildren<PlayerCamFollow>();
        Debug.Log(transform.name + " LoadPlayerCamFollow", gameObject);
    }
    
    protected void LoadPlayerAimVisual()
    {
        if (aimVisual != null) return;
        aimVisual = GetComponentInChildren<PlayerAimVisual>();
        Debug.Log(transform.name + " LoadPlayerAimVisual", gameObject);
    }
}
