
using System;
using UnityEngine;

public class PlayerMovement : NhoxBehaviour
{

    [SerializeField] protected LayerMask aimLayerMask;
    [SerializeField] protected CharacterController charCtrl;
    
    [Header("Movement Info")]
    [SerializeField] protected Vector3 movementDirection;
    public Vector3 MovementDirection => movementDirection;
    [SerializeField] protected float walkSpeed = 2f;
    [SerializeField] protected float runSpeed = 4f;
    protected bool isRunning;
    public bool IsRunning => isRunning;
    protected float verticalVelocity;
    protected float Speed => isRunning ? runSpeed : walkSpeed;

    [Header("Aim Info")]
    [SerializeField] protected Transform aim;
    [SerializeField] protected Camera mainCamera;
    protected Vector3 lookingDirection;

    protected void Update()
    {
        ApplyMovement();
        AimTowardsMouse();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCharacterCtrl();
        LoadMainCamera();
        LoadAimLayer();
    }

    protected void LoadCharacterCtrl()
    {
        if(charCtrl != null) return;
        charCtrl = GetComponentInParent<CharacterController>();
        Debug.Log(transform.name + " LoadCharacterCtrl", gameObject);
    }
    
    protected void LoadMainCamera()
    {
        if(mainCamera != null) return;
        mainCamera = Camera.main;
        Debug.Log(transform.name + " LoadMainCamera", gameObject);
    }
    
    protected void LoadAimLayer()
    {
        if(aimLayerMask != 0) return;
        aimLayerMask = LayerMask.GetMask("Ground", "Obstacles");
        Debug.Log(transform.name + " LoadAimLayer", gameObject);
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
    
    protected void AimTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(PlayerCtrl.Instance.PlayerInput.AimInput);
        if (!Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask)) return;
        lookingDirection = hitInfo.point - transform.parent.position;
        lookingDirection.y = 0;
        lookingDirection.Normalize();
            
        transform.parent.forward = lookingDirection;
            
        aim.position = new Vector3(hitInfo.point.x, transform.parent.position.y, hitInfo.point.z);
    }

}
