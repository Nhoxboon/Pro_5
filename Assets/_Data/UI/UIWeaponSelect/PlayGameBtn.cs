
using UnityEngine;

public class PlayGameBtn : ScaleButton
{
    protected override void OnClick()
    {
        UIManager.Instance.StartTheGame();
    }
}
