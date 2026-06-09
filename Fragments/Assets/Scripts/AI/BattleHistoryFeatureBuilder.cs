using UnityEngine;

public static class BattleHistoryFeatureBuilder
{
    public static float[] BuildFeatures(BattleHistorySaveData history)
    {
        float[] features = new float[10];

        if (history == null)
        {
            return features;
        }

        int battleCount = Mathf.Max(history.totalBattleCount, 1);

        features[0] = (float)history.winCount / battleCount;
        features[1] = (float)history.loseCount / battleCount;
        features[2] = (float)history.escapeCount / battleCount;

        features[3] = (float)history.totalPlayerDamage / battleCount;
        features[4] = (float)history.totalEnemyDamage / battleCount;

        features[5] = (float)history.magicAttackCount / battleCount;
        features[6] = (float)history.itemUseCount / battleCount;
        features[7] = (float)history.healCount / battleCount;

        features[8] = (float)history.criticalCount / battleCount;
        features[9] = (float)history.missCount / battleCount;

        return features;
    }

    public static EnemyAIInput BuildAIInput(BattleHistorySaveData history, int playerLevel, int areaDifficulty)
    {
        EnemyAIInput input = new EnemyAIInput();

        int battleCount = Mathf.Max(history.totalBattleCount, 1);

        input.winRate = (float)history.winCount / battleCount;

        input.loseRate = (float)history.loseCount / battleCount;

        input.escapeRate = (float)history.escapeCount / battleCount;

        input.averagePlayerDamage = (float)history.totalPlayerDamage / battleCount;

        input.averageEnemyDamage = (float)history.totalEnemyDamage / battleCount;

        input.magicRate = (float)history.magicAttackCount / battleCount;

        input.itemRate = (float)history.itemUseCount / battleCount;

        input.healRate = (float)history.healCount / battleCount;

        input.criticalRate = (float)history.criticalCount / battleCount;

        input.missRate = (float)history.missCount / battleCount;

        input.playerLevel = playerLevel;

        input.areaDifficulty = areaDifficulty;

        return input;
    }
}