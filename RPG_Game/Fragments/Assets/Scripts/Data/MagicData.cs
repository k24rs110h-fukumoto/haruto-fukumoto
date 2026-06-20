using UnityEngine;

[CreateAssetMenu(fileName = "MagicData", menuName = "Game/MagicData")]
public class MagicData : ScriptableObject
{
    public string magicID;
    public string magicName;
    public string description;
    public Sprite icon;
    public MagicType magicType;
    public MagicElement element;
    public MagicTargetType targetType;
    public MagicRarity rarity;
    public string behindDescription;

    public int maxUses
    {
        get
        {
            switch (rarity)
            {
                case MagicRarity.Common:
                    return 15;
                case MagicRarity.Rare:
                    return 10;
                case MagicRarity.Epic:
                    return 7;
                case MagicRarity.Legendary:
                    return 5;
                case MagicRarity.Mythic:
                    return 3;
                default:
                    return 1;
            }
        }
    }

    public int buyPrice
    {
        get
        {
            switch (rarity)
            {
                case MagicRarity.Common:
                    return 100;
                case MagicRarity.Rare:
                    return 300;
                case MagicRarity.Epic:
                    return 800;
                case MagicRarity.Legendary:
                    return 2000;
                case MagicRarity.Mythic:
                    return 5000;
                default:
                    return 0;
            }
        }
    }

    public int power
    {
        get
        {
            int basePower = 0;

            switch (rarity)
            {
                case MagicRarity.Common:
                    basePower = 30;
                    break;
                case MagicRarity.Rare:
                    basePower = 55;
                    break;
                case MagicRarity.Epic:
                    basePower = 80;
                    break;
                case MagicRarity.Legendary:
                    basePower = 120;
                    break;
                case MagicRarity.Mythic:
                    basePower = 170;
                    break;
            }

            if (targetType == MagicTargetType.AllEnemies)
            {
                basePower = Mathf.RoundToInt(basePower * 0.7f);
            }

            if (magicType == MagicType.Heal)
            {
                basePower = Mathf.RoundToInt(basePower * 0.8f);
            }

            return basePower;
        }
    }
}