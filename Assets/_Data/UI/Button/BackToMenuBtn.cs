
using UnityEngine;

public class BackToMenuBtn : ScaleButton
{
    protected override void OnClick() => UIManager.Instance.SwitchToMainMenu();
}
