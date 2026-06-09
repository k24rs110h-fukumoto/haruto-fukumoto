using UnityEngine;
using UnityEngine.SceneManagement;

public class DialoguePanelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanelPrefab;

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
        SpawnDialoguePanel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnDialoguePanel();
    }

    private void SpawnDialoguePanel()
    {
        if (FindFirstObjectByType<DialoguePanelTag>() != null)
        {
            return;
        }

        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogWarning("CanvasがSceneにありません。");
            return;
        }

        Instantiate(dialoguePanelPrefab, canvas.transform);
    }
}