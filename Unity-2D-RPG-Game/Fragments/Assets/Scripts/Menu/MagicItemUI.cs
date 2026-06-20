using TMPro;
using UnityEngine;

public class MagicItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI magicNameText;
    [SerializeField] private TextMeshProUGUI usesText;
    private string magicName;

    public void Setup(MagicInventoryItem item)
    {
        magicName = item.magicData.magicName;
        magicNameText.text = magicName;
        usesText.text = item.currentUses + " / " + item.magicData.maxUses;
    }

    public void SetSelected(bool isSelected)
    {
        if(isSelected)
        {
            magicNameText.text = "＞ " + magicName;
        }
        else
        {
            magicNameText.text = magicName;
        }
    }
}