using System;
using UnityEngine;

public class WeaponModel : NhoxBehaviour
{
    public WeaponType weaponType;
    public EquipType equipType;
    public HoldType holdType;

    public Transform gunPoint;
    public Transform holdPoint;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponType();
        LoadGunPoint();
        LoadHoldPoint();
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
}