using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : NhoxBehaviour
{
    [SerializeField] protected WeaponDataSO defaultWeaponData;
    [SerializeField] protected Weapon currentWeapon;
    public Weapon CurrentWeapon => currentWeapon;

    [SerializeField] protected int maxSlots = 2;
    [SerializeField] protected List<Weapon> weaponSlots;

    protected string weaponPickupPrefab = "PickupWeapon";

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
        UpdateWeaponUI();
    }

    public void PickupWeapon(Weapon newWeapon)
    {
        if (WeaponInSlots(newWeapon.weaponType) != null)
        {
            WeaponInSlots(newWeapon.weaponType).totalReserveAmmo += newWeapon.bulletsInMagazine;
            return;
        }

        if (weaponSlots.Count >= maxSlots && newWeapon.weaponType != currentWeapon.weaponType)
        {
            int weaponIndex = weaponSlots.IndexOf(currentWeapon);
            PlayerCtrl.Instance.PlayerWeaponVisual.SwitchOffWeaponModels();
            weaponSlots[weaponIndex] = newWeapon;

            CreateWeaponOnDrop();
            EquipWeapon(weaponIndex);
            return;
        }

        weaponSlots.Add(newWeapon);
        PlayerCtrl.Instance.PlayerWeaponVisual.SwitchOnBackupWeapon();
        UpdateWeaponUI();
    }

    public Weapon WeaponInSlots(WeaponType weaponType)
    {
        return weaponSlots.FirstOrDefault(weapon => weapon.weaponType == weaponType);
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
        CreateWeaponOnDrop();

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    protected void CreateWeaponOnDrop()
    {
        Transform dropWeapon = WeaponSpawner.Instance.Spawn(weaponPickupPrefab,
            transform.parent.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        dropWeapon.gameObject.SetActive(true);
        if (dropWeapon.TryGetComponent(out PickupWeapon pickupWeapon))
            pickupWeapon.SetupPickupWeapon(currentWeapon, transform.parent);
    }

    public void UpdateWeaponUI() => UIManager.Instance.UIInGame.WeaponSlots.UpdateWeaponUI(weaponSlots, currentWeapon);
}