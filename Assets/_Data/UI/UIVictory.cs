using UnityEngine;

public class UIVictory : UIShowComic
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMenuBtn();
    }

    protected void LoadMenuBtn()
    {
        if (btnToEnable != null) return;
        btnToEnable = GetComponentInChildren<MainMenuBtn>(true).transform;
        Debug.Log(transform.name + " :LoadMenuBtn", gameObject);
    }
}