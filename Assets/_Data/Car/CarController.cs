using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : NhoxBehaviour
{
    protected PlayerControls controls;
    protected float moveInput;
    protected float steerInput;

    protected bool carEnabled;

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected CarWheel[] wheels;

    [Range(30, 60)] [SerializeField] protected float turnSensitivity = 30;

    [Header("Car Settings")]
    [SerializeField] protected DriveType driveType;
    [SerializeField] protected Transform centerOfMass;
    [Range(350, 1000)]
    [SerializeField] protected float carMass = 400;
    [Range(20, 80)]
    [SerializeField] protected float wheelMass = 30;
    [Range(0.5f, 2f)]
    [SerializeField] protected float frontWheelTraction = 1;
    [Range(0.5f, 2f)]
    [SerializeField] protected float backWheelTraction = 1;

    [Header("Engine Settings")]
    [Range(7, 12)]
    [SerializeField] protected float maxSpeed = 10;
    [Range(0.5f, 10)]
    [SerializeField] protected float accelerationSpeed = 5;
    [Range(1500f, 5000)]
    [SerializeField] protected float motorForce = 1700f;
    protected float currentSpeed;

    [Header("Brake Settings")]
    [Range(0, 10)]
    [SerializeField] protected float frontBrakesSensitivity = 5;
    [Range(0, 10)]
    [SerializeField] protected float backBrakesSensitivity = 5;
    [Range(4000, 6000)]
    [SerializeField] protected float brakePower = 5000;
    protected bool isBraking;

    [Header("Drift Settings")]
    [Range(0, 1)]
    [SerializeField] protected float frontDriftFactor = 0.5f;
    [Range(0, 1)] [SerializeField] protected float backDriftFactor = 0.5f;
    [SerializeField] protected float driftDuration = 1f;
    protected float driftTimer;
    protected bool isDrifting;

    protected override void Start()
    {
        base.Start();
        controls = ControlsManager.Instance.Controls;
        ControlsManager.Instance.SwitchToCarControls();

        AssignInputEvents();
        SetupDefaultValues();
    }

    protected void Update()
    {
        if(!carEnabled) return;

        driftTimer -= Time.deltaTime;
        if (driftTimer <= 0f)
            isDrifting = false;
    }

    protected void FixedUpdate()
    {
        if(!carEnabled) return;

        AnimateWheels();
        ApplyDrive();
        ApplySteering();
        ApplyBrake();
        ApplySpeedLimit();

        HandleDrifting();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCarWheels();
        LoadRigidbody();
    }

    protected void LoadCarWheels()
    {
        if (wheels.Length != 0) return;
        wheels = GetComponentsInChildren<CarWheel>();
        Debug.Log(transform.name + " LoadCarWheels", gameObject);
    }

    protected void LoadRigidbody()
    {
        if (rb != null) return;
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        centerOfMass = transform.Find("CenterOfMass");
        Debug.Log(transform.name + " LoadRigidbody", gameObject);
    }

    public void ActivateCar(bool active) => carEnabled = active;

    protected void SetupDefaultValues()
    {
        rb.centerOfMass = centerOfMass.localPosition;
        rb.mass = carMass;
        foreach (CarWheel wheel in wheels)
        {
            wheel.Col.mass = wheelMass;
            if(wheel.axelType == AxelType.Front)
                wheel.SetDefaultStiffness(frontWheelTraction);
            if(wheel.axelType == AxelType.Back)
                wheel.SetDefaultStiffness(backWheelTraction);
        }
    }

    protected void AssignInputEvents()
    {
        controls.Car.Movement.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            moveInput = input.y;
            steerInput = input.x;
        };

        controls.Car.Movement.canceled += _ =>
        {
            moveInput = 0;
            steerInput = 0;
        };

        controls.Car.Brake.performed += _ =>
        {
            isBraking = true;
            isDrifting = true;
            driftTimer = driftDuration;
        };
        controls.Car.Brake.canceled += _ => isBraking = false;
    }

    protected void AnimateWheels()
    {
        foreach (CarWheel wheel in wheels)
        {
            wheel.Col.GetWorldPose(out Vector3 position, out Quaternion rotation);
            if (wheel.Model is null) continue;
            wheel.Model.transform.position = position;
            wheel.Model.transform.rotation = rotation;
        }
    }

    protected void HandleDrifting()
    {
        if (isDrifting)
            ApplyDrift();
        else
            StopDrift();
    }

    protected void ApplyDrive()
    {
        currentSpeed = moveInput * accelerationSpeed * Time.fixedDeltaTime;
        float motorTorqueValue = motorForce * currentSpeed;

        foreach (CarWheel wheel in wheels)
        {
            if (driveType == DriveType.FrontWheelDrive)
            {
                if (wheel.axelType == AxelType.Front)
                    wheel.Col.motorTorque = motorTorqueValue;
            }
            else if (driveType == DriveType.RearWheelDrive)
            {
                if (wheel.axelType == AxelType.Back)
                    wheel.Col.motorTorque = motorTorqueValue;
            }
            else
                wheel.Col.motorTorque = motorTorqueValue;
        }
    }

    protected void ApplySpeedLimit()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    protected void ApplySteering()
    {
        foreach (CarWheel wheel in wheels)
        {
            if (wheel.axelType == AxelType.Front)
            {
                float targetSteerAngle = steerInput * turnSensitivity;
                wheel.Col.steerAngle = Mathf.Lerp(wheel.Col.steerAngle, targetSteerAngle, 0.5f);
            }
        }
    }

    protected void ApplyBrake()
    {
        foreach (CarWheel wheel in wheels)
        {
            bool frontBrakes = wheel.axelType == AxelType.Front;
            float brakeSensitivity = frontBrakes ? frontBrakesSensitivity : backBrakesSensitivity;

            float newBrakeTorque = brakePower * brakeSensitivity * Time.fixedDeltaTime;
            float currentBrakeTorque = isBraking ? newBrakeTorque : 0;

            wheel.Col.brakeTorque = currentBrakeTorque;
        }
    }

    protected void ApplyDrift()
    {
        foreach (CarWheel wheel in wheels)
        {
            bool frontWheel = wheel.axelType == AxelType.Front;
            float driftFactor = frontWheel ? frontDriftFactor : backDriftFactor;

            WheelFrictionCurve sidewaysFriction = wheel.Col.sidewaysFriction;
            sidewaysFriction.stiffness *= 1 - driftFactor;
            wheel.Col.sidewaysFriction = sidewaysFriction;
        }
    }

    protected void StopDrift()
    {
        foreach (CarWheel wheel in wheels)
            wheel.RestoreDefaultStiffness();
    }

    [ContextMenu("Focus camera and enable")]
    public void TestThisCar()
    {
        ActivateCar(true);
        CameraManager.Instance.ChangeCameraTarget(this.transform, 12f);
        ControlsManager.Instance.SwitchToCarControls();
    }
}