
using System;
using System.Collections;
using UnityEngine;

public class ZoneLimitation : NhoxBehaviour
{
    [SerializeField] protected ParticleSystem[] lines;
    [SerializeField] protected BoxCollider col;

    protected override void Start()
    {
        base.Start();
        ActivateWall(false);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLines();
        LoadCollider();
    }

    protected void LoadLines()
    {
        if(lines is { Length: > 0 }) return;
        lines = transform.GetComponentsInChildren<ParticleSystem>();
        Debug.Log(transform.name + " LoadLines", gameObject);
    }
    
    protected void LoadCollider()
    {
        if(col != null) return;
        col = GetComponent<BoxCollider>();
        Debug.Log(transform.name + " LoadCollider", gameObject);
    }

    protected void ActivateWall(bool active)
    {
        foreach (var line in lines)
        {
            if(active)
                line.Play();
            else
                line.Stop();
        }
        col.isTrigger = !active;
    }

    protected IEnumerator WallActivateCoroutine()
    {
        ActivateWall(true);
        yield return new WaitForSeconds(1);
        ActivateWall(false);
    }

    protected void OnTriggerEnter(Collider other)
    {
        StartCoroutine(WallActivateCoroutine());
    }
}
