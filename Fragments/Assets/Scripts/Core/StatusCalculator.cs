using UnityEngine;

public static class StatusCalculator
{
    public static int GetMaxHp(int baseHp, int level)
    {
        return Mathf.RoundToInt(baseHp * Mathf.Pow(level, 1.2f));
    }

    public static int GetAttack(int baseAttack, int level)
    {
        return Mathf.RoundToInt(baseAttack * Mathf.Pow(level, 1.15f));
    }

    public static int GetDefense(int baseDefense, int level)
    {
        return Mathf.RoundToInt(baseDefense * Mathf.Pow(level, 1.15f));
    }

    public static int GetSpeed(int baseSpeed, int level)
    {
        return Mathf.RoundToInt(baseSpeed * Mathf.Pow(level, 1.1f));
    }
}