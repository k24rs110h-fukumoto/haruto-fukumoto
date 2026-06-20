using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private SaveData currentSaveData;
    private const string SaveVersion = "1.0.0";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame(int slotID)
    {
        currentSaveData = CreateSaveData(slotID);
        SaveFile(slotID);
    }

    public void LoadGame(int slotID)
    {
        string path = GetSavePath(slotID);

        if (!File.Exists(path))
        {
            Debug.LogWarning("セーブデータが存在しません: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        currentSaveData = JsonUtility.FromJson<SaveData>(json);

        ApplySaveData();
        LoadSavedScene();

        Debug.Log("ロード完了: Slot " + slotID);
    }

    public bool HasSaveData(int slotID)
    {
        return File.Exists(GetSavePath(slotID));
    }

    public void DeleteSaveData(int slotID)
    {
        string path = GetSavePath(slotID);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("セーブデータ削除: " + path);
        }
    }

    public SaveData GetCurrentSaveData()
    {
        return currentSaveData;
    }

    private SaveData CreateSaveData(int slotID)
    {
        SaveData saveData = new SaveData();

        saveData.slotID = slotID;
        saveData.version = SaveVersion;
        saveData.savedAt = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        saveData.playTime = Time.time;

        saveData.playerSaveData = CreatePlayerSaveData();
        saveData.sceneSaveData = CreateSceneSaveData();
        saveData.inventorySaveData = CreateInventorySaveData();
        saveData.magicSaveDataList = CreateMagicSaveDataList();
        saveData.battleHistorySaveData = CreateBattleHistorySaveData();

        if (saveData.playerSaveData != null)
        {
            saveData.saveName = saveData.playerSaveData.playerName;
        }
        else
        {
            saveData.saveName = "No Name";
        }

        return saveData;
    }

    private PlayerSaveData CreatePlayerSaveData()
    {
        PlayerSaveData playerSaveData = new PlayerSaveData();

        if (PlayerManager.Instance == null || PlayerManager.Instance.playerData == null)
        {
            Debug.LogWarning("PlayerManager または playerData がありません。空のPlayerSaveDataを作成します。");
            return playerSaveData;
        }

        PlayerData playerData = PlayerManager.Instance.playerData;

        playerSaveData.playerName = playerData.playerName;
        playerSaveData.maxHp = playerData.maxHp;
        playerSaveData.currentHp = playerData.currentHp;
        playerSaveData.level = playerData.level;
        playerSaveData.currentExp = playerData.currentExp;
        playerSaveData.currentGold = playerData.currentGold;
        playerSaveData.attack = playerData.attack;
        playerSaveData.defense = playerData.defense;
        playerSaveData.speed = playerData.speed;
        playerSaveData.iconNumber = playerData.iconNumber;

        return playerSaveData;
    }

    private SceneSaveData CreateSceneSaveData()
    {
        SceneSaveData sceneSaveData = new SceneSaveData();

        sceneSaveData.sceneName = SceneManager.GetActiveScene().name;
        sceneSaveData.spawnPointID = "";

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            return sceneSaveData;
        }

        Vector3 playerPosition = player.transform.position;

        sceneSaveData.playerPosX = playerPosition.x;
        sceneSaveData.playerPosY = playerPosition.y;
        sceneSaveData.playerPosZ = playerPosition.z;

        return sceneSaveData;
    }

    private InventorySaveData CreateInventorySaveData()
    {
        InventorySaveData inventorySaveData = new InventorySaveData();

        if (InventoryManager.Instance == null || InventoryManager.Instance.inventoryList == null)
        {
            inventorySaveData.itemSaveDataList = new ItemSaveData[0];
            return inventorySaveData;
        }

        List<InventoryItem> inventoryList = InventoryManager.Instance.inventoryList;
        inventorySaveData.itemSaveDataList = new ItemSaveData[inventoryList.Count];

        for (int i = 0; i < inventoryList.Count; i++)
        {
            ItemSaveData itemSaveData = new ItemSaveData();

            itemSaveData.itemID = inventoryList[i].itemData.itemID;
            itemSaveData.count = inventoryList[i].count;

            inventorySaveData.itemSaveDataList[i] = itemSaveData;
        }

        return inventorySaveData;
    }

    private MagicSaveData[] CreateMagicSaveDataList()
    {
        if (MagicManager.Instance == null || MagicManager.Instance.magicInventoryList == null)
        {
            return new MagicSaveData[0];
        }

        List<MagicInventoryItem> magicInventoryList = MagicManager.Instance.magicInventoryList;
        MagicSaveData[] magicSaveDataList = new MagicSaveData[magicInventoryList.Count];

        for (int i = 0; i < magicInventoryList.Count; i++)
        {
            MagicSaveData magicSaveData = new MagicSaveData();

            magicSaveData.magicID = magicInventoryList[i].magicData.magicID;
            magicSaveData.currentUses = magicInventoryList[i].currentUses;

            magicSaveDataList[i] = magicSaveData;
        }

        return magicSaveDataList;
    }

    private BattleHistorySaveData CreateBattleHistorySaveData()
    {
        if (BattleHistoryManager.Instance == null)
        {
            return new BattleHistorySaveData();
        }

        return BattleHistoryManager.Instance.GetBattleHistorySaveData();
    }

    private void ApplySaveData()
    {
        if (currentSaveData == null)
        {
            Debug.LogError("currentSaveData がありません。");
            return;
        }

        ApplyPlayerSaveData(currentSaveData.playerSaveData);
        ApplyInventorySaveData(currentSaveData.inventorySaveData);
        ApplyMagicSaveData(currentSaveData.magicSaveDataList);
        ApplyBattleHistorySaveData(currentSaveData.battleHistorySaveData);
    }

    private void ApplyPlayerSaveData(PlayerSaveData playerSaveData)
    {
        if (playerSaveData == null)
        {
            Debug.LogWarning("playerSaveData がありません。");
            return;
        }

        if (PlayerManager.Instance == null || PlayerManager.Instance.playerData == null)
        {
            Debug.LogError("PlayerManager または playerData がありません。");
            return;
        }

        PlayerData playerData = PlayerManager.Instance.playerData;

        playerData.playerName = playerSaveData.playerName;
        playerData.maxHp = playerSaveData.maxHp;
        playerData.currentHp = playerSaveData.currentHp;
        playerData.level = playerSaveData.level;
        playerData.currentExp = playerSaveData.currentExp;
        playerData.currentGold = playerSaveData.currentGold;
        playerData.attack = playerSaveData.attack;
        playerData.defense = playerSaveData.defense;
        playerData.speed = playerSaveData.speed;
        playerData.iconNumber = playerSaveData.iconNumber;
    }

    private void ApplyInventorySaveData(InventorySaveData inventorySaveData)
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager がありません。");
            return;
        }

        List<InventoryItem> inventoryList = new List<InventoryItem>();

        if (inventorySaveData == null || inventorySaveData.itemSaveDataList == null)
        {
            InventoryManager.Instance.inventoryList = inventoryList;
            return;
        }

        for (int i = 0; i < inventorySaveData.itemSaveDataList.Length; i++)
        {
            ItemSaveData itemSaveData = inventorySaveData.itemSaveDataList[i];

            if (itemSaveData == null)
            {
                continue;
            }

            ItemData itemData = ItemDatabase.Instance.GetItemData(itemSaveData.itemID);

            if (itemData == null)
            {
                Debug.LogWarning("ItemData が見つかりません: " + itemSaveData.itemID);
                continue;
            }

            InventoryItem inventoryItem = new InventoryItem();

            inventoryItem.itemData = itemData;
            inventoryItem.count = itemSaveData.count;

            inventoryList.Add(inventoryItem);
        }

        InventoryManager.Instance.inventoryList = inventoryList;
    }

    private void ApplyMagicSaveData(MagicSaveData[] magicSaveDataList)
    {
        if (MagicManager.Instance == null)
        {
            Debug.LogError("MagicManager がありません。");
            return;
        }

        List<MagicInventoryItem> magicInventoryList = new List<MagicInventoryItem>();

        if (magicSaveDataList == null)
        {
            MagicManager.Instance.magicInventoryList = magicInventoryList;
            return;
        }

        for (int i = 0; i < magicSaveDataList.Length; i++)
        {
            MagicSaveData magicSaveData = magicSaveDataList[i];

            if (magicSaveData == null)
            {
                continue;
            }

            MagicData magicData = MagicDatabase.Instance.GetMagicData(magicSaveData.magicID);

            if (magicData == null)
            {
                Debug.LogWarning("MagicData が見つかりません: " + magicSaveData.magicID);
                continue;
            }

            MagicInventoryItem magicInventoryItem = new MagicInventoryItem();

            magicInventoryItem.magicData = magicData;
            magicInventoryItem.currentUses = magicSaveData.currentUses;

            magicInventoryList.Add(magicInventoryItem);
        }

        MagicManager.Instance.magicInventoryList = magicInventoryList;
    }

    private void ApplyBattleHistorySaveData(BattleHistorySaveData battleHistorySaveData)
    {
        if (BattleHistoryManager.Instance == null)
        {
            Debug.LogError("BattleHistoryManager がありません。");
            return;
        }

        BattleHistoryManager.Instance.SetBattleHistorySaveData(battleHistorySaveData);
    }

    private void LoadSavedScene()
    {
        if (currentSaveData == null || currentSaveData.sceneSaveData == null)
        {
            Debug.LogError("SceneSaveData がありません。");
            return;
        }

        SceneSaveData sceneSaveData = currentSaveData.sceneSaveData;

        Vector3 playerPosition = new Vector3(
            sceneSaveData.playerPosX,
            sceneSaveData.playerPosY,
            sceneSaveData.playerPosZ
        );

        SceneTransitionManager.LoadSceneAtPosition(
            sceneSaveData.sceneName,
            playerPosition
        );
    }

    private void SaveFile(int slotID)
    {
        string json = JsonUtility.ToJson(currentSaveData, true);
        string path = GetSavePath(slotID);

        File.WriteAllText(path, json);

        Debug.Log("セーブ完了: " + path);
    }

    private string GetSavePath(int slotID)
    {
        return Application.persistentDataPath + "/save_" + slotID + ".json";
    }
}