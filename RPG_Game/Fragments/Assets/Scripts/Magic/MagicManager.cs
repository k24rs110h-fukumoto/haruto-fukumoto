using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour
{
    public static MagicManager Instance;
    public List<MagicInventoryItem> magicInventoryList;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (magicInventoryList == null)
        {
            magicInventoryList = new List<MagicInventoryItem>();
        }

    }

    // 魔法追加関数
    public void AddMagic(MagicData magicData, int amount)
    {
        foreach (MagicInventoryItem magic in magicInventoryList)
        {
            if (magic.magicData.magicID == magicData.magicID)
            {
                if(amount != 0)
                {
                    magic.currentUses = magic.currentUses + amount;
                    return;
                }
                magic.currentUses = magic.currentUses + magicData.maxUses;
                return;
            }
        }

        MagicInventoryItem magicInventoryItem = new MagicInventoryItem();
        magicInventoryItem.magicData = magicData;
        magicInventoryItem.currentUses = magicData.maxUses;
        magicInventoryList.Add(magicInventoryItem);
    }

    // 魔法取得関数
    public MagicInventoryItem GetMagic(string magicID)
    {
        foreach (MagicInventoryItem magic in magicInventoryList)
        {
            if (magic.magicData.magicID == magicID)
            {
                return magic;
            }
        }
        return null;
    }

    // 魔法所持確認
    public bool HasMagic(string magicID)
    {
        MagicInventoryItem magic = GetMagic(magicID);
        if (magic != null && magic.currentUses > 0)
        {
            return true;
        }
        return false;
    }

    // 魔法の残り回数取得関数
    public int GetCurrentUses(string magicID)
    {
        MagicInventoryItem magic = GetMagic(magicID);
        if (magic != null)
        {
            return magic.currentUses;
        }
        return 0;
    }

    // 魔法使用
    public bool UseMagic(string magicID, int amount)
    {
        MagicInventoryItem magic = GetMagic(magicID);
        if (magic != null)
        {
            if (magic.currentUses >= amount)
            {
                magic.currentUses = magic.currentUses - amount;
                if (magic.currentUses <= 0)
                {
                    magicInventoryList.Remove(magic);
                }
                return true;
            }
            else
            {
                Debug.Log("Do not have enough magic.");
                return false;
            }
        }
        Debug.Log("Do not have this magic.");
        return false;
    }
}