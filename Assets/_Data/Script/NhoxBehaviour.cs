using UnityEngine;

public class NhoxBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        LoadComponents();
    }

    protected virtual void Start()
    {
        //For override
    }

    protected virtual void LoadComponents()
    {
        //For override
    }

    public virtual void Reset()
    {
        LoadComponents();
    }
}