using UnityEngine;

public class MainMenuBtn : ScaleButton
{
    protected override void OnClick()
    {
        GameManager.Instance.RestartScene();
    }
}
