using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NhoxSingleton<UIManager>
{
    [Header("UI Menu")]
    [SerializeField] protected UIMainMenu uiMainMenu;
    [SerializeField] protected UIInGame uiInGame;
    public UIInGame UIInGame => uiInGame;
    [SerializeField] protected UIUpgrades uiUpgrades;
    [SerializeField] protected UISettings uiSettings;
    [SerializeField] protected UICredits uiCredits;

    [Header("Other UI")]
    [SerializeField] protected UIMissionSelection uiMissionSel;
    [SerializeField] protected UIWeaponSelection uiWeaponSel;
    public UIWeaponSelection UIWeaponSel => uiWeaponSel;

    [SerializeField] protected UIComicPanel uiComicPanel;
    [SerializeField] protected UIPause uiPause;
    [SerializeField] protected UIGameOver uiGameOver;

    [SerializeField] protected Transform[] uiElements;

    [Header("Fade Image")]
    [SerializeField] protected Image  fadeImage;

    [Header("Switching UI")] 
    protected Transform currentUI;
    protected Stack<Transform> uiHistory = new Stack<Transform>();

    protected override void Start()
    {
        base.Start();
        AssignUIInput();
        StartCoroutine(ChangeImageAlpha(0, 1.5f, null));
    }

    #region LoadComponents

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
        LoadUIComicPanel();
        LoadUIPause();
        LoadUIGameOver();
        LoadFadeImage();
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

    protected void LoadUIComicPanel()
    {
        if(uiComicPanel != null) return;
        uiComicPanel = GetComponentInChildren<UIComicPanel>(true);
        Debug.Log(transform.name + " LoadUIComicPanel", gameObject);
    }

    protected void LoadUIPause()
    {
        if (uiPause != null) return;
        uiPause = GetComponentInChildren<UIPause>(true);
        Debug.Log(transform.name + " LoadUIPause", gameObject);
    }

    protected void LoadUIGameOver()
    {
        if (uiGameOver != null) return;
        uiGameOver = GetComponentInChildren<UIGameOver>(true);
        Debug.Log(transform.name + " LoadUIGameOver", gameObject);
    }

    protected void LoadFadeImage()
    {
        if(fadeImage != null) return;
        fadeImage = transform.Find("FadeImage").GetComponent<Image>();
        Debug.Log(transform.name + " LoadFadeImage", gameObject);
    }

    protected void LoadUIElements()
    {
        if (uiElements is { Length: > 0 }) return;

        int childCount = transform.childCount;
        uiElements = new Transform[childCount];
        int index = 0;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if(child.name == "FadeImage")
                continue;
            uiElements[index] = child;
            index++;
        }
        if(index < childCount)
            Array.Resize(ref uiElements, index);
        Debug.Log(transform.name + " LoadUIElements", gameObject);
    }

    #endregion

    protected void AssignUIInput()
    {
        PlayerControls controls = PlayerCtrl.Instance.PlayerInput.Controls;
        controls.UI.UIPause.performed += ctx => PauseSwitch();
    }

    protected void UpdateHistory(Transform newUI)
    {
        if (currentUI != null)
            uiHistory.Push(currentUI);

        currentUI = newUI;
    }

    public void ShowGameOverUI(string msg = "GAME OVER!")
    {
        SwitchTo(uiGameOver.transform);
        uiGameOver.ShowGameOverMessage(msg);
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

    public void StartTheGame() => StartCoroutine(StartGameSequence());

    public void RestartTheGame() => StartCoroutine(ChangeImageAlpha(1, 1f, GameManager.Instance.RestartScene));

    public void PauseSwitch()
    {
        bool gamePaused = uiPause.gameObject.activeSelf;
        if (gamePaused)
        {
            SwitchTo(uiInGame.transform);
            ControlsManager.Instance?.SwitchToCharacterControls();
            TimeManager.Instance?.ResumeTime();
        }
        else
        {
            SwitchTo(uiPause.transform);
            ControlsManager.Instance?.SwitchToUIControls();
            TimeManager.Instance?.PauseTime();
        }
    }

    protected IEnumerator StartGameSequence()
    {
        StartCoroutine(ChangeImageAlpha(1, 1, null));
        yield return new WaitForSeconds(1);
        SwitchTo(uiInGame.transform);
        GameManager.Instance.GameStart();
        StartCoroutine(ChangeImageAlpha(0, 1, null));
    }

    protected IEnumerator ChangeImageAlpha(float targetAlpha, float duration, Action onComplete)
    {
        float time = 0;
        Color currentColor = fadeImage.color;
        float startAlpha = currentColor.a;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        onComplete?.Invoke();
    }

    public void SwitchToMainMenu() => SwitchTo(uiMainMenu.transform);
    public void SwitchToUpgradesUI() => SwitchTo(uiUpgrades.transform);
    public void SwitchToSettingsUI() => SwitchTo(uiSettings.transform);
    public void SwitchToCreditsUI() => SwitchTo(uiCredits.transform);
    public void SwitchToComicPanelUI() => SwitchTo(uiComicPanel.transform);
    public void SwitchToMissionSelectionUI() => SwitchTo(uiMissionSel.transform);
    public void SwitchToWeaponSelectionUI() => SwitchTo(uiWeaponSel.transform);
}