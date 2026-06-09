using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<InventoryItem> inventoryList;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (inventoryList == null)
        {
            inventoryList = new List<InventoryItem>();
        }

    }

    // 所持アイテム数を追加関数
    public void AddItem(ItemData itemData, int count)
    {
        foreach (InventoryItem item in inventoryList)
        {
            if (item.itemData.itemID == itemData.itemID)
            {
                item.count = item.count + count;
                return;
            }
        }

        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.itemData = itemData;
        inventoryItem.count = count;
        inventoryList.Add(inventoryItem);
    }

    // アイテム所持確認関数
    public InventoryItem GetItem(string itemID)
    {
        foreach (InventoryItem item in inventoryList)
        {
            if (item.itemData.itemID == itemID)
            {
                return item;
            }
        }
        return null;
    }

    // そのアイテムを指定数以上持っているか判定関数
    public bool HasItem(string itemID, int count)
    {
        InventoryItem item = GetItem(itemID);
        if (item != null)
        {
            if (item.count >= count)
            {
                return true;
            }
        }
        return false;
    }

    // アイテムを何個持っているか返す関数
    public int GetItemCount(string itemID)
    {
        InventoryItem item = GetItem(itemID);
        if (item != null)
        {
            return item.count;
        }
        return 0;
    }

    // アイテムを指定数減らす関数
    public bool RemoveItem(string itemID, int amount)
    {
        InventoryItem item = GetItem(itemID);
        if (item != null)
        {
            if (HasItem(itemID, amount))
            {
                item.count = item.count - amount;
                if (item.count <= 0)
                {
                    inventoryList.Remove(item);
                }
                return true;
            }
            else
            {
                Debug.Log("Do not have enough item.");
                return false;
            }
        }
        Debug.Log("Do not have this item.");
        return false;
    }
}