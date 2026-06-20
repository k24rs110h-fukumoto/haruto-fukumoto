using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleDeleteSavePanelController : MonoBehaviour
{
    [SerializeField] private GameObject deleteSavePanel;
    [SerializeField] private TextMeshProUGUI[] slotTexts;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button backButton;

    private int currentIndex;

    private void Start()
    {
        deleteSavePanel.SetActive(false);

        deleteButton.onClick.AddListener(DeleteCurrentSlot);
        backButton.onClick.AddListener(ClosePanel);

        currentIndex = 0;
        RefreshPanel();
    }

    private void Update()
    {
        if (!deleteSavePanel.activeSelf)
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
            DeleteCurrentSlot();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        deleteSavePanel.SetActive(true);
        currentIndex = 0;
        RefreshPanel();
    }

    private void ClosePanel()
    {
        deleteSavePanel.SetActive(false);
    }

    private void MoveUp()
    {
        if (currentIndex == 0)
        {
            currentIndex = slotTexts.Length - 1;
        }
        else
        {
            currentIndex--;
        }

        RefreshPanel();
    }

    private void MoveDown()
    {
        if (currentIndex == slotTexts.Length - 1)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
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

    private void DeleteCurrentSlot()
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

        SaveManager.Instance.DeleteSaveData(slot);
        Debug.Log("Slot " + slot + " を削除しました。");

        RefreshPanel();
    }
}