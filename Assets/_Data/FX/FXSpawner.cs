
using UnityEngine;

public class FXSpawner : Spawner
{
    private static FXSpawner instance;
    public static FXSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if(instance != null)
        {
            Debug.LogError("Only 1 instance of FXSpawner allow to exist");
            return;
        }
        instance = this;
    }
}
