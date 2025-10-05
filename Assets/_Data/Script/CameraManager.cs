using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : NhoxBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance => instance;

    [SerializeField] protected CinemachineCamera cinemachineCamera;
    [SerializeField] protected CinemachinePositionComposer positionComposer;

    [SerializeField] protected float distanceChangeRate;
    protected float targetCamDistance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only 1 instance of CameraManager allow to exist");
            return;
        }

        instance = this;
    }

    protected void Update()
    {
        // UpdateCameraDistance();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCinemachineCam();
        LoadCinemachineComposer();
    }

    protected void LoadCinemachineCam()
    {
        if (cinemachineCamera != null) return;
        cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
        Debug.Log(transform.name + " LoadCinemachineCam", gameObject);
    }

    protected void LoadCinemachineComposer()
    {
        if (positionComposer != null) return;
        positionComposer = GetComponentInChildren<CinemachinePositionComposer>();
        Debug.Log(transform.name + " LoadCinemachineComposer", gameObject);
    }

    protected void UpdateCameraDistance()
    {
        float currentDistance = positionComposer.CameraDistance;
        if (Math.Abs(targetCamDistance - currentDistance) > 0.01f)
            positionComposer.CameraDistance = Mathf.Lerp(positionComposer.CameraDistance, targetCamDistance,
                distanceChangeRate * Time.deltaTime);
    }

    public void ChangeCameraDistance(float distance) => targetCamDistance = distance;
}