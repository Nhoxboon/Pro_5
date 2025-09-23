using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : NhoxBehaviour
{
    protected PlayerControls controls;

    public Vector2 MoveInput { get; protected set; }
    public Vector2 AimInput { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        controls = new PlayerControls();
        SetupInputEvents();
    }

    protected void OnEnable() => controls.Enable();

    protected void OnDisable() => controls.Disable();

    protected void SetupInputEvents()
    {
        var instance = PlayerCtrl.Instance;

        controls.Player.Fire.performed += ctx => ShootInput();

        SetupMovementInput(instance);
        SetupAimInput();

        controls.Player.Reload.performed += ctx => instance.PlayerAnim.ReloadAnim();
        // controls.Player.SwitchWeapon.performed += OnSwitchWeapon;
    }

    protected void SetupMovementInput(PlayerCtrl instance)
    {
        controls.Player.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => MoveInput = Vector2.zero;

        controls.Player.Run.performed += ctx => instance.PlayerMovement.SetRunning(true);
        controls.Player.Run.canceled += ctx => instance.PlayerMovement.SetRunning(false);
    }

    protected void SetupAimInput()
    {
        controls.Player.Aim.performed += ctx => AimInput = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += ctx => AimInput = Vector2.zero;
    }

    protected void ShootInput()
    {
        PlayerCtrl.Instance.PlayerAnim.ShootAnim();
    }

    // protected void OnSwitchWeapon(InputAction.CallbackContext ctx)
    // {
    //     if (int.TryParse(ctx.control.name, out int weaponIndex))
    //         weaponVisual.SwitchGun(weaponIndex - 1);
    // }
}