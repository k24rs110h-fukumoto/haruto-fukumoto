using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip titleBGM;
    [SerializeField] private AudioClip stageBGM;
    [SerializeField] private AudioClip endingBGM;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ApplyVolume();
    }

    private void Update()
    {
        ApplyVolume();
    }

    public void ApplyVolume()
    {
        if (SettingsManager.Instance == null)
        {
            return;
        }

        float master = SettingsManager.Instance.MasterVolume;

        if (bgmSource != null)
        {
            bgmSource.volume = master * SettingsManager.Instance.BGMVolume;
        }

        if (seSource != null)
        {
            seSource.volume = master * SettingsManager.Instance.SEVolume;
        }
    }

    public void PlayTitleBGM()
    {
        PlayBGM(titleBGM);
    }

    public void PlayStageBGM()
    {
        PlayBGM(stageBGM);
    }

    public void PlayEndingBGM()
    {
        PlayBGM(endingBGM);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource == null || clip == null)
        {
            return;
        }

        if (bgmSource.clip == clip && bgmSource.isPlaying)
        {
            return;
        }

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

    public void PlaySE(AudioClip clip)
    {
        if (seSource == null || clip == null)
        {
            return;
        }

        seSource.PlayOneShot(clip);
    }
}