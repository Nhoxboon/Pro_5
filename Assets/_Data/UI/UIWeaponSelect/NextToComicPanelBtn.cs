using UnityEngine;

public class NextToComicPanelBtn : ScaleButton
{
    protected override void OnClick()
    {
        UIManager.Instance.SwitchToComicPanelUI();
        LevelGenerator.Instance.InitializeGeneration();
    }
}
