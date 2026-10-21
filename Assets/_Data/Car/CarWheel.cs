using UnityEngine;

public enum AxelType
{
    Front,
    Back,
}

[RequireComponent(typeof(WheelCollider))]
public class CarWheel : NhoxBehaviour
{
    public AxelType axelType;
    [SerializeField] protected WheelCollider col;
    public WheelCollider Col => col;
    [SerializeField] protected MeshRenderer model;
    public MeshRenderer Model => model;

    protected float defaultSideStiffness;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCollider();
        LoadModel();
    }

    protected void LoadCollider()
    {
        if (col != null) return;
        col = GetComponent<WheelCollider>();
        Debug.Log(transform.name + " LoadCollider", gameObject);
    }

    protected void LoadModel()
    {
        if (model != null) return;
        model = GetComponentInChildren<MeshRenderer>();
        Debug.Log(transform.name + " LoadModel", gameObject);
    }

    public void SetDefaultStiffness(float value)
    {
        defaultSideStiffness = value;
        RestoreDefaultStiffness();
    }

    public void RestoreDefaultStiffness()
    {
        WheelFrictionCurve sidewaysFriction = col.sidewaysFriction;

        sidewaysFriction.stiffness = defaultSideStiffness;
        col.sidewaysFriction = sidewaysFriction;
    }
}