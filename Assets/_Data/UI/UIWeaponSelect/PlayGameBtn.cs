
using UnityEngine;

public class PlayGameBtn : ScaleButton
{
    protected override void OnClick()
    {
        UIManager.Instance.SwitchToInGameUI();
        GameManager.Instance.SetDefaultWeapons();
    }
}
