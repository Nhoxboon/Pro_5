
using UnityEngine;

public class AmmoSpawner : Spawner
{
    private static AmmoSpawner instance;
    public static AmmoSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only 1 instance of AmmoSpawner allow to exist");
            return;
        }

        instance = this;
    }
}
