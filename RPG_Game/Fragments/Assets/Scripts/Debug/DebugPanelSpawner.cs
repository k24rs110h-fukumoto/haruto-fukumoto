using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugPanelSpawner : MonoBehaviour
{
    public static DebugPanelSpawner Instance;

    [Header("Debug Panel Prefab")]
    [SerializeField] private GameObject debugPanelPrefab;

    private GameObject currentDebugPanel;

    private TMP_InputField itemIDInput;
    private TMP_InputField itemCountInput;
    private TMP_InputField magicIDInput;
    private TMP_InputField magicUsesInput;
    private TMP_InputField goldInput;
    private TMP_InputField expInput;

    private TextMeshProUGUI playerInfoText;
    private TextMeshProUGUI debugMessageText;

    private Button addItemButton;
    private Button addMagicButton;
    private Button addGoldButton;
    private Button addExpButton;
    private Button recoverHpButton;
    private Button saveButton;
    private Button loadButton;
    private Button deleteSaveButton;

    private Button allItemButton;
    private Button allMagicButton;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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
        SpawnDebugPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleDebugPanel();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnDebugPanel();
    }

    private void SpawnDebugPanel()
    {
        if (debugPanelPrefab == null)
        {
            Debug.LogError("DebugPanelPrefab が設定されていません。");
            return;
        }

        if (currentDebugPanel != null)
        {
            Destroy(currentDebugPanel);
        }

        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Canvas がSceneにありません。");
            return;
        }

        currentDebugPanel = Instantiate(debugPanelPrefab, canvas.transform);
        currentDebugPanel.SetActive(false);

        CacheReferences();

        Debug.Log("DebugPanelを自動生成しました。");
    }

    private void CacheReferences()
    {
        itemIDInput = FindTMPInput("itemIDInput");
        itemCountInput = FindTMPInput("itemCountInput");
        magicIDInput = FindTMPInput("magicIDInput");
        magicUsesInput = FindTMPInput("magicUsesInput");
        goldInput = FindTMPInput("goldInput");
        expInput = FindTMPInput("expInput");

        playerInfoText = FindTMPText("playerInfoText");
        debugMessageText = FindTMPText("debugMessageText");

        addItemButton = FindButton("addItemButton");
        addMagicButton = FindButton("addMagicButton");
        addGoldButton = FindButton("addGoldButton");
        addExpButton = FindButton("addExpButton");
        recoverHpButton = FindButton("recoverHpButton");
        saveButton = FindButton("saveButton");
        loadButton = FindButton("loadButton");
        deleteSaveButton = FindButton("deleteSaveButton");

        allItemButton = FindButton("allItemButton");
        allMagicButton = FindButton("allMagicButton");

        RegisterButtons();

        SetMessage("");
    }

    private void RegisterButtons()
    {
        if (addItemButton != null)
            addItemButton.onClick.AddListener(AddDebugItem);

        if (addMagicButton != null)
            addMagicButton.onClick.AddListener(AddDebugMagic);

        if (addGoldButton != null)
            addGoldButton.onClick.AddListener(AddDebugGold);

        if (addExpButton != null)
            addExpButton.onClick.AddListener(AddDebugExp);

        if (recoverHpButton != null)
            recoverHpButton.onClick.AddListener(RecoverPlayerHp);

        if (saveButton != null)
            saveButton.onClick.AddListener(SaveDebug);

        if (loadButton != null)
            loadButton.onClick.AddListener(LoadDebug);

        if (allItemButton != null)
            allItemButton.onClick.AddListener(AddAllItems);

        if (allMagicButton != null)
            allMagicButton.onClick.AddListener(AddAllMagics);

        // if (deleteSaveButton != null)
        //     deleteSaveButton.onClick.AddListener(ClearSaveDebug);
    }

    private TMP_InputField FindTMPInput(string objectName)
    {
        Transform target = FindChildRecursive(currentDebugPanel.transform, objectName);

        if (target == null)
        {
            Debug.LogError(objectName + " がDebugPanel内に見つかりません。");
            return null;
        }

        return target.GetComponent<TMP_InputField>();
    }

    private TextMeshProUGUI FindTMPText(string objectName)
    {
        Transform target = FindChildRecursive(currentDebugPanel.transform, objectName);

        if (target == null)
        {
            Debug.LogError(objectName + " がDebugPanel内に見つかりません。");
            return null;
        }

        return target.GetComponent<TextMeshProUGUI>();
    }

    private Button FindButton(string objectName)
    {
        Transform target = FindChildRecursive(currentDebugPanel.transform, objectName);

        if (target == null)
        {
            Debug.LogError(objectName + " がDebugPanel内に見つかりません。");
            return null;
        }

        return target.GetComponent<Button>();
    }

    private Transform FindChildRecursive(Transform parent, string objectName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == objectName)
            {
                return child;
            }

            Transform result = FindChildRecursive(child, objectName);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private void ToggleDebugPanel()
    {
        if (currentDebugPanel == null)
        {
            SpawnDebugPanel();
        }

        if (currentDebugPanel == null) return;

        bool nextState = !currentDebugPanel.activeSelf;
        currentDebugPanel.SetActive(nextState);

        if (nextState)
        {
            UpdatePlayerInfo();
            SetMessage("Debug Panel Open");
        }
    }

    public void UpdatePlayerInfo()
    {
        if (playerInfoText == null) return;

        if (PlayerManager.Instance == null || PlayerManager.Instance.playerData == null)
        {
            playerInfoText.text = "PlayerData がありません";
            return;
        }

        PlayerData player = PlayerManager.Instance.playerData;

        playerInfoText.text =
            "Name : " + player.playerName + "\n" +
            "HP : " + player.currentHp + " / " + player.maxHp + "\n" +
            "Lv : " + player.level + "\n" +
            "Exp : " + player.currentExp + "\n" +
            "Gold : " + player.currentGold + "\n" +
            "Attack : " + player.attack + "\n" +
            "Defense : " + player.defense + "\n" +
            "Speed : " + player.speed + "\n" +
            "Scene : " + player.currentSceneName + "\n" +
            "Spawn : " + player.currentSpawnPointID;
    }

    public void AddDebugItem()
    {
        if (itemIDInput == null) return;

        string itemID = itemIDInput.text;

        int count = 1;
        if (itemCountInput != null)
        {
            int.TryParse(itemCountInput.text, out count);
        }

        if (count <= 0) count = 1;

        if (string.IsNullOrEmpty(itemID))
        {
            SetMessage("ItemID が空です");
            return;
        }

        ItemData itemData = ItemDatabase.Instance.GetItemData(itemID);

        if (itemData == null)
        {
            SetMessage("ItemData が見つかりません: " + itemID);
            return;
        }

        InventoryManager.Instance.AddItem(itemData, count);
        SetMessage("Item追加: " + itemData.itemName + " x" + count);
        UpdatePlayerInfo();
    }

    public void AddDebugMagic()
    {
        if (magicIDInput == null) return;

        string magicID = magicIDInput.text;

        int uses = 1;
        if (magicUsesInput != null)
        {
            int.TryParse(magicUsesInput.text, out uses);
        }

        if (uses <= 0) uses = 1;

        if (string.IsNullOrEmpty(magicID))
        {
            SetMessage("MagicID が空です");
            return;
        }

        MagicData magicData = MagicDatabase.Instance.GetMagicData(magicID);

        if (magicData == null)
        {
            SetMessage("MagicData が見つかりません: " + magicID);
            return;
        }

        MagicManager.Instance.AddMagic(magicData, uses);
        SetMessage("Magic追加: " + magicData.magicName + " +" + uses);
        UpdatePlayerInfo();
    }

    public void AddAllItems()
    {
        for (int i = 1; i <= 35; i++)
        {
            string id = "I" + i.ToString("000");

            ItemData itemData = ItemDatabase.Instance.GetItemData(id);

            if (itemData != null)
            {
                InventoryManager.Instance.AddItem(itemData, 99);
            }
        }

        SetMessage("全アイテム追加");
        UpdatePlayerInfo();
    }

    public void AddAllMagics()
    {
        string[] types =
        {
        "A",
        "D",
        "E",
        "F",
        "L",
        "N",
        "W"
    };

        foreach (string type in types)
        {
            for (int i = 1; i <= 5; i++)
            {
                string id = type + i.ToString("000");

                MagicData magicData = MagicDatabase.Instance.GetMagicData(id);

                if (magicData != null)
                {
                    MagicManager.Instance.AddMagic(magicData, 99);
                }
            }
        }

        SetMessage("全魔法追加");
        UpdatePlayerInfo();
    }

    private void AddItemByID(string itemID, int count)
    {
        ItemData itemData = ItemDatabase.Instance.GetItemData(itemID);

        if (itemData == null)
        {
            Debug.LogWarning("ItemData が見つかりません: " + itemID);
            return;
        }

        InventoryManager.Instance.AddItem(itemData, count);
    }

    private void AddMagicByID(string magicID, int uses)
    {
        MagicData magicData = MagicDatabase.Instance.GetMagicData(magicID);

        if (magicData == null)
        {
            Debug.LogWarning("MagicData が見つかりません: " + magicID);
            return;
        }

        MagicManager.Instance.AddMagic(magicData, uses);
    }

    public void AddDebugGold()
    {
        int gold = 0;

        if (goldInput != null)
        {
            int.TryParse(goldInput.text, out gold);
        }

        PlayerManager.Instance.AddGold(gold);
        SetMessage("Gold追加: " + gold);
        UpdatePlayerInfo();
    }

    public void AddDebugExp()
    {
        int exp = 0;

        if (expInput != null)
        {
            int.TryParse(expInput.text, out exp);
        }

        PlayerManager.Instance.AddExp(exp);
        SetMessage("Exp追加: " + exp);
        UpdatePlayerInfo();
    }

    public void RecoverPlayerHp()
    {
        if (PlayerManager.Instance == null || PlayerManager.Instance.playerData == null)
        {
            SetMessage("PlayerData がありません");
            return;
        }

        PlayerData player = PlayerManager.Instance.playerData;
        player.currentHp = player.maxHp;

        SetMessage("HP全回復");
        UpdatePlayerInfo();
    }

    public void SaveDebug()
    {
        SaveManager.Instance.SaveGame(0);
        SetMessage("セーブしました");
        UpdatePlayerInfo();
    }

    public void LoadDebug()
    {
        SaveManager.Instance.LoadGame(0);
        SetMessage("ロードしました");
        UpdatePlayerInfo();
    }

    // public void ClearSaveDebug()
    // {
    //     SaveSystem.Instance.DeleteSave();
    //     SetMessage("セーブデータ削除");
    //     UpdatePlayerInfo();
    // }

    private void SetMessage(string message)
    {
        if (debugMessageText == null) return;
        debugMessageText.text = message;
    }
}