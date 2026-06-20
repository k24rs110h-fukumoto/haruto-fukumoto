using System.Collections.Generic;

[System.Serializable]
public class BattleHistorySaveData
{
    public int totalBattleCount;
    public int winCount;
    public int loseCount;
    public int escapeCount;

    public int totalPlayerDamage;
    public int totalEnemyDamage;

    public int normalAttackCount;
    public int magicAttackCount;
    public int itemUseCount;
    public int healCount;

    public int criticalCount;
    public int missCount;

    public int maxPlayerDamage;
    public int maxEnemyDamage;

    public string lastEnemyID;
    public int lastEnemyLevel;

    public List<BattleRecordData> battleRecords = new List<BattleRecordData>();
}