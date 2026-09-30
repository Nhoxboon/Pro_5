using UnityEngine;

public class NextToComicPanelBtn : ScaleButton
{
    protected override void OnClick()
    {
        ConfirmWeaponSelection();
    }

    protected void ConfirmWeaponSelection()
    {
        if(UIManager.Instance.UIWeaponSel.AtLeastOneWeaponSelected())
        {
            UIManager.Instance.SwitchToComicPanelUI();
            LevelGenerator.Instance.InitializeGeneration();
        }
        else
            UIManager.Instance.UIWeaponSel.ShowWarningMessage("Select at least 1 weapon.");
    }
}
