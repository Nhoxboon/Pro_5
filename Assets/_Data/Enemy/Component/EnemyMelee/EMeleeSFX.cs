using UnityEngine;

public class EMeleeSFX : NhoxBehaviour
{
    [SerializeField] protected AudioSource meleeAttack;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMeleeAttackSFX();
    }

    private void LoadMeleeAttackSFX()
    {
        if(meleeAttack != null) return;
        meleeAttack = GetComponentInChildren<AudioSource>();
        Debug.Log(transform.name + " LoadMeleeAttackSFX", gameObject);
    }

    public void PlayMeleeAttackSFX() => AudioManager.Instance.PlaySFX(meleeAttack, true);
}
