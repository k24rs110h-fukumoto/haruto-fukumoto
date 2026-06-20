using UnityEngine;

public class EnemyLevelManager : MonoBehaviour
{
    public static EnemyLevelManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int GetEnemyLevel(EnemyData enemyData, int playerLevel, int areaDifficulty)
    {
        int level = areaDifficulty;

        if (playerLevel > areaDifficulty)
        {
            level = playerLevel - 1;
        }

        if (level < 1)
        {
            level = 1;
        }

        if (BattleAILevelManager.Instance != null)
        {
            level = BattleAILevelManager.Instance.GetAdjustedEnemyLevel(enemyData, level);
        }

        return level;
    }
}