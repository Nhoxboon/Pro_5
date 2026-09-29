using UnityEngine;

public class MainMenuBtn : ScaleButton
{
    protected override void OnClick()
    {
        UIManager.Instance.RestartTheGame();
    }
}
