using System.Collections.Generic;
using UnityEngine;

public class LevelPart : NhoxBehaviour
{
    [SerializeField] protected SnapPoint[] snapPoints;
    
    [Header("Intersection Check")] 
    [SerializeField] protected LayerMask intersectionLayer;
    [SerializeField] protected Transform intersectionCheckParent;
    [SerializeField] protected Collider[] intersectionCheckColliders;

    protected override void Awake()
    {
        base.Awake();
        LoadSnapPoints();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSnapPoints();
        LoadLayerMask();
        LoadIntersectionCheck();
    }

    protected void LoadSnapPoints()
    {
        if (snapPoints is { Length: > 0 }) return;
        snapPoints = GetComponentsInChildren<SnapPoint>();
        Debug.Log(transform.name + " LoadSnapPoints", gameObject);
    }

    protected void LoadLayerMask()
    {
        if (intersectionLayer.value != 0) return;
        intersectionLayer = LayerMask.GetMask("IntersectionCheck");
        Debug.Log(transform.name + " LoadLayerMask", gameObject);
    }
    
    protected void LoadIntersectionCheck()
    {
        if (intersectionCheckColliders.Length > 0) return;
        intersectionCheckParent = GetComponentInChildren<IntersectionCheck>().transform;
        intersectionCheckColliders = intersectionCheckParent.GetComponentsInChildren<Collider>();
        Debug.Log(transform.name + " LoadIntersectionCheckColliders", gameObject);
    }

    public bool IntersectionDetected()
    {
        Physics.SyncTransforms();

        foreach (var col in intersectionCheckColliders)
        {
            Collider[] hitCols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, Quaternion.identity,
                intersectionLayer);

            foreach (var hit in hitCols)
            {
                if (hit.transform.parent.TryGetComponent(out IntersectionCheck intersectionCheck) &&
                    intersectionCheckParent != intersectionCheck.transform)
                    return true;
            }
        }
        return false;
    }
    
    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint = GetEntrancePoint();

        AlignTo(entrancePoint, targetSnapPoint); //Warning: Alignment should be done before position snapping
        SnapTo(entrancePoint, targetSnapPoint);
    }

    protected void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        //Calculate the rotation offset between the level part's current rotation and its own snap point's rotation
        //This'll help fine-tuning the alignment later
        var rotationOffset = ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;

        //Set the level part's rotation to match the target snap point's rotation
        //This is the initial step to align the orientations of the two parts
        transform.rotation = targetSnapPoint.transform.rotation;

        //Rotate the level part by 180 degrees around the Y-axis
        //This is necessary because snap points are typically designed to face opposite directions
        //and this rotation aligns them to face each other correctly
        transform.Rotate(0, 180, 0);

        //Apply the previously calculated offset. This step fine-tunes the alignment by adjusting the level part's rotation
        //to account for any initial differences in orientations between the level part's own snap point and the main body of the part
        transform.Rotate(0, -rotationOffset, 0);
    }

    protected void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        //Calculate the offset between the level part's current position and its own snap point's position
        //This offset represents the distance and direction from the level part's pivot to its snap point
        var offset = transform.position - ownSnapPoint.transform.position;

        //Determine the new position for the level part. It's calculated by adding the previous computed offset to the target snap point's position.
        //This effectively moves the level part so its snap point aligns with the target snap point's position.
        var newPosition = targetSnapPoint.transform.position + offset;
        //Update the level part's position to the new calculated position by using snap points
        transform.position = newPosition;
    }

    public SnapPoint GetEntrancePoint() => GetSnapPointOfType(SnapPointType.Enter);

    public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.Exit);

    protected SnapPoint GetSnapPointOfType(SnapPointType pointType)
    {
        List<SnapPoint> filterSnapPoints = new List<SnapPoint>();

        foreach (SnapPoint snapPoint in snapPoints)
        {
            if (snapPoint.pointType == pointType)
                filterSnapPoints.Add(snapPoint);
        }

        if (filterSnapPoints.Count <= 0) return null;
        int randomIndex = Random.Range(0, filterSnapPoints.Count);
        return filterSnapPoints[randomIndex];
    }
}