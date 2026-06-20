using UnityEngine;
using UnityEngine.Video;

public class TitleVideoBackground : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private bool playOnStart = true;

    private void Start()
    {
        if (videoPlayer == null)
        {
            return;
        }

        videoPlayer.isLooping = true;

        if (playOnStart)
        {
            videoPlayer.Play();
        }
    }
}