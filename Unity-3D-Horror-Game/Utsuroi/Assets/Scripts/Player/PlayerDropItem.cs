using UnityEngine;

public class PlayerDropItem : MonoBehaviour
{
    [SerializeField] private Transform dropPoint;
    private PlayerInventory inventory;

    private void Awake()
    {
        inventory = FindFirstObjectByType<PlayerInventory>();
    }

    public void DropItem()
    {
        if (inventory == null)
        {
            return;
        }

        if (dropPoint == null)
        {
            return;
        }

        if (!inventory.HasItem())
        {
            return;
        }

        ItemData item = inventory.DropItem();

        if (item == null || item.dropPrefab == null)
        {
            return;
        }

        Instantiate(item.dropPrefab, dropPoint.position, Quaternion.identity);
    }
}