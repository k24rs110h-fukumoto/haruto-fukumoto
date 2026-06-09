using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuPanelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject gameMenuPanelPrefab;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        SpawnGameMenuPanel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnGameMenuPanel();
    }

    private void SpawnGameMenuPanel()
    {
        if (GameStateManager.GetState() != GameStateManager.GameState.Field)
        {
            return;
        }

        if (FindFirstObjectByType<GameMenuManager>() != null)
        {
            return;
        }

        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("CanvasがSceneにありません。");
            return;
        }

        Instantiate(gameMenuPanelPrefab, canvas.transform);

        Debug.Log("GameMenuPanelを自動生成しました。");
    }
}