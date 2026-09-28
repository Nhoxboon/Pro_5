using UnityEngine;

public class ControlsManager : NhoxSingleton<ControlsManager>
{
    public PlayerControls controls {get; private set;}

    protected override void Start()
    {
        base.Start();
        controls = PlayerCtrl.Instance?.PlayerInput.Controls;
        SwitchToCharacterControls();
    }

    public void SwitchToCharacterControls()
    {
        controls.UI.Disable();
        controls.Player.Enable();
        PlayerCtrl.Instance?.PlayerInput.SetControlsEnabled(true);
    }

    public void SwitchToUIControls()
    {
        controls.UI.Enable();
        controls.Player.Disable();
        PlayerCtrl.Instance?.PlayerInput.SetControlsEnabled(false);
    }
}
