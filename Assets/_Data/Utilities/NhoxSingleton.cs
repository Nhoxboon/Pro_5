
using UnityEngine;

public class NhoxSingleton<T> : NhoxBehaviour where T : NhoxBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance is null)
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                Debug.LogError($"Instance of {typeof(T).Name} is null");
            }
            return _instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
        if (_instance != null && _instance != this)
        {
            Debug.LogError($"Only one instance of {typeof(T).Name} is allowed to exist");
            return;
        }

        _instance = this as T;
    }
}
