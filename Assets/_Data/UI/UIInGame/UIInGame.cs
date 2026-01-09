using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : NhoxBehaviour
{
    [SerializeField] protected UIHealthBar healthBar;
    [SerializeField] protected UIWeaponSlots weaponSlots;
    public UIWeaponSlots WeaponSlots => weaponSlots;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHealthBar();
        LoadWeaponSlots();
    }

    protected void LoadHealthBar()
    {
        if (healthBar != null) return;
        healthBar = transform.GetComponentInChildren<UIHealthBar>();
        Debug.Log(transform.name + " LoadHealthBar", gameObject);
    }

    protected void LoadWeaponSlots()
    {
        if (weaponSlots != null) return;
        weaponSlots = transform.GetComponentInChildren<UIWeaponSlots>();
        Debug.Log(transform.name + " LoadWeaponSlots", gameObject);
    }
}
