
using UnityEngine;

public class UpgradesBtn : ScaleButton
{
    protected override void OnClick() => UIManager.Instance.SwitchToUpgradesUI();
}
