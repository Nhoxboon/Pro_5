using UnityEngine;

public class ResumeBtn : ScaleButton
{
    protected override void OnClick()
    {
        UIManager.Instance.PauseSwitch();
    }
}
