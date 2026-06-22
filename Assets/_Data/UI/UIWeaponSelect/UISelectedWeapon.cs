using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISelectedWeapon : NhoxBehaviour
{
    [SerializeField] protected WeaponDataSO weaponData;
    public WeaponDataSO WeaponData => weaponData;

    [SerializeField] protected Image weaponIcon;
    [SerializeField] protected TextMeshProUGUI weaponInfo;

    protected override void Start()
    {
        base.Start();
        UpdateSlotInfo(null);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponIcon();
        LoadWeaponInfo();
    }

    protected void LoadWeaponIcon()
    {
        if (weaponIcon != null) return;
        weaponIcon = transform.Find("WeaponIcon").GetComponent<Image>();
        Debug.Log(transform.name + " LoadWeaponIcon", gameObject);
    }

    protected void LoadWeaponInfo()
    {
        if (weaponInfo != null) return;
        weaponInfo = transform.GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(transform.name + " LoadWeaponInfo", gameObject);
    }

    public void SetWeaponSlot(WeaponDataSO newWeaponData)
    {
        weaponData = newWeaponData;
        UpdateSlotInfo(weaponData);
    }

    public void UpdateSlotInfo(WeaponDataSO weaponData)
    {
        if (weaponData == null)
        {
            weaponIcon.color = Color.clear;
            weaponInfo.text = "Select a weapon...";
            return;
        }

        weaponIcon.color = Color.white;
        weaponIcon.sprite = weaponData.weaponIcon;
        weaponInfo.text = weaponData.weaponInfo;
    }

    public bool IsEmpty() => weaponData == null;
}