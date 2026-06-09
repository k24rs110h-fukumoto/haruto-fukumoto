using UnityEngine;

[System.Serializable]
public class BattleEnemyData
{
    public EnemyData enemyData;
    public int maxHp;
    public int currentHp;
    public int attack;
    public int defense;
    public int speed;
    public int level;
    public enum StatusEffect
    {
        None,
        Poison,
        Sleep,
        Paralyze
    }

    public StatusEffect statusEffect;

    public int GetExpReward()
    {
        int exp = enemyData.expReward;
        exp = exp + level * 2;
        return exp;
    }

    public int GetGoldReward()
    {
        int gold = enemyData.goldReward;
        gold = gold + level * 3;
        return gold;
    }
}