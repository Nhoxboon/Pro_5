using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : NhoxBehaviour
{
    protected PlayerControls controls;

    public Vector2 MoveInput { get; protected set; }
    public Vector2 MouseInput { get; protected set; }

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
        ShootInput(instance);
        SetupMovementInput(instance);
        SetupAimInput();

        SetupReloadInput(instance);
        controls.Player.LockTarget.performed += ctx => instance.PlayerAim.Aim.SwitchLockTarget();
        controls.Player.Interaction.performed += _ => instance.PlayerInteraction.InteractWithClosest();

        SetupEquipInput(instance);
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
        controls.Player.Aim.performed += ctx => MouseInput = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += ctx => MouseInput = Vector2.zero;
    }

    protected void ShootInput(PlayerCtrl instance)
    {
        controls.Player.Fire.performed += _ => instance.PlayerAttack.SetIsShooting(true);
        controls.Player.Fire.canceled += _ => instance.PlayerAttack.SetIsShooting(false);

        controls.Player.BurstMode.performed += _ => instance.PlayerAttack.CurrentWeapon.ToggleBurst();
    }

    protected void SetupReloadInput(PlayerCtrl instance)
    {
        controls.Player.Reload.performed += _ => ReloadAction(instance);
    }

    protected void SetupEquipInput(PlayerCtrl instance)
    {
        controls.Player.EquipSlot1.performed += _ => instance.PlayerInventory.EquipWeapon(0);
        controls.Player.EquipSlot2.performed += _ => instance.PlayerInventory.EquipWeapon(1);
        controls.Player.EquipSlot3.performed += _ => instance.PlayerInventory.EquipWeapon(2);
        controls.Player.EquipSlot4.performed += _ => instance.PlayerInventory.EquipWeapon(3);
        controls.Player.EquipSlot5.performed += _ => instance.PlayerInventory.EquipWeapon(4);

        controls.Player.DropCurrentWeapon.performed += _ => instance.PlayerInventory.DropWeapon();
    }

    protected void ReloadAction(PlayerCtrl instance)
    {
        if (!instance.PlayerAttack.CurrentWeapon.CanReload() || !instance.PlayerAttack.WeaponReady) return;
        instance.PlayerAttack.SetWeaponReady(false);
        instance.PlayerAnim.ReloadAnim();
    }
}