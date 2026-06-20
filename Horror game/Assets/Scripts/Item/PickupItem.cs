using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;

    public void Interact()
    {
        PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();

        if (inventory == null)
        {
            return;
        }

        if (itemData == null)
        {
            return;
        }

        if (inventory.HasItem())
        {
            ItemData oldItem = inventory.DropItem();

            if (oldItem != null && oldItem.dropPrefab != null)
            {
                Instantiate(oldItem.dropPrefab, transform.position, transform.rotation);
            }
        }

        inventory.SetItem(itemData);
        Destroy(gameObject);
    }

    public string GetPrompt()
    {
        return "[E] 拾う";
    }
}