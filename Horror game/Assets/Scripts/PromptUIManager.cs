using TMPro;
using UnityEngine;

public class PromptUIManager : MonoBehaviour
{
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TextMeshProUGUI promptText;

    private void Start()
    {
        promptPanel.SetActive(false);
    }

    public void Show(string prompt)
    {
        promptText.text = prompt;
        promptPanel.SetActive(true);
    }

    public void Hide()
    {
        promptPanel.SetActive(false);
    }

}