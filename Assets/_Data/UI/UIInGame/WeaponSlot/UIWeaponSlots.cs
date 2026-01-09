using System.Collections.Generic;
using UnityEngine;

public class UIWeaponSlots : NhoxBehaviour
{
    [SerializeField] protected UIWeaponSlot[] weaponSlots;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponSlots();
    }

    protected void LoadWeaponSlots()
    {
        if (weaponSlots is { Length: > 0 }) return;
        weaponSlots = GetComponentsInChildren<UIWeaponSlot>();
        Debug.Log(transform.name + " LoadWeaponSlots", gameObject);
    }

    public void UpdateWeaponUI(List<Weapon> weapons, Weapon currentWeapon)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (i < weapons.Count)
            {
                bool isActiveWeapon = weapons[i] == currentWeapon ? true : false;
                weaponSlots[i].UpdateWeaponSlot(weapons[i], isActiveWeapon);
            }
            else
                weaponSlots[i].UpdateWeaponSlot(null, false);
        }
    }
}
