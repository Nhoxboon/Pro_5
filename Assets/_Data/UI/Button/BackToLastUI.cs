
using UnityEngine;

public class BackToLastUI : ScaleButton
{
    protected override void OnClick() => UIManager.Instance.BackToLastUI();
}
