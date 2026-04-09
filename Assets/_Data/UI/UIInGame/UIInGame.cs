using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : NhoxBehaviour
{
    [SerializeField] protected UIHealthBar healthBar;
    [SerializeField] protected UIWeaponSlots weaponSlots;
    public UIWeaponSlots WeaponSlots => weaponSlots;
    [SerializeField] protected UIMission uiMission;
    public UIMission UIMission => uiMission;

    [SerializeField] protected Transform missionHelpToolTip;
    [SerializeField] protected Transform missionToolTip;
    protected bool tooltipActive = true;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHealthBar();
        LoadWeaponSlots();
        LoadMission();
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

    protected void LoadMission()
    {
        if (uiMission != null) return;
        uiMission = transform.GetComponentInChildren<UIMission>();
        missionHelpToolTip = transform.Find("MissionInfo - TooltipHelper");
        missionToolTip = uiMission.transform;
        Debug.Log(transform.name + " LoadMission", gameObject);
    }

    public void SwitchMissionTooltip()
    {
        tooltipActive = !tooltipActive;
        missionToolTip.gameObject.SetActive(tooltipActive);
        missionHelpToolTip.gameObject.SetActive(!tooltipActive);
    }
}
