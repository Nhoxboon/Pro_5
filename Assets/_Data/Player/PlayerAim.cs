using System;
using UnityEngine;

public class PlayerAim : NhoxBehaviour
{
    [SerializeField] protected Camera mainCamera;
    [SerializeField] protected Transform cameraTarget;

    [Header("Camera Info")] [Range(0.5f, 1f)] [SerializeField]
    protected float minCameraDistance = 1f;

    [Range(1f, 3f)] [SerializeField] protected float maxCameraDistance = 3f;
    [Range(3f, 5f)] [SerializeField] protected float camSensitivity = 4f;

    [Header("Aim Info")] [SerializeField] protected Transform aim;

    [SerializeField] protected LayerMask aimLayerMask;
    protected RaycastHit lastKnownMouseHit;

    protected void Update()
    {
        aim.position = GetMouseHitInfo().point;
        aim.position = new Vector3(aim.position.x, transform.parent.position.y + 1f, aim.position.z);

        cameraTarget.position =
            Vector3.Lerp(cameraTarget.position, DesiredCameraPos(), camSensitivity * Time.deltaTime);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMainCamera();
        LoadAimLayer();
        LoadCamTarget();
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

    protected void LoadCamTarget()
    {
        if (cameraTarget != null) return;
        cameraTarget = GameObject.Find("CameraTarget").transform;
        Debug.Log(transform.name + " LoadCamTarget", gameObject);
    }

    protected void LoadAimPoint()
    {
        if (aim != null) return;
        aim = GameObject.Find("AimPoint").transform;
        Debug.Log(transform.name + " LoadAimPoint", gameObject);
    }

    protected Vector3 DesiredCameraPos()
    {
        float actualMaxCameraDistance =
            PlayerCtrl.Instance.PlayerInput.MoveInput.y < -0.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCamPos = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCamPos - transform.parent.position).normalized;
        float distanceToDesiredPos = Vector3.Distance(transform.parent.position, desiredCamPos);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPos, minCameraDistance, actualMaxCameraDistance);

        desiredCamPos = transform.parent.position + aimDirection * clampedDistance;
        desiredCamPos.y = transform.parent.position.y + 1;
        return desiredCamPos;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = mainCamera.ScreenPointToRay(PlayerCtrl.Instance.PlayerInput.AimInput);

        if (!Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
            return lastKnownMouseHit;
        lastKnownMouseHit = hitInfo;
        return hitInfo;
    }
}