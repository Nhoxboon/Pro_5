
using UnityEngine;

public class GameManager : NhoxBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    
    public bool friendlyFire;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.Log("Only 1 instance of GameManager allow to exist");
            return;
        }
        instance = this;
    }
}
