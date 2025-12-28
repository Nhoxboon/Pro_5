
using UnityEngine;

public class Ragdoll : CoreComponent
{
    [SerializeField] protected Transform ragdollParent;
    [SerializeField] protected Collider[] ragdollCols;
    [SerializeField] protected Rigidbody[] ragdollRbs;

    protected override void Awake()
    {
        base.Awake();
        RagdollActive(false);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRagdollParent();
        LoadRagdollColliders();
        LoadRagdollRigidbodies();
    }

    protected void LoadRagdollParent()
    {
        if(ragdollParent != null) return;
        ragdollParent = core.Root.transform.Find("Model");
        Debug.Log(transform.name + " LoadRagdollParent", gameObject);
    }

    protected void LoadRagdollColliders()
    {
        if(ragdollCols is { Length: > 0 }) return;
        ragdollCols = core.Root.GetComponentsInChildren<Collider>();
        Debug.Log(transform.name + " LoadRagdollColliders", gameObject);
    }
    
    protected void LoadRagdollRigidbodies()
    {
        if(ragdollRbs is { Length: > 0 }) return;
        ragdollRbs = core.Root.GetComponentsInChildren<Rigidbody>();
        Debug.Log(transform.name + " LoadRagdollRigidbodies", gameObject);
    }
    
    public void RagdollActive(bool value)
    {
        foreach (var rb in ragdollRbs) 
            rb.isKinematic = !value;
    }

    public void ColliderActive(bool value)
    {
        foreach (var col in ragdollCols)
            col.enabled = value;
    }

    public void ResetRagdoll()
    {
        RagdollActive(false);
        ColliderActive(true);
    }
}
