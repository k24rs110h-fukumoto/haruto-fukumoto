using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleLoadSavePanelController : MonoBehaviour
{
    [SerializeField] private GameObject loadSavePanel;
    [SerializeField] private TextMeshProUGUI[] slotTexts;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button backButton;

    private int currentIndex;

    private void Start()
    {
        loadSavePanel.SetActive(false);

        loadButton.onClick.AddListener(LoadCurrentSlot);
        backButton.onClick.AddListener(ClosePanel);

        currentIndex = 0;
        RefreshPanel();
    }

    private void Update()
    {
        if (!loadSavePanel.activeSelf)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            MoveUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            MoveDown();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            LoadCurrentSlot();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        loadSavePanel.SetActive(true);
        currentIndex = 0;
        RefreshPanel();
    }

    private void ClosePanel()
    {
        loadSavePanel.SetActive(false);
    }

    private void MoveUp()
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = slotTexts.Length - 1;
        }

        RefreshPanel();
    }

    private void MoveDown()
    {
        currentIndex++;

        if (currentIndex >= slotTexts.Length)
        {
            currentIndex = 0;
        }

        RefreshPanel();
    }

    private void RefreshPanel()
    {
        for (int i = 0; i < slotTexts.Length; i++)
        {
            int slot = i + 1;
            string text = "Slot " + slot;

            if (SaveManager.Instance != null && SaveManager.Instance.HasSaveData(slot))
            {
                text += "  [DATA]";
            }
            else
            {
                text += "  [EMPTY]";
            }

            if (i == currentIndex)
            {
                text = "＞ " + text;
            }

            slotTexts[i].text = text;
        }
    }

    private void LoadCurrentSlot()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager がありません。");
            return;
        }

        int slot = currentIndex + 1;

        if (!SaveManager.Instance.HasSaveData(slot))
        {
            Debug.Log("Slot " + slot + " は空です。");
            return;
        }

        SaveManager.Instance.LoadGame(slot);
    }
}