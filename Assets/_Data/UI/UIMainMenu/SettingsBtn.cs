
using UnityEngine;

public class SettingsBtn : ScaleButton
{
    protected override void OnClick() => UIManager.Instance.SwitchToSettingsUI();
}
