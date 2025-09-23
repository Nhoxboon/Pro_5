using System;
using UnityEngine;

public class PlayerAim : NhoxBehaviour
{
    [SerializeField] protected Camera mainCamera;

    [Header("Aim Info")] [Range(0.5f, 1f)] [SerializeField]
    protected float minCameraDistance = 1f;

    [Range(1f, 3f)] [SerializeField] protected float maxCameraDistance = 3f;

    [Range(3f, 5f)] [SerializeField] protected float aimSensitivity = 4f;

    [SerializeField] protected Transform aim;
    [SerializeField] protected LayerMask aimLayerMask;

    protected void Update()
    {
        aim.position = Vector3.Lerp(aim.position, DesiredAimPos(), aimSensitivity * Time.deltaTime);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMainCamera();
        LoadAimLayer();
        LoadAimPoint();
    }

    protected void LoadMainCamera()
    {
        if (mainCamera != null) return;
        mainCamera = Camera.main;
        Debug.Log(transform.name + " LoadMainCamera", gameObject);
    }

    protected void LoadAimLayer()
    {
        if (aimLayerMask != 0) return;
        aimLayerMask = LayerMask.GetMask("Ground", "Obstacles");
        Debug.Log(transform.name + " LoadAimLayer", gameObject);
    }

    protected void LoadAimPoint()
    {
        if (aim != null) return;
        aim = GameObject.Find("Aim").transform;
        Debug.Log(transform.name + " LoadAimPoint", gameObject);
    }

    protected Vector3 DesiredAimPos()
    {
        bool movingDownwards = PlayerCtrl.Instance.PlayerInput.MoveInput.y < -0.5f;
        
        float actualMaxCameraDistance = movingDownwards ? minCameraDistance : maxCameraDistance;
        
        Vector3 desiredAimPos = GetMousePosition();
        Vector3 aimDirection = (desiredAimPos - transform.parent.position).normalized;
        float distanceToDesiredPos = Vector3.Distance(transform.parent.position, desiredAimPos);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPos, minCameraDistance, actualMaxCameraDistance);

        desiredAimPos = transform.parent.position + aimDirection * clampedDistance;
        desiredAimPos.y = transform.parent.position.y + 1;
        return desiredAimPos;
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(PlayerCtrl.Instance.PlayerInput.AimInput);
        return Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask) ? hitInfo.point : Vector3.zero;
    }
}