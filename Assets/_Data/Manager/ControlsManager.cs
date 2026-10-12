using UnityEngine;

public class ControlsManager : NhoxSingleton<ControlsManager>
{
    public PlayerControls Controls {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        Controls = new PlayerControls();
    }

    protected override void Start()
    {
        base.Start();
        SwitchToCharacterControls();
    }

    public void SwitchToCharacterControls()
    {
        Controls.Player.Enable();
        Controls.UI.Disable();
        Controls.Car.Disable();
        PlayerCtrl.Instance?.PlayerInput.SetControlsEnabled(true);
    }

    public void SwitchToUIControls()
    {
        Controls.UI.Enable();
        Controls.Player.Disable();
        Controls.Car.Disable();
        PlayerCtrl.Instance?.PlayerInput.SetControlsEnabled(false);
    }

    public void SwitchToCarControls()
    {
        Controls.Car.Enable();
        Controls.UI.Disable();
        Controls.Player.Disable();
        PlayerCtrl.Instance?.PlayerInput.SetControlsEnabled(false);
    }
}
