using UnityEngine;

[System.Serializable]
public class EnemyAIInput
{
    public float winRate;
    public float loseRate;
    public float escapeRate;

    public float averagePlayerDamage;
    public float averageEnemyDamage;

    public float magicRate;
    public float itemRate;
    public float healRate;

    public float criticalRate;
    public float missRate;

    public float playerLevel;
    public float areaDifficulty;
}