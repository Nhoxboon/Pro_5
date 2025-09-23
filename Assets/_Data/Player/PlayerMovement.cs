using System;
using UnityEngine;

public class PlayerMovement : NhoxBehaviour
{
    [SerializeField] protected CharacterController charCtrl;

    [Header("Movement Info")] [SerializeField]
    protected Vector3 movementDirection;

    public Vector3 MovementDirection => movementDirection;
    [SerializeField] protected float walkSpeed = 2f;
    [SerializeField] protected float runSpeed = 4f;
    [SerializeField] protected float turnSpeed = 10f;

    protected bool isRunning;
    public bool IsRunning => isRunning;
    protected float verticalVelocity;
    protected float Speed => isRunning ? runSpeed : walkSpeed;

    protected void Update()
    {
        ApplyMovement();
        ApplyRotation();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCharacterCtrl();
    }

    protected void LoadCharacterCtrl()
    {
        if (charCtrl != null) return;
        charCtrl = GetComponentInParent<CharacterController>();
        Debug.Log(transform.name + " LoadCharacterCtrl", gameObject);
    }

    public void SetRunning(bool value) => isRunning = value;

    protected void ApplyMovement()
    {
        Vector2 moveInput = PlayerCtrl.Instance.PlayerInput.MoveInput;
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        if (movementDirection.magnitude > 0)
            charCtrl.Move(movementDirection * (Speed * Time.deltaTime));
    }

    protected void ApplyGravity()
    {
        if (!charCtrl.isGrounded)
        {
            verticalVelocity -= 9.81f * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
            verticalVelocity = -0.5f;
    }

    protected void ApplyRotation()
    {
        Vector3 lookingDirection = PlayerCtrl.Instance.PlayerAim.GetMousePosition() - transform.parent.position;
        lookingDirection.y = 0;
        lookingDirection.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);
        transform.parent.rotation =
            Quaternion.Slerp(transform.parent.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }
}