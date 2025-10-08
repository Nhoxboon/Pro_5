using UnityEngine;

public class WeaponSpawner : Spawner
{
    private static WeaponSpawner instance;
    public static WeaponSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only one instance of WeaponSpawner allow to exist");
            return;
        }

        instance = this;
    }
}