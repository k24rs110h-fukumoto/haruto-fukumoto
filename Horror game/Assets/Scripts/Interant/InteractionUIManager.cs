using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class InteractionUIManager : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject imagePanel;
    [SerializeField] private Image image;
    public bool IsOpen { get; private set; }
    [SerializeField] private PlayerController playerController;


    private void Start()
    {
        Hide();
    }

    public void ShowMessage(string message)
    {
        imagePanel.SetActive(false);
        messagePanel.SetActive(true);
        messageText.text = message;
        IsOpen = true;
        playerController.canControl = false;
    }

    public void ShowImage(Sprite sprite)
    {
        messagePanel.SetActive(false);
        imagePanel.SetActive(true);
        image.sprite = sprite;
        IsOpen = true;
        playerController.canControl = false;
    }

    public void Hide()
    {
        messagePanel.SetActive(false);
        imagePanel.SetActive(false);
        IsOpen = false;
        playerController.canControl = true;
    }
}