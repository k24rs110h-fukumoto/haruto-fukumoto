using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource seSource;
    public AudioSource bgmSource;

    public AudioClip startSE;
    public AudioClip flashSE;
    public AudioClip cutInSE;
    public AudioClip blackoutSE;
    public AudioClip sevenChanceSE;
    public AudioClip pushSE;
    public AudioClip reelStopSE;
    public AudioClip summonSE;

    public AudioClip slotBGM;
    public AudioClip battleBGM;
    public AudioClip resultBGM;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySE(AudioClip clip)
    {
        if (clip == null || seSource == null) return;
        seSource.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource == null) return;

        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }
}