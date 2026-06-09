using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;
    public ItemData[] itemDataList;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        itemDataList = Resources.LoadAll<ItemData>("Items");
    }

    public ItemData GetItemData(string itemID)
    {
        foreach(ItemData itemData in itemDataList)
        {
            if(itemData.itemID == itemID)
            {
                return itemData;
            }
        }
        return null;
    }
}