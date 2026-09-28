using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIWeaponSelection : NhoxBehaviour
{
    [SerializeField] protected UISelectedWeapon[] selectedWeapons;
    
    [Header("Warning Text")]
    [SerializeField] protected TextMeshProUGUI warningText;
    [SerializeField] protected float disableWarningSpeed = 1f;
    protected float currentWarningAlpha;
    protected float targetWarningAlpha;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSelectedWeapons();
        LoadWarningTxt();
    }

    protected void Update()
    {
        if (currentWarningAlpha > targetWarningAlpha)
        {
            currentWarningAlpha -= disableWarningSpeed * Time.deltaTime;
            warningText.color = new Color(1, 1, 1, currentWarningAlpha);
        }
    }

    protected void LoadSelectedWeapons()
    {
        if (selectedWeapons is { Length: > 0 }) return;
        selectedWeapons = transform.GetComponentsInChildren<UISelectedWeapon>(true);
        Debug.Log(transform.name + " LoadSelectedWeapons", gameObject);
    }
    
    protected void LoadWarningTxt()
    {
        if (warningText != null) return;
        warningText = transform.Find("WarningText").GetComponent<TextMeshProUGUI>();
        Debug.Log(transform.name + " LoadWarningTxt", gameObject);   
    }
    
    public List<WeaponDataSO> SelectedWeaponsData()
    {
        List<WeaponDataSO> selectedData = new List<WeaponDataSO>();
        foreach (var weapon in selectedWeapons)
        {
            if (weapon != null && !weapon.IsEmpty())
                selectedData.Add(weapon.WeaponData);
        }

        return selectedData;
    }

    public UISelectedWeapon FindEmptySlot()
    {
        foreach (var selectedWeapon in selectedWeapons)
        {
            if (selectedWeapon.IsEmpty())
                return selectedWeapon;
        }

        return null;
    }

    public UISelectedWeapon FindSlotByWeaponType(WeaponDataSO weaponData)
    {
        foreach (var selectedWeapon in selectedWeapons)
        {
            if (selectedWeapon.WeaponData == weaponData)
                return selectedWeapon;
        }

        return null;
    }
    
    public void ShowWarningMessage(string message)
    {
        warningText.color = Color.white;
        warningText.text = message;
        currentWarningAlpha = warningText.color.a;
        targetWarningAlpha = 0;
    }
}