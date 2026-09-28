using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : NhoxSingleton<CameraManager>
{
    [SerializeField] protected CinemachineCamera cinemachineCamera;
    [SerializeField] protected CinemachinePositionComposer positionComposer;

    [SerializeField] private bool canChangeCameraDistance;
    [SerializeField] protected float distanceChangeRate;
    [SerializeField] protected float targetCamDistance;

    protected void Update()
    {
        UpdateCameraDistance();
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
        if (!canChangeCameraDistance)
            return;

        float currentDistance = positionComposer.CameraDistance;
        if (Math.Abs(targetCamDistance - currentDistance) > 0.01f)
            positionComposer.CameraDistance = Mathf.Lerp(positionComposer.CameraDistance, targetCamDistance,
                distanceChangeRate * Time.deltaTime);
    }

    public void ChangeCameraDistance(float distance) => targetCamDistance = distance;
}