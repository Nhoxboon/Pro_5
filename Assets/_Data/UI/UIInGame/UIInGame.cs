using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : NhoxBehaviour
{
    [SerializeField] protected GameObject playerInfo;
    [SerializeField] protected GameObject carInfo;

    [Header("Player Info")]
    [SerializeField] protected UIHealthBar healthBar;
    [SerializeField] protected UIWeaponSlots weaponSlots;
    public UIWeaponSlots WeaponSlots => weaponSlots;

    [Header("Car Info")]
    [SerializeField] protected UICarSpeed carSpeed;
    [SerializeField] protected UICarHealthBar carHealthBar;

    [Header("Mission Info")]
    [SerializeField] protected UIMission uiMission;
    public UIMission UIMission => uiMission;

    [SerializeField] protected Transform missionHelpToolTip;
    [SerializeField] protected Transform missionToolTip;
    protected bool tooltipActive = true;

    protected CarController subscribedCar;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayerUI();
        LoadCarUI();
        LoadHealthBar();
        LoadWeaponSlots();
        LoadCarSpeedUI();
        LoadCarHealthUI();
        LoadMission();
    }

    protected void LoadPlayerUI()
    {
        if(playerInfo != null) return;
        playerInfo = transform.Find("PlayerInfo").gameObject;
        Debug.Log(transform.name + " LoadPlayerUI", gameObject);
    }

    protected void LoadCarUI()
    {
        if(carInfo != null) return;
        carInfo = transform.Find("CarInfo").gameObject;
        Debug.Log(transform.name + " LoadCarUI", gameObject);
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

    protected void LoadCarSpeedUI()
    {
        if (carSpeed != null) return;
        carSpeed = GetComponentInChildren<UICarSpeed>();
        Debug.Log(transform.name + " LoadCarSpeedUI", gameObject);
    }

    protected void LoadCarHealthUI()
    {
        if (carHealthBar != null) return;
        carHealthBar = GetComponentInChildren<UICarHealthBar>();
        Debug.Log(transform.name + " LoadCarHealthUI", gameObject);
    }

    protected void LoadMission()
    {
        if (uiMission != null) return;
        uiMission = transform.GetComponentInChildren<UIMission>();
        missionHelpToolTip = transform.Find("MissionInfo - TooltipHelper");
        missionToolTip = uiMission.transform;
        Debug.Log(transform.name + " LoadMission", gameObject);
    }

    public void SubscribeToPlayerCar(CarController car)
    {
        UnsubscribeFromCar();

        subscribedCar = car;
        subscribedCar.OnSpeedChanged  += carSpeed.UpdateSpeedTxt;
        subscribedCar.OnHealthChanged += carHealthBar.UpdateCarHealthUI;

        // Immediately sync health UI with car's current state
        car.CarHealth.BroadcastCurrentHealth();
    }

    public void UnsubscribeFromCar()
    {
        if (subscribedCar == null) return;
        subscribedCar.OnSpeedChanged  -= carSpeed.UpdateSpeedTxt;
        subscribedCar.OnHealthChanged -= carHealthBar.UpdateCarHealthUI;
        subscribedCar = null;
    }

    public void SwitchToPlayerUI()
    {
        playerInfo.SetActive(true);
        carInfo.SetActive(false);
    }

    public void SwitchToCarUI()
    {
        playerInfo.SetActive(false);
        carInfo.SetActive(true);
    }

    public void SwitchMissionTooltip()
    {
        tooltipActive = !tooltipActive;
        missionToolTip.gameObject.SetActive(tooltipActive);
        missionHelpToolTip.gameObject.SetActive(!tooltipActive);
    }
}
