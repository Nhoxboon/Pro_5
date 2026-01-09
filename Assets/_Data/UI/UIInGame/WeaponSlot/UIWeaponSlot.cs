using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponSlot : NhoxBehaviour
{
    [SerializeField] protected Image weaponIcon;
    [SerializeField] protected TextMeshProUGUI ammoText;

    protected Weapon currentWeapon;
    protected bool isActiveWeapon;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadIcon();
        LoadAmmoText();
    }

    protected void LoadIcon()
    {
        if (weaponIcon != null) return;
        weaponIcon = GetComponentInChildren<Image>();
        Debug.Log(transform.name + " LoadIcon", gameObject);
    }

    protected void LoadAmmoText()
    {
        if(ammoText != null) return;
        ammoText = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(transform.name + " LoadAmmoText", gameObject);
    }

    public void UpdateWeaponSlot(Weapon myWeapon, bool activeWeapon)
    {
        UnsubscribeFromWeapon();

        currentWeapon = myWeapon;
        isActiveWeapon = activeWeapon;

        if (myWeapon == null)
        {
            ClearSlot();
            return;
        }

        SubscribeToWeapon();
        UpdateWeaponIcon();
        UpdateAmmoText();
    }

    protected void SubscribeToWeapon()
    {
        if (currentWeapon == null) return;
        currentWeapon.OnAmmoChanged += UpdateAmmoText;
    }

    protected void UnsubscribeFromWeapon()
    {
        if (currentWeapon == null) return;
        currentWeapon.OnAmmoChanged -= UpdateAmmoText;
    }

    protected void UpdateWeaponIcon()
    {
        if (currentWeapon == null) return;

        Color newColor = isActiveWeapon ? Color.white : new Color(1f, 1f, 1f, 0.35f);
        weaponIcon.color = newColor;
        weaponIcon.sprite = currentWeapon.WeaponData.weaponIcon;
    }

    protected void UpdateAmmoText()
    {
        if (currentWeapon == null) return;
        ammoText.text = currentWeapon.bulletsInMagazine + "/" + currentWeapon.totalReserveAmmo;
    }

    protected void ClearSlot()
    {
        weaponIcon.color = Color.clear;
        ammoText.text = "";
    }

    protected void OnDestroy() => UnsubscribeFromWeapon();
}
