using System;

[Serializable]
public class BattleRecordData
{
    public string enemyID;

    public int enemyLevel;

    public int playerLevel;

    public bool win;

    public bool escape;

    public int playerDamage;

    public int enemyDamage;

    public int magicCount;

    public int healCount;

    public int itemCount;

    public int criticalCount;

    public int missCount;

    public float battleTime;
}