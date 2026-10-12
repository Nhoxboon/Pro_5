using UnityEngine;

public class CarController : NhoxBehaviour
{
    protected PlayerControls controls;
    protected float moveInput;
    protected float steerInput;
    protected bool isBraking;

    protected override void Start()
    {
        base.Start();
        controls = ControlsManager.Instance.Controls;
        ControlsManager.Instance.SwitchToCarControls();

        AssignInputEvents();
    }

    private void AssignInputEvents()
    {
        controls.Car.Movement.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            moveInput = input.y;
            steerInput = input.x;
        };

        controls.Car.Movement.canceled += ctx =>
        {
            moveInput = 0;
            steerInput = 0;
        };

        controls.Car.Brake.performed += ctx => isBraking = true;
        controls.Car.Brake.canceled += ctx => isBraking = false;
    }
}
