using System;
using UnityEngine;

public class SnapPoint : NhoxBehaviour
{
    public SnapPointType pointType;
    [SerializeField] protected Collider col;
    [SerializeField] protected MeshRenderer mr;

    protected override void Start()
    {
        base.Start();
        col.enabled = false;
        mr.enabled = false;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCollider();
        LoadMesh();
    }

    protected void LoadCollider()
    {
        if(col != null) return;
        col = GetComponent<Collider>();
        Debug.Log(transform.name + " LoadCollider", gameObject);
    }
    
    protected void LoadMesh()
    {
        if(mr != null) return;
        mr = GetComponent<MeshRenderer>();
        Debug.Log(transform.name + " LoadMesh", gameObject);
    }

    protected void OnValidate()
    {
        gameObject.name = "SnapPoint " + pointType.ToString();
    }
}