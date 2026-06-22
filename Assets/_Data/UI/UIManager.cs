using System.Collections.Generic;
using UnityEngine;

public class UIManager : NhoxSingleton<UIManager>
{
    [Header("UI Menu")] [SerializeField] protected UIMainMenu uiMainMenu;
    [SerializeField] protected UIInGame uiInGame;
    public UIInGame UIInGame => uiInGame;
    [SerializeField] protected UIUpgrades uiUpgrades;
    [SerializeField] protected UISettings uiSettings;
    [SerializeField] protected UICredits uiCredits;

    [Header("Other UI")] 
    [SerializeField] protected UIMissionSelection uiMissionSel;
    [SerializeField] protected UIWeaponSelection uiWeaponSel;
    public UIWeaponSelection UIWeaponSel => uiWeaponSel;

    [SerializeField] protected Transform[] uiElements;
    
    [Header("Switching UI")] 
    protected Transform currentUI;
    protected Stack<Transform> uiHistory = new Stack<Transform>();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIMainMenu();
        LoadUIInGame();
        LoadUIUpgrades();
        LoadUISettings();
        LoadUICredits();
        LoadUIMissionSel();
        LoadUIWeaponSel();
        LoadUIElements();
    }

    protected void LoadUIInGame()
    {
        if (uiInGame != null) return;
        uiInGame = GetComponentInChildren<UIInGame>(true);
        Debug.Log(transform.name + " LoadUIInGame", gameObject);
    }

    protected void LoadUIMainMenu()
    {
        if (uiMainMenu != null) return;
        uiMainMenu = GetComponentInChildren<UIMainMenu>(true);
        Debug.Log(transform.name + " LoadUIMainMenu", gameObject);
    }

    protected void LoadUIUpgrades()
    {
        if (uiUpgrades != null) return;
        uiUpgrades = GetComponentInChildren<UIUpgrades>(true);
        Debug.Log(transform.name + " LoadUIUpgrades", gameObject);
    }

    protected void LoadUISettings()
    {
        if (uiSettings != null) return;
        uiSettings = GetComponentInChildren<UISettings>(true);
        Debug.Log(transform.name + " LoadUISettings", gameObject);
    }

    protected void LoadUICredits()
    {
        if (uiCredits != null) return;
        uiCredits = GetComponentInChildren<UICredits>(true);
        Debug.Log(transform.name + " LoadUICredits", gameObject);
    }

    protected void LoadUIMissionSel()
    {
        if (uiMissionSel != null) return;
        uiMissionSel = GetComponentInChildren<UIMissionSelection>(true);
        Debug.Log(transform.name + " LoadUIMissionSel", gameObject);
    }
    
    protected void LoadUIWeaponSel()
    {
        if (uiWeaponSel != null) return;
        uiWeaponSel = GetComponentInChildren<UIWeaponSelection>(true);
        Debug.Log(transform.name + " LoadUIWeaponSel", gameObject);
    }

    protected void LoadUIElements()
    {
        if (uiElements is { Length: > 0 }) return;

        int childCount = transform.childCount;
        uiElements = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
            uiElements[i] = transform.GetChild(i);
        Debug.Log(transform.name + " LoadUIElements", gameObject);
    }
    
    protected void UpdateHistory(Transform newUI)
    {
        if (currentUI != null)
            uiHistory.Push(currentUI);
            
        currentUI = newUI;
    }

    protected void SwitchTo(Transform ui)
    {
        if (currentUI == ui) return;

        UpdateHistory(ui);
        
        foreach (Transform tr in uiElements)
            tr.gameObject.SetActive(tr == ui);
    }
    
    public void BackToLastUI()
    {
        if (uiHistory.Count > 0)
        {
            Transform lastUI = uiHistory.Pop();
            currentUI = lastUI;

            foreach (Transform tr in uiElements)
                tr.gameObject.SetActive(tr == lastUI);
        }
        else
            SwitchToMainMenu();
    }

    public void SwitchToMainMenu() => SwitchTo(uiMainMenu.transform);
    public void SwitchToInGameUI() => SwitchTo(uiInGame.transform);
    public void SwitchToUpgradesUI() => SwitchTo(uiUpgrades.transform);
    public void SwitchToSettingsUI() => SwitchTo(uiSettings.transform);
    public void SwitchToCreditsUI() => SwitchTo(uiCredits.transform);
    public void SwitchToMissionSelectionUI() => SwitchTo(uiMissionSel.transform);
}