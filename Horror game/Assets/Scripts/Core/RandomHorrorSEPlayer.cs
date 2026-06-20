using UnityEngine;

public class RandomHorrorSEPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] randomClips;
    [SerializeField] private float minInterval = 8f;
    [SerializeField] private float maxInterval = 25f;

    private float timer;
    private float nextPlayTime;

    private void Start()
    {
        SetNextPlayTime();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextPlayTime)
        {
            PlayRandomSE();
            timer = 0f;
            SetNextPlayTime();
        }
    }

    private void SetNextPlayTime()
    {
        nextPlayTime = Random.Range(minInterval, maxInterval);
    }

    private void PlayRandomSE()
    {
        if (randomClips == null || randomClips.Length == 0)
        {
            return;
        }

        AudioClip clip = randomClips[Random.Range(0, randomClips.Length)];

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(clip);
        }
    }
}