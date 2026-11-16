using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : NhoxSingleton<AudioManager>
{
    [Header("BGM Settings")]
    [SerializeField] protected bool playBgm;
    [SerializeField] protected AudioSource bgmSource;
    [SerializeField] protected AudioClip[] bgmClips;
    protected int bgmIndex;

    [Header("UI SFX Settings")]
    [SerializeField] protected AudioSource uiSfxSource;
    [SerializeField] protected SerializableDictionary<UISfx, AudioClip> uiSfxClips;

    protected override void Start()
    {
        base.Start();
        PlayBGM(3);
    }

    protected void Update()
    {
        if (!playBgm && bgmSource.isPlaying)
            StopAllBGM();
        else if (!bgmSource.isPlaying)
            PlayRandomBGM();
    }

    public void PlaySFX(AudioSource sfx, bool randomPitch = false)
    {
        if (!sfx.clip) return;
        if (sfx.isPlaying) sfx.Stop();

        sfx.pitch = randomPitch ? Random.Range(0.9f, 1.1f) : 1f;
        sfx.Play();
    }

    public void PlayUISfx(UISfx sfx, bool randomPitch = false)
    {
        if (!uiSfxClips.TryGetValue(sfx, out AudioClip clip)) return;
        uiSfxSource.clip = clip;
        PlaySFX(uiSfxSource, randomPitch);
    }

    public void PlayBGM(int index)
    {
        if (bgmIndex == index && bgmSource.isPlaying) return;
        bgmIndex = index;
        bgmSource.clip = bgmClips[index];
        bgmSource.Play();
    }

    public void StopAllBGM() => bgmSource.Stop();

    public void PlayRandomBGM()
    {
        int randomIndex = Random.Range(0, bgmClips.Length);
        PlayBGM(randomIndex);
    }
}