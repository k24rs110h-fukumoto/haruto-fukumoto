using TMPro;
using UnityEngine;

public class SavePanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] saveSlotTexts;

    private int currentIndex;
    private bool isSelecting;

    private void Start()
    {
        RefreshSavePanel();
    }

    private void Update()
    {
        if (!isSelecting)
        {
            return;
        }

        if (InputDelayManager.IsLocked())
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
            SaveCurrentSlot();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            ExitSelectionMode();
        }
    }

    public void EnterSelectionMode()
    {
        isSelecting = true;
        currentIndex = 0;
        RefreshSavePanel();
        InputDelayManager.Lock(0.15f);
    }

    public void ExitSelectionMode()
    {
        isSelecting = false;
        InputDelayManager.Lock(0.15f);

        if (GameMenuManager.Instance != null)
        {
            GameMenuManager.Instance.ReturnToMenuBar();
        }
    }

    public void RefreshSavePanel()
    {
        if (saveSlotTexts == null || saveSlotTexts.Length == 0)
        {
            Debug.LogError("Save Slot Texts が設定されていません。");
            return;
        }

        for (int i = 0; i < saveSlotTexts.Length; i++)
        {
            if (saveSlotTexts[i] == null)
            {
                Debug.LogError("Save Slot Texts の Element " + i + " が None です。");
                continue;
            }

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

            if (isSelecting && currentIndex == i)
            {
                text = "＞ " + text;
            }

            saveSlotTexts[i].text = text;
        }
    }

    private void MoveUp()
    {
        if (saveSlotTexts == null || saveSlotTexts.Length == 0)
        {
            return;
        }

        if (currentIndex == 0)
        {
            currentIndex = saveSlotTexts.Length - 1;
        }
        else
        {
            currentIndex--;
        }

        RefreshSavePanel();
    }

    private void MoveDown()
    {
        if (saveSlotTexts == null || saveSlotTexts.Length == 0)
        {
            return;
        }

        if (currentIndex == saveSlotTexts.Length - 1)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }

        RefreshSavePanel();
    }

    private void SaveCurrentSlot()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager がありません。");
            return;
        }

        int slot = currentIndex + 1;
        SaveManager.Instance.SaveGame(slot);

        Debug.Log("Save Slot " + slot);

        RefreshSavePanel();
        InputDelayManager.Lock(0.2f);
    }
}