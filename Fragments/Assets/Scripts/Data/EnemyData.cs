using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyID;
    public string enemyName;
    public string description;
    public Sprite icon;
    public Sprite battleImage;
    public int maxHp;
    public int attack;
    public int defense;
    public int expReward;
    public int speed;
    public int goldReward;
    public ItemData dropItem;
    public float dropRate;
    public MagicElement element;
    public int baseLevel;
}