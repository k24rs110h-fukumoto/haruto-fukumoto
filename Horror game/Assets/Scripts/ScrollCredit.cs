using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollCredit : MonoBehaviour
{
    [SerializeField] private RectTransform creditTransform;
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float endY = 4000f;
    [SerializeField] private string titleSceneName = "Title";
    [SerializeField] private float waitTime = 3f;

    private bool isEnd;

    private void Update()
    {
        if (isEnd)
        {
            return;
        }

        creditTransform.anchoredPosition +=
            Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditTransform.anchoredPosition.y >= endY)
        {
            isEnd = true;
            Invoke(nameof(ReturnTitle), waitTime);
        }
    }

    private void ReturnTitle()
    {
        GameStateManager.SetState(
            GameStateManager.GameState.Title);

        SceneManager.LoadScene(titleSceneName);
    }
}