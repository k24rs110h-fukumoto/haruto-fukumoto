using UnityEngine;

public class BattleHistoryManager : MonoBehaviour
{
    public static BattleHistoryManager Instance;

    public BattleHistorySaveData battleHistorySaveData = new BattleHistorySaveData();

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

    public void RecordBattleStart(string enemyID, int enemyLevel)
    {
        battleHistorySaveData.totalBattleCount++;
        battleHistorySaveData.lastEnemyID = enemyID;
        battleHistorySaveData.lastEnemyLevel = enemyLevel;
    }

    public void RecordWin()
    {
        battleHistorySaveData.winCount++;
    }

    public void RecordLose()
    {
        battleHistorySaveData.loseCount++;
    }

    public void RecordEscape()
    {
        battleHistorySaveData.escapeCount++;
    }

    public void RecordPlayerDamage(int damage)
    {
        battleHistorySaveData.totalPlayerDamage += damage;

        if (damage > battleHistorySaveData.maxPlayerDamage)
        {
            battleHistorySaveData.maxPlayerDamage = damage;
        }
    }

    public void RecordEnemyDamage(int damage)
    {
        battleHistorySaveData.totalEnemyDamage += damage;

        if (damage > battleHistorySaveData.maxEnemyDamage)
        {
            battleHistorySaveData.maxEnemyDamage = damage;
        }
    }

    public void RecordNormalAttack()
    {
        battleHistorySaveData.normalAttackCount++;
    }

    public void RecordMagicAttack()
    {
        battleHistorySaveData.magicAttackCount++;
    }

    public void RecordItemUse()
    {
        battleHistorySaveData.itemUseCount++;
    }

    public void RecordHeal()
    {
        battleHistorySaveData.healCount++;
    }

    public void RecordCritical()
    {
        battleHistorySaveData.criticalCount++;
    }

    public void RecordMiss()
    {
        battleHistorySaveData.missCount++;
    }

    public void AddBattleRecord(BattleRecordData record)
    {
        if (record == null)
        {
            return;
        }

        if (battleHistorySaveData.battleRecords == null)
        {
            battleHistorySaveData.battleRecords = new System.Collections.Generic.List<BattleRecordData>();
        }

        battleHistorySaveData.battleRecords.Add(record);

        if (battleHistorySaveData.battleRecords.Count > 1000)
        {
            battleHistorySaveData.battleRecords.RemoveAt(0);
        }
    }

    public void SetBattleHistorySaveData(BattleHistorySaveData data)
    {
        if (data == null)
        {
            battleHistorySaveData = new BattleHistorySaveData();
            return;
        }

        battleHistorySaveData = data;

        if (battleHistorySaveData.battleRecords == null)
        {
            battleHistorySaveData.battleRecords = new System.Collections.Generic.List<BattleRecordData>();
        }
    }

    public BattleHistorySaveData GetBattleHistorySaveData()
    {
        return battleHistorySaveData;
    }
}