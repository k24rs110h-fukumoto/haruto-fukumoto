using UnityEngine;

public class RequiredItemObject : MonoBehaviour, IInteractable
{
    [SerializeField] private RequiredItemType requiredItemType;
    [SerializeField] private ItemData requiredItem;
    [SerializeField] private string failMessage;
    [SerializeField] private bool consumeItem;
    [SerializeField] private string successMessage;
    [SerializeField] private InteractionUIManager interactionUIManager;

    public void Interact()
    {
        PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();

        if (inventory == null)
        {
            return;
        }

        if (!inventory.HasItem())
        {
            Fail();
            return;
        }

        if (inventory.CurrentItem == requiredItem)
        {
            if (consumeItem)
            {
                inventory.ConsumeItem();
            }

            Success();
        }
        else
        {
            Fail();
        }
    }

    public virtual string GetPrompt()
    {
        switch (requiredItemType)
        {
            case RequiredItemType.Door:
                return "[E] 開ける";
            case RequiredItemType.Altar:
                return "[E] 置く";
            case RequiredItemType.Goal:
                return "[E] 脱出する";
            case RequiredItemType.Box:
                return "[E] 開ける";
            default:
                return "[E] 使用する";
        }
    }

    protected virtual void Fail()
    {
        if (interactionUIManager != null)
        {
            interactionUIManager.ShowMessage(failMessage);
        }
    }

    protected virtual void Success()
    {
        if (interactionUIManager != null)
        {
            interactionUIManager.ShowMessage(successMessage);
        }
    }
}