using UnityEngine;

public class BattleAILevelManager : MonoBehaviour
{
    public static BattleAILevelManager Instance;
    [SerializeField] private int minEnemyLevel = 1;
    [SerializeField] private int maxEnemyLevel = 99;
    [SerializeField] private bool useSentis = false;

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

    public int GetAdjustedEnemyLevel(EnemyData enemyData, int baseLevel)
    {
        int adjustment = 0;

        if (useSentis)
        {
            adjustment = GetSentisLevelAdjustment(enemyData, baseLevel);
        }
        else
        {
            adjustment = GetRuleBasedLevelAdjustment();
        }

        int resultLevel = baseLevel + adjustment;
        resultLevel = Mathf.Clamp(resultLevel, minEnemyLevel, maxEnemyLevel);

        return resultLevel;
    }

    private int GetRuleBasedLevelAdjustment()
    {
        if (BattleHistoryManager.Instance == null)
        {
            return 0;
        }

        BattleHistorySaveData history =
            BattleHistoryManager.Instance.GetBattleHistorySaveData();

        if (history == null)
        {
            return 0;
        }

        if (history.totalBattleCount < 3)
        {
            return 0;
        }

        float winRate = 0f;
        float loseRate = 0f;
        float escapeRate = 0f;

        if (history.totalBattleCount > 0)
        {
            winRate = (float)history.winCount / history.totalBattleCount;
            loseRate = (float)history.loseCount / history.totalBattleCount;
            escapeRate = (float)history.escapeCount / history.totalBattleCount;
        }

        float averagePlayerDamage = 0f;
        float averageEnemyDamage = 0f;

        if (history.totalBattleCount > 0)
        {
            averagePlayerDamage = (float)history.totalPlayerDamage / history.totalBattleCount;
            averageEnemyDamage = (float)history.totalEnemyDamage / history.totalBattleCount;
        }

        int adjustment = 0;

        if (winRate >= 0.8f)
        {
            adjustment += 2;
        }
        else if (winRate >= 0.6f)
        {
            adjustment += 1;
        }

        if (loseRate >= 0.4f)
        {
            adjustment -= 2;
        }
        else if (loseRate >= 0.25f)
        {
            adjustment -= 1;
        }

        if (escapeRate >= 0.4f)
        {
            adjustment -= 1;
        }

        if (averagePlayerDamage > averageEnemyDamage * 1.5f)
        {
            adjustment += 1;
        }

        if (averageEnemyDamage > averagePlayerDamage * 1.5f)
        {
            adjustment -= 1;
        }

        return Mathf.Clamp(adjustment, -2, 3);
    }

    private int GetSentisLevelAdjustment(EnemyData enemyData, int baseLevel)
    {
        if (BattleHistoryManager.Instance == null)
        {
            return 0;
        }

        if (EnemyLevelSentisManager.Instance == null)
        {
            return GetRuleBasedLevelAdjustment();
        }

        BattleHistorySaveData history = BattleHistoryManager.Instance.GetBattleHistorySaveData();

        float[] features = BattleHistoryFeatureBuilder.BuildFeatures(history);

        return EnemyLevelSentisManager.Instance.PredictLevelAdjustment(features);
    }
}