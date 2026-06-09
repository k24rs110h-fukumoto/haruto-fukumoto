using TMPro;
using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI countText;
    private string itemName;

    public void Setup(InventoryItem item)
    {
        itemName = item.itemData.itemName;
        itemNameText.text = itemName;
        countText.text = " × " + item.count.ToString();
    }

    public void SetSelected(bool isSelected)
    {
        if(isSelected)
        {
            itemNameText.text = "＞ " + itemName;
        }
        else
        {
            itemNameText.text = itemName;
        }
    }
}