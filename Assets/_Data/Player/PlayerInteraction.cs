using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : NhoxBehaviour
{
    protected List<Interactable> interactables = new();
    public List<Interactable> Interactables => interactables;

    protected Interactable closestInteractable;

    public void InteractWithClosest()
    {
        closestInteractable?.Interaction();
        interactables.Remove(closestInteractable);
        UpdateClosestInteractable();
    }

    public void UpdateClosestInteractable()
    {
        closestInteractable?.HighlightActive(false);

        closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.parent.position, interactable.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }

            closestInteractable?.HighlightActive(true);
        }
    }
}