using UnityEngine;

public class NotificationPanelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject notificationRootPrefab;

    private void Start()
    {
        SpawnNotificationRoot();
    }

    private void SpawnNotificationRoot()
    {
        if (FindFirstObjectByType<NotificationManager>() != null)
        {
            return;
        }

        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("CanvasがSceneにありません。");
            return;
        }

        Instantiate(notificationRootPrefab, canvas.transform);

        Debug.Log("NotificationRootを自動生成しました。");
    }
}