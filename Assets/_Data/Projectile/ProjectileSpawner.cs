using UnityEngine;

public class ProjectileSpawner : Spawner
{
    private static ProjectileSpawner instance;
    public static ProjectileSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only 1 instance of ProjectileSpawner allow to exist");
            return;
        }

        instance = this;
    }
}