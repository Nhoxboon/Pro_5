using UnityEngine;

public class MenuPlayBtn : ScaleButton
{
    protected override void OnClick() => UIManager.Instance.SwitchToMissionSelectionUI();
}