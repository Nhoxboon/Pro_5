using System;
using System.Linq;
using UnityEngine;

public class CarInteraction : Interactable
{
    [SerializeField] protected CarController car;
    protected Transform player;

    protected float defaultPlayerScale;

    [Header("Exit Details")]
    [SerializeField] protected float exitCheckRadius = 0.2f;
    [SerializeField] protected Transform[] exitPoints;
    [SerializeField] protected LayerMask whatToIgnoreForExit;

    protected override void Start()
    {
        base.Start();
        player = PlayerCtrl.Instance.transform;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMeshRenderer();
        LoadCarController();
        LoadExitPoints();
        LoadLayerIgnore();
    }

    protected void LoadMeshRenderer()
    {
        if(mr != null) return;
        mr = transform.parent.Find("Model/Body").GetComponent<MeshRenderer>();
        Debug.Log(mr.name + ": LoadMeshRenderer", gameObject);
    }

    protected void LoadCarController()
    {
        if (car != null) return;
        car = GetComponentInParent<CarController>();
        Debug.Log(transform.name + ": LoadCarController", gameObject);
    }

    protected void LoadExitPoints()
    {
        if(exitPoints is { Length: > 0 }) return;
        var allPoints = transform.Find("ExitPointHolder").GetComponentsInChildren<Transform>();
        exitPoints = allPoints.Where(p => p.name != "ExitPointHolder").ToArray();
        Debug.Log(transform.name + ": LoadExitPoints", gameObject);
    }

    protected void LoadLayerIgnore()
    {
        if(whatToIgnoreForExit != 0) return;
        whatToIgnoreForExit = LayerMask.GetMask("Default", "Ground", "Projectile", "Aim", "Enemy");
        Debug.Log(transform.name + ": LoadLayerIgnore", gameObject);
    }

    public override void Interaction()
    {
        base.Interaction();
        GetIntoTheCar();
    }

    protected void GetIntoTheCar()
    {
        ControlsManager.Instance.SwitchToCarControls();
        car.ActivateCar(true);

        defaultPlayerScale = player.localScale.x;
        player.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        player.transform.parent = car.transform;
        player.transform.localPosition = Vector3.up / 2;

        CameraManager.Instance.ChangeCameraTarget(car.transform);
    }

    public void GetOutOfTheCar()
    {
        if(!car.CarActive) return;
        car.ActivateCar(false);
        player.parent = null;
        player.position = GetExitPoint();
        player.localScale = new Vector3(defaultPlayerScale, defaultPlayerScale, defaultPlayerScale);

        ControlsManager.Instance.SwitchToCharacterControls();
        CameraManager.Instance.ChangeCameraTarget(PlayerCtrl.Instance.PlayerAim.CamFollow.GetAimCameraTarget());
    }

    protected Vector3 GetExitPoint()
    {
        for (int i = 0; i < exitPoints.Length; i++)
        {
            if (IsExitClear(exitPoints[i].position))
                return exitPoints[i].position;
        }
        return exitPoints[0].position;
    }

    protected bool IsExitClear(Vector3 point)
    {
        Collider[] cols = Physics.OverlapSphere(point, exitCheckRadius, ~whatToIgnoreForExit);
        return cols.Length == 0;
    }

    private void OnDrawGizmos()
    {
        if (exitPoints.Length <= 0) return;
        foreach (var exitPoint in exitPoints)
        {
            Gizmos.DrawWireSphere(exitPoint.position, exitCheckRadius);
        }
    }
}
