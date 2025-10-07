using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : NhoxBehaviour
{
    [SerializeField] protected WeaponDataSO defaultWeaponData;
    [SerializeField]protected Weapon currentWeapon;
    public Weapon CurrentWeapon => currentWeapon;
    
    [SerializeField] protected int maxSlots = 2;
    [SerializeField] protected List<Weapon> weaponSlots;

    protected override void Start() => Invoke(nameof(EquipStartingWeapon), 0.1f);

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadDefaultWeaponData();
    }

    protected void LoadDefaultWeaponData()
    {
        if (defaultWeaponData != null) return;
        defaultWeaponData = Resources.Load<WeaponDataSO>("Weapon/Pistol");
        Debug.Log(transform.name + " LoadDefaultWeaponData", gameObject);
    }

    protected void EquipStartingWeapon()
    {
        weaponSlots[0] = new Weapon(defaultWeaponData);
        EquipWeapon(0);
    }

    public void EquipWeapon(int i)
    {
        if (i >= weaponSlots.Count) return;
        PlayerCtrl.Instance.PlayerAttack.SetWeaponReady(false);
        
        currentWeapon = weaponSlots[i];
        PlayerCtrl.Instance.PlayerAnim.WeaponEquipAnim();
        CameraManager.Instance.ChangeCameraDistance(currentWeapon.CameraDistance);
    }

    public void PickupWeapon(WeaponDataSO newWeaponData)
    {
        if (weaponSlots.Count >= maxSlots) return;
        weaponSlots.Add(new Weapon(newWeaponData));
        PlayerCtrl.Instance.PlayerWeaponVisual.SwitchOnBackupWeapon();
    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    public bool HasWeaponTypeInInventory(WeaponType type)
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon.weaponType == type)
                return true;
        }
        return false;
    }

    public void DropWeapon()
    {
        if (HasOnlyOneWeapon()) return;
        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }
}