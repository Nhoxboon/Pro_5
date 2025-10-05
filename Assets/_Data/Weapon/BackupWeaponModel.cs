using System;
using UnityEngine;

public class BackupWeaponModel : NhoxBehaviour
{
    public WeaponType weaponType;
    [SerializeField] protected HangType hangType;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponType();
    }

    protected void LoadWeaponType()
    {
        string weaponName = transform.name.Replace("Backup", "");
        if (!Enum.TryParse(weaponName, out WeaponType result) || weaponType == result) return;
        weaponType = result;
        Debug.Log(transform.name + "LoadWeaponType", gameObject);
    }

    public void Activate(bool activated) => gameObject.SetActive(activated);

    public bool HangTypeIs(HangType type) => hangType == type;
}