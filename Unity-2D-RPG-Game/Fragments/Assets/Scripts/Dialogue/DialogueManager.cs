using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text dialogueText;

    private int currentLineIndex;
    private DialogueData currentDialogueData;
    private string currentNpcName;
    private bool isTalking;
    private System.Action onDialogueFinished;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        AutoFindUI();

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (dialoguePanel == null)
        {
            AutoFindUI();
        }

        if (!isTalking)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.E))
        {
            NextDialogue();
        }
    }

    public void ShowDialogue(string npcName, DialogueData dialogueData, System.Action onFinished = null)
    {
        if (dialoguePanel == null)
        {
            AutoFindUI();
        }

        if (dialoguePanel == null)
        {
            Debug.LogError("DialoguePanel が見つかりません。DialoguePanelPrefabが生成されているか確認してください。");
            return;
        }

        if (dialogueData == null || dialogueData.dialogueLines == null || dialogueData.dialogueLines.Length == 0)
        {
            Debug.LogWarning("DialogueData is empty");
            return;
        }

        currentLineIndex = 0;
        currentDialogueData = dialogueData;
        currentNpcName = npcName;
        isTalking = true;
        onDialogueFinished = onFinished;

        dialoguePanel.SetActive(true);

        if (nameText != null)
        {
            nameText.text = currentNpcName;
        }

        if (dialogueText != null)
        {
            dialogueText.text = currentDialogueData.dialogueLines[currentLineIndex];
        }
    }

    public void ShowDialogue(string npcName, Sprite image, DialogueData dialogueData, System.Action onFinished = null)
    {
        ShowDialogue(npcName, dialogueData, onFinished);

        if (characterImage != null)
        {
            characterImage.sprite = image;
            characterImage.gameObject.SetActive(image != null);
        }
    }

    public void HideDialogue()
    {
        if (onDialogueFinished != null)
        {
            onDialogueFinished.Invoke();
            onDialogueFinished = null;
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        isTalking = false;
        currentLineIndex = 0;
        currentDialogueData = null;
        currentNpcName = "";
    }

    public void NextDialogue()
    {
        if (currentDialogueData == null || currentDialogueData.dialogueLines == null)
        {
            HideDialogue();
            return;
        }

        currentLineIndex++;

        if (currentLineIndex >= currentDialogueData.dialogueLines.Length)
        {
            HideDialogue();
            return;
        }

        if (dialogueText != null)
        {
            dialogueText.text = currentDialogueData.dialogueLines[currentLineIndex];
        }
    }

    private void AutoFindUI()
    {
        DialoguePanelTag panelTag =
            FindFirstObjectByType<DialoguePanelTag>(FindObjectsInactive.Include);

        if (panelTag == null)
        {
            Debug.LogError("DialoguePanelTag が見つかりません。PrefabのRootに付いているか確認してください。");
            return;
        }

        dialoguePanel = panelTag.gameObject;

        Transform nameTextTransform = dialoguePanel.transform.Find("NameText");
        if (nameTextTransform != null)
        {
            nameText = nameTextTransform.GetComponent<TMP_Text>();
        }

        Transform dialogueTextTransform = dialoguePanel.transform.Find("DialogueText");
        if (dialogueTextTransform != null)
        {
            dialogueText = dialogueTextTransform.GetComponent<TMP_Text>();
        }

        Transform characterImageTransform = dialoguePanel.transform.Find("CharacterImage");
        if (characterImageTransform != null)
        {
            characterImage = characterImageTransform.GetComponent<UnityEngine.UI.Image>();
        }

        dialoguePanel.SetActive(false);
    }
}