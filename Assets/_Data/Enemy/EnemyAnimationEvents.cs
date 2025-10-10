
using UnityEngine;

public class EnemyAnimationEvents : NhoxBehaviour
{
    [SerializeField] protected Enemy enemy;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemy();
    }

    protected void LoadEnemy()
    {
        if(enemy != null) return;
        enemy = GetComponentInParent<Enemy>();
        Debug.Log(transform.name + " LoadEnemy", gameObject);
    }
    
    public void AnimationTrigger() => enemy.AnimationTrigger();
}
