using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSelectBtn : ScaleButton
{
    [SerializeField] protected WeaponDataSO weaponData;
    [SerializeField] protected UIWeaponSelection weaponSelection;

    [SerializeField] protected Image weaponIcon;

    protected UISelectedWeapon emptySlot;

    protected override void Start()
    {
        base.Start();
        weaponIcon.sprite = weaponData.weaponIcon;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponSelection();
        LoadWeaponIcon();
    }

    protected void LoadWeaponSelection()
    {
        if (weaponSelection != null) return;
        weaponSelection = transform.GetComponentInParent<UIWeaponSelection>();
        Debug.Log(transform.name + " LoadWeaponSelection", gameObject);
    }

    protected void LoadWeaponIcon()
    {
        if (weaponIcon != null) return;
        weaponIcon = transform.Find("WeaponIcon").GetComponent<Image>();
        Debug.Log(transform.name + " LoadWeaponIcon", gameObject);
    }

    protected override void OnClick()
    {
        ManageWeaponSlot();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        weaponIcon.color = Color.yellow;
        emptySlot = weaponSelection.FindEmptySlot();
        emptySlot?.UpdateSlotInfo(weaponData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        weaponIcon.color = Color.white;
        emptySlot?.UpdateSlotInfo(null);
        emptySlot = null;
    }
    
    protected void ManageWeaponSlot()
    {
        bool noMoreEmptySlot = weaponSelection.FindEmptySlot() == null;
        bool noThisWeaponInSlots = weaponSelection.FindSlotByWeaponType(weaponData) == null;
        if(noMoreEmptySlot && noThisWeaponInSlots)
        {
            weaponSelection.ShowWarningMessage("No empty slot...");
            return;
        }
        
        UISelectedWeapon busyWeaponSlot = weaponSelection.FindSlotByWeaponType(weaponData);
        
        if (busyWeaponSlot != null)
            busyWeaponSlot.SetWeaponSlot(null);
        else
        {
            emptySlot = weaponSelection.FindEmptySlot();
            emptySlot?.SetWeaponSlot(weaponData);
        }
        
        emptySlot = null;
    }

    protected override void ResetBtn()
    {
        base.ResetBtn();
        weaponIcon.color = Color.white;
    }
}