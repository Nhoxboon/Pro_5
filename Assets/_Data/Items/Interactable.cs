using System;
using UnityEngine;

public class Interactable : NhoxBehaviour
{
    [SerializeField] protected MeshRenderer mr;
    [SerializeField] protected Material highlightMaterial;
    [SerializeField] protected Material defaultMaterial;
    
    protected PlayerInventory inventory;

    protected override void Start()
    {
        base.Start();
        if (mr == null)
            mr = GetComponentInChildren<MeshRenderer>();
        defaultMaterial = mr.material;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHighLightMat();
    }
    
    protected void LoadHighLightMat()
    {
        if (highlightMaterial != null) return;
        highlightMaterial = Resources.Load<Material>("Material/InteractMat");
        Debug.Log(transform.name + " LoadHighLightMat", gameObject);
    }

    protected void UpdateMeshAndMaterial(MeshRenderer newMesh)
    {
        mr = newMesh;
        defaultMaterial = newMesh.sharedMaterial;
    }

    public virtual void Interaction()
    {
        //For override
    }

    public void HighlightActive(bool active)
    {
        mr.material = active ? highlightMaterial : defaultMaterial;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponentInChildren(out PlayerInventory inv) && inventory == null)
            inventory = inv;
        
        if (!other.TryGetComponentInChildren(out PlayerInteraction interaction)) return;
        interaction.Interactables.Add(this);
        interaction.UpdateClosestInteractable();
        
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponentInChildren(out PlayerInteraction interaction)) return;
        interaction.Interactables.Remove(this);
        interaction.UpdateClosestInteractable();
    }
}