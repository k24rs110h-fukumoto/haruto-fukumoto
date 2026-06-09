using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;

    [SerializeField] private TextMeshProUGUI loadingText;

    public void Start()
    {
        StartCoroutine(LoadTargetScene());

    }

    private IEnumerator LoadTargetScene()
    {
        string nextSceneName = SceneTransitionManager.GetNextSceneName();

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("Next scene name is empty");
            yield break;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (loadingSlider != null)
            {
                loadingSlider.value = progress;
            }

            if (loadingText != null)
            {
                loadingText.text = "Loading... " + Mathf.RoundToInt(progress * 100f) + "%";
            }

            yield return null;
        }
    }
}
