using UnityEngine;

public class UIComicPanel : UIShowComic
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayBtn();
    }

    protected void LoadPlayBtn()
    {
        if (btnToEnable != null) return;
        btnToEnable = GetComponentInChildren<PlayGameBtn>(true).transform;
        Debug.Log(transform.name + " :LoadPlayBtn", gameObject);
    }
}