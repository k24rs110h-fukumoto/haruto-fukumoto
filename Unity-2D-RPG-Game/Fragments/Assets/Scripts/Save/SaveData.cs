[System.Serializable]
public class SaveData
{
    public int slotID;
    public string saveName;
    public string version;
    public string savedAt;
    public float playTime;

    public PlayerSaveData playerSaveData;
    public SceneSaveData sceneSaveData;
    public InventorySaveData inventorySaveData;
    public MagicSaveData[] magicSaveDataList;
    public BattleHistorySaveData battleHistorySaveData;
}