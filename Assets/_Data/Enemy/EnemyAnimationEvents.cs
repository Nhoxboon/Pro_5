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
        if (enemy != null) return;
        enemy = GetComponentInParent<Enemy>();
        Debug.Log(transform.name + " LoadEnemy", gameObject);
    }

    public void AnimationTrigger() => enemy.AnimationTrigger();

    public void StartManualMovement() => enemy.EnemyMove.ActivateManualMovement(true);
    public void StopManualMovement() => enemy.EnemyMove.ActivateManualMovement(false);

    public void StartManualRotation() => enemy.EnemyMove.ActivateManualRotation(true);
    public void StopManualRotation() => enemy.EnemyMove.ActivateManualRotation(false);

    public void AbilityEvent() => enemy.AbilityTrigger();

    public void EnableWeaponModel()
    {
        if (enemy.EnemyVisuals is not ERangeVisuals enemyVisuals) return;
        enemyVisuals.EnableWeaponModel(true);
        enemyVisuals.EnableSecondaryWeaponModel(false);
    }
}