using UnityEngine;

public class UIManager : NhoxSingleton<UIManager>
{
    [SerializeField] protected UIMainMenu uiMainMenu;
    public UIMainMenu UIMainMenu => uiMainMenu;
    [SerializeField] protected UIInGame uiInGame;
    public UIInGame UIInGame => uiInGame;
    [SerializeField] protected UIUpgrades uiUpgrades;
    [SerializeField] protected UISettings uiSettings;
    [SerializeField] protected UICredits uiCredits;
    
    [SerializeField] protected Transform[] uiElements;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIMainMenu();
        LoadUIInGame();
        LoadUIUpgrades();
        LoadUISettings();
        LoadUICredits();
        LoadUIElements();
    }

    protected void LoadUIInGame()
    {
        if(uiInGame != null) return;
        uiInGame = GetComponentInChildren<UIInGame>(true);
        Debug.Log(transform.name + " LoadUIInGame", gameObject);
    }
    
    protected void LoadUIMainMenu()
    {
        if(uiMainMenu != null) return;
        uiMainMenu = GetComponentInChildren<UIMainMenu>(true);
        Debug.Log(transform.name + " LoadUIMainMenu", gameObject);
    }
    
    protected void LoadUIUpgrades()
    {
        if(uiUpgrades != null) return;
        uiUpgrades = GetComponentInChildren<UIUpgrades>(true);
        Debug.Log(transform.name + " LoadUIUpgrades", gameObject);
    }

    protected void LoadUISettings()
    {
        if(uiSettings != null) return;
        uiSettings = GetComponentInChildren<UISettings>(true);
        Debug.Log(transform.name + " LoadUISettings", gameObject);
    }
    
    protected void LoadUICredits()
    {
        if(uiCredits != null) return;
        uiCredits = GetComponentInChildren<UICredits>(true);
        Debug.Log(transform.name + " LoadUICredits", gameObject);
    }
    
    protected void LoadUIElements()
    {
        if(uiElements is { Length: > 0 }) return;
        
        int childCount = transform.childCount;
        uiElements = new Transform[childCount];
        
        for (int i = 0; i < childCount; i++)
            uiElements[i] = transform.GetChild(i);
        Debug.Log(transform.name + " LoadUIElements", gameObject);
    }

    public void SwitchTo(Transform ui)
    {
        foreach (Transform tr in uiElements)
            tr.gameObject.SetActive(tr == ui);
    }
    
    public void SwitchToMainMenu() => SwitchTo(uiMainMenu.transform);
    
    public void SwitchToInGameUI() => SwitchTo(uiInGame.transform);
    
    public void SwitchToUpgradesUI() => SwitchTo(uiUpgrades.transform);
    
    public void SwitchToSettingsUI() => SwitchTo(uiSettings.transform);
    
    public void SwitchToCreditsUI() => SwitchTo(uiCredits.transform);
}
