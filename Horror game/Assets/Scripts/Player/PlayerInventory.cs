using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public ItemData CurrentItem
    {
        get;
        private set;
    }

    public bool HasItem()
    {
        return CurrentItem != null;
    }

    public void SetItem(ItemData item)
    {
        CurrentItem = item;
    }

    public ItemData DropItem()
    {
        ItemData item = CurrentItem;
        CurrentItem = null;
        return item;
    }

    public void ConsumeItem()
    {
        CurrentItem = null;
    }

}