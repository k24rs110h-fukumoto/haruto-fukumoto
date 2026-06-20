using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] private string chestID;

    [SerializeField] private MagicData magicData;
    [SerializeField] private int magicAmount = 0;

    [SerializeField] private ItemData itemData;
    [SerializeField] private int itemAmount = 1;

    [SerializeField] private KeyCode openKey = KeyCode.E;

    private bool isPlayerNear;
    private bool isOpened;

    private void Start()
    {
        if (OpenedChestManager.Instance == null)
        {
            Debug.LogError("OpenedChestManager が Scene にありません。");
            return;
        }

        if (OpenedChestManager.Instance.IsOpened(chestID))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isOpened) return;
        if (!isPlayerNear) return;

        if (Input.GetKeyDown(openKey))
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        if (string.IsNullOrEmpty(chestID))
        {
            Debug.LogError("ChestID が設定されていません。");
            return;
        }

        if (magicData == null && itemData == null)
        {
            Debug.LogWarning("宝箱に魔法もアイテムも設定されていません。");
            return;
        }

        if (magicData != null)
        {
            if (MagicManager.Instance == null)
            {
                Debug.LogError("MagicManager が Scene にありません。");
                return;
            }

            MagicManager.Instance.AddMagic(magicData, magicAmount);

            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.Show(magicData.magicName + " を入手しました。");
            }
        }

        if (itemData != null)
        {
            if (InventoryManager.Instance == null)
            {
                Debug.LogError("InventoryManager が Scene にありません。");
                return;
            }

            InventoryManager.Instance.AddItem(itemData, itemAmount);

            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.Show(itemData.itemName + " を " + itemAmount + " 個入手しました。");
            }
        }

        OpenedChestManager.Instance.AddOpenedChest(chestID);

        isOpened = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerNear = false;
    }
}