using System;
using UnityEngine;

public class WeaponModel : NhoxBehaviour
{
    public WeaponType weaponType;
    public EquipType equipType;
    public HoldType holdType;

    public Transform gunPoint;
    public Transform holdPoint;

    public AudioSource fireSfx;
    public AudioSource reloadSfx;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponType();
        LoadGunPoint();
        LoadHoldPoint();
        LoadFireSfx();
        LoadReloadSfx();
    }

    protected void LoadWeaponType()
    {
        if (!Enum.TryParse(transform.name, out WeaponType result) || weaponType == result) return;
        weaponType = result;
        Debug.Log(transform.name + "LoadWeaponType", gameObject);
    }

    protected void LoadGunPoint()
    {
        if (gunPoint != null) return;
        gunPoint = transform.Find("GunPoint");
        Debug.Log(transform.name + " LoadGunPoint", gameObject);
    }

    protected void LoadHoldPoint()
    {
        if (holdPoint != null) return;
        holdPoint = transform.Find("LeftHand_TargetTrans");
        Debug.Log(transform.name + " LoadHoldPoint", gameObject);
    }

    protected void LoadFireSfx()
    {
        if (fireSfx != null) return;
        fireSfx = transform.Find("SFX/Fire").GetComponent<AudioSource>();
        Debug.Log(transform.name + " LoadFireSfx", gameObject);
    }

    protected void LoadReloadSfx()
    {
        if(reloadSfx != null) return;
        reloadSfx = transform.Find("SFX/Reload").GetComponent<AudioSource>();
        Debug.Log(transform.name + " LoadReloadSfx", gameObject);
    }
}