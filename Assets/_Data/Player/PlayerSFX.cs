using UnityEngine;

public class PlayerSFX : NhoxBehaviour
{
    [SerializeField] protected AudioSource weaponReady;

    [SerializeField] protected AudioSource footsteps;
    [SerializeField] protected AudioClip walkSfx;
    [SerializeField] protected AudioClip runSfx;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponReadySFX();
        LoadFootstepsSFX();
    }

    protected void LoadWeaponReadySFX()
    {
        if (weaponReady != null) return;
        weaponReady = transform.Find("WeaponReady").GetComponentInChildren<AudioSource>();
        Debug.Log(transform.name + " LoadWeaponReadySFX", gameObject);
    }

    protected void LoadFootstepsSFX()
    {
        if (footsteps != null) return;
        footsteps = transform.Find("Footsteps").GetComponentInChildren<AudioSource>();
        Debug.Log(transform.name + " LoadFootstepsSFX", gameObject);
    }

    protected void PlayFootstepsSFX()
    {
        if (!footsteps.isPlaying)
            AudioManager.Instance.PlaySFX(footsteps, true);
    }

    public void PlayWalkSFX()
    {
        if(footsteps.clip != walkSfx)
            footsteps.clip = walkSfx;
        PlayFootstepsSFX();
    }

    public void PlayRunSFX()
    {
        if(footsteps.clip != runSfx)
            footsteps.clip = runSfx;
        PlayFootstepsSFX();
    }

    public void StopFootstepsSFX() => footsteps.Stop();

    public void PlayWeaponReadySFX() => AudioManager.Instance.PlaySFX(weaponReady);
}
