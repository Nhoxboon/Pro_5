using System;
using UnityEngine;

public class PlayerCamFollow : AimComponent
{
    [Header("Camera Info")] 
    [SerializeField] protected Transform cameraTarget;

    [Range(0.5f, 1f)] 
    [SerializeField] protected float minCameraDistance = 1f;
    [Range(1f, 3f)] 
    [SerializeField] protected float maxCameraDistance = 3f;
    [Range(3f, 5f)] 
    [SerializeField] protected float camSensitivity = 3.5f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCamTarget();
    }
    
    protected void LoadCamTarget()
    {
        if (cameraTarget != null) return;
        cameraTarget = GameObject.Find("CameraTarget").transform;
        Debug.Log(transform.name + " LoadCamTarget", gameObject);
    }
    
    public void UpdateCamPos()
    {
        cameraTarget.position =
            Vector3.Lerp(cameraTarget.position, DesiredCameraPos(), camSensitivity * Time.deltaTime);
    }

    protected Vector3 DesiredCameraPos()
    {
        float actualMaxCameraDistance =
            PlayerCtrl.Instance.PlayerInput.MoveInput.y < -0.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCamPos = aimCtrl.Aim.GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCamPos - aimCtrl.transform.parent.position).normalized;
        float distanceToDesiredPos = Vector3.Distance(aimCtrl.transform.parent.position, desiredCamPos);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPos, minCameraDistance, actualMaxCameraDistance);

        desiredCamPos = aimCtrl.transform.parent.position + aimDirection * clampedDistance;
        desiredCamPos.y = aimCtrl.transform.parent.position.y + 1;
        return desiredCamPos;
    }
}
