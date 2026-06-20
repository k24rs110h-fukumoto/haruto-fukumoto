using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType itemType;
    public ItemRarity rarity;
    public EquipmentType equipmentType;
    public string behindDescription;

    public int attack
    {
        get
        {
            if (itemType == ItemType.BattleItem)
            {
                return GetBaseValue();
            }

            if (itemType == ItemType.Equipment && equipmentType == EquipmentType.Weapon)
            {
                return GetBaseValue();
            }

            return 0;
        }
    }

    public int heal
    {
        get
        {
            if (itemType == ItemType.Recovery)
            {
                return GetBaseValue();
            }

            return 0;
        }
    }

    public int deffense
    {
        get
        {
            if (itemType == ItemType.Equipment && equipmentType == EquipmentType.Armor)
            {
                return GetBaseValue();
            }

            if (itemType == ItemType.Equipment && equipmentType == EquipmentType.Accessory)
            {
                return Mathf.RoundToInt(GetBaseValue() * 0.7f);
            }

            return 0;
        }
    }

    public int buyPrice
    {
        get
        {
            switch (rarity)
            {
                case ItemRarity.Common:
                    return 50;
                case ItemRarity.Rare:
                    return 150;
                case ItemRarity.Epic:
                    return 400;
                case ItemRarity.Legendary:
                    return 1000;
                case ItemRarity.Mythic:
                    return 2500;
                default:
                    return 0;
            }
        }
    }

    private int GetBaseValue()
    {
        switch (rarity)
        {
            case ItemRarity.Common:
                return 30;
            case ItemRarity.Rare:
                return 60;
            case ItemRarity.Epic:
                return 100;
            case ItemRarity.Legendary:
                return 160;
            case ItemRarity.Mythic:
                return 250;
            default:
                return 0;
        }
    }
}

public enum ItemType
{
    Recovery,
    BattleItem,
    Material,
    QuestItem,
    Equipment,
    KeyItem
}

public enum ItemRarity
{
    Common,
    Rare,
    Epic,
    Legendary,
    Mythic
}

public enum EquipmentType
{
    None,
    Weapon,
    Armor,
    Accessory
}