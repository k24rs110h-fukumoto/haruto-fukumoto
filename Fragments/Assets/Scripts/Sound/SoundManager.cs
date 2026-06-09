using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;
    [SerializeField] private SoundDatabase soundDatabase;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(BGMType type)
    {
        AudioClip clip = GetBGM(type);

        if (clip == null)
        {
            return;
        }

        if (bgmSource.clip == clip && bgmSource.isPlaying)
        {
            return;
        }
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public AudioClip GetBGM(BGMType type)
    {
        foreach (BGMData data in soundDatabase.bgmList)
        {
            if (data.type == type)
            {
                return data.clip;
            }
        }

        Debug.LogWarning(type + " is not found");
        return null;
    }

    public void PlaySE(SEType type)
    {
        AudioClip clip = GetSE(type);

        if (clip == null)
        {
            return;
        }

        seSource.PlayOneShot(clip);
    }

    public AudioClip GetSE(SEType type)
    {
        foreach (SEData data in soundDatabase.seList)
        {
            if (data.type == type)
            {
                return data.clip;
            }
        }

        Debug.LogWarning(type + " is not found");
        return null;
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void StopSE()
    {
        seSource.Stop();
    }
}