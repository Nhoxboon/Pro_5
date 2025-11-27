
using UnityEngine;

public class EBossAbility : EnemyAbility
{
    [Header("Flame Thrower")] 
    [SerializeField] protected float flameThrowDuration = 3f;
    public float FlameThrowDuration => flameThrowDuration;
    [SerializeField] protected ParticleSystem flameParticle;
    public bool FlameThrowActive { get; protected set;}
    
    [Header("Hammer")]
    protected string hammerFXPrefab = "HammerFX";
    [SerializeField] protected Transform impactPoint;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadFlameParticles();
    }
    
    protected void LoadFlameParticles()
    {
        if (flameParticle != null) return;
        flameParticle = transform.GetComponentInChildren<ParticleSystem>();
        Debug.Log(transform.name + " LoadFlameParticles", gameObject);
    }
    
    public void ActivateFlameThrower(bool activate)
    {
        FlameThrowActive = activate;
        if (!activate)
        {
            flameParticle.Stop();
            enemy.EnemyAnimator.AnimationTrigger("stopFlameThrow");
            return;
        }
        var mainModule = flameParticle.main;
        mainModule.duration = flameThrowDuration;
        
        if (flameParticle.transform.GetChild(0).TryGetComponent(out ParticleSystem extraParticle))
        {
            var extraModule = extraParticle.main;
            extraModule.duration = flameThrowDuration;
        }
        flameParticle.Clear();
        flameParticle.Play();
    }
    
    public void ActivateHammer()
    {
        if(impactPoint == null) return;
        Transform newFX = FXSpawner.Instance.Spawn(hammerFXPrefab, impactPoint.position, Quaternion.identity);
        newFX.gameObject.SetActive(true);
    }
}
