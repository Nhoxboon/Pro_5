
using UnityEngine;

public class PlayBtn : ScaleButton
{
    protected override void OnClick() => UIManager.Instance.SwitchToInGameUI();
}
