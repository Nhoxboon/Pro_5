
using UnityEngine;

public class CreditsBtn : ScaleButton
{
    protected override void OnClick() => UIManager.Instance.SwitchToCreditsUI();
}
