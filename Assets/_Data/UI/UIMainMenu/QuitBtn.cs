
using UnityEngine;

public class QuitBtn : ScaleButton
{
    protected override void OnClick() => QuitGame();

    protected static void QuitGame() => Application.Quit();
}
