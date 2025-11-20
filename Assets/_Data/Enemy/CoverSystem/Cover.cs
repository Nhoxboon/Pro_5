
using System.Collections.Generic;
using UnityEngine;

public class Cover : NhoxBehaviour
{
    [Header("Cover Points")] 
    [SerializeField] protected GameObject coverPointPrefabs;
    [SerializeField] protected List<CoverPoint> coverPoints = new();
    public List<CoverPoint> CoverPoints => coverPoints;
    
    [SerializeField] protected float xOffset = 1f;
    [SerializeField] protected float yOffset = 0.2f;
    [SerializeField] protected float zOffset = 1f;
    
    [SerializeField] protected Transform coverPointHolder;

    protected override void Start()
    {
        base.Start();
        GenerateCoverPoints();
    }
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCoverHolder();
    }
    
    protected void LoadCoverHolder()
    {
        if (coverPointHolder != null) return;
        coverPointHolder = GameObject.Find("CoverPointHolder").transform;
        Debug.Log(transform.name + " LoadCoverHolder", gameObject);
    }

    protected void GenerateCoverPoints()
    {
        Vector3[] localCoverPoints = {
            new Vector3(0, yOffset, zOffset), // Front
            new Vector3(0, yOffset, -zOffset), // Back
            new Vector3(xOffset, yOffset, 0), // Right
            new Vector3(-xOffset, yOffset, 0), // Left
        };

        foreach (Vector3 localPoint in localCoverPoints)
        {
            Vector3 worldPoint = transform.TransformPoint(localPoint);
            CoverPoint coverPoint =
                Instantiate(coverPointPrefabs, worldPoint, Quaternion.identity, coverPointHolder)
                    .GetComponent<CoverPoint>(); //Instantiate only 1 time when started, then no need to use object pooling
            
            coverPoints.Add(coverPoint);
        }
    }
}
