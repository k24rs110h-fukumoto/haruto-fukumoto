using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private float showTime = 3f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        Instance = this;

        AutoFindUI();

        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }
    }

    public void Show(string message)
    {
        if (notificationPanel == null || notificationText == null)
        {
            AutoFindUI();
        }

        if (notificationPanel == null || notificationText == null)
        {
            Debug.LogError("Notification UI が見つかりません。");
            return;
        }

        notificationText.text = message;
        notificationPanel.SetActive(true);

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideAfterTime());
    }

    private IEnumerator HideAfterTime()
    {
        yield return new WaitForSeconds(showTime);

        notificationPanel.SetActive(false);
        hideCoroutine = null;
    }

    private void AutoFindUI()
    {
        Transform panelTransform = transform.Find("NotificationPanel");

        if (panelTransform != null)
        {
            notificationPanel = panelTransform.gameObject;
        }

        Transform textTransform = transform.Find("NotificationPanel/NotificationText");

        if (textTransform != null)
        {
            notificationText = textTransform.GetComponent<TextMeshProUGUI>();
        }
    }
}