using UnityEngine;

public class UIManager : NhoxSingleton<UIManager>
{
    [SerializeField] protected UIInGame uiInGame;
    public UIInGame UIInGame => uiInGame;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIInGame();
    }

    protected void LoadUIInGame()
    {
        if(uiInGame != null) return;
        uiInGame = GetComponentInChildren<UIInGame>();
        Debug.Log(transform.name + " LoadUIInGame", gameObject);
    }
}
