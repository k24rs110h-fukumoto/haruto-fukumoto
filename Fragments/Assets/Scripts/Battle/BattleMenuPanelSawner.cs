using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMenuPanelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject battleMenuPanelPrefab;

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
        SpawnBattleMenuPanel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnBattleMenuPanel();
    }

    public void SpawnBattleMenuPanel()
    {
        if (GameStateManager.GetState() != GameStateManager.GameState.Battle)
        {
            return;
        }

        if (FindFirstObjectByType<BattleMenuManager>() != null)
        {
            return;
        }

        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("CanvasがSceneにありません。");
            return;
        }

        Instantiate(battleMenuPanelPrefab, canvas.transform);

        Debug.Log("BattleMenuPanelを自動生成しました。");
    }
}