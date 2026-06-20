using TMPro;
using UnityEngine;

public class InventoryPanelController : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI behindDescriptionText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healText;
    [SerializeField] private TextMeshProUGUI deffenseText;
    private int currentItemIndex;
    private bool isSelectingItem;
    [SerializeField] private GameObject itemMenuPanel;
    [SerializeField] private TextMeshProUGUI[] itemMenuTexts;
    private bool isItemMenuOpen;
    private int currentActionIndex;
    [SerializeField] private TextMeshProUGUI ErrorText;
    [SerializeField] private EnemySelectPanelController enemySelectPanelController;

    void Start()
    {
        RefreshInventory();

    }

    private void Update()
    {
        if (InputDelayManager.IsLocked())
        {
            return;
        }
        if (!isSelectingItem) return;
        if (!isItemMenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
            {
                ExitSelectionMode();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                MoveSelectionUp();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                MoveSelectionDown();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                SelectCurrentItem();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
            {
                CloseItemMenu();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                MoveActionUp();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                MoveActionDown();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                InventoryItem inventoryItem = InventoryManager.Instance.inventoryList[currentItemIndex];
                ExecuteItemAction(inventoryItem.itemData.itemType);
            }
        }

    }

    // インベントリセレクト入り処理
    public void EnterSelectionMode()
    {
        if (InventoryManager.Instance.inventoryList.Count == 0)
        {
            return;
        }
        isSelectingItem = true;
        InputDelayManager.Lock(0.15f);
        currentItemIndex = 0;
        descriptionPanel.SetActive(true);
        UpdateSelection();
        UpdateDescription();
    }

    // インベントリセレクト抜け処理
    private void ExitSelectionMode()
    {
        isSelectingItem = false;
        InputDelayManager.Lock(0.15f);
        currentItemIndex = -1;
        descriptionPanel.SetActive(false);
        UpdateSelection();
    }

    // セレクション移動処理
    private void UpdateSelection()
    {
        InventoryItemUI[] inventoryItemUIs = contentParent.GetComponentsInChildren<InventoryItemUI>();
        if (currentItemIndex < 0)
        {
            for (int i = 0; i < inventoryItemUIs.Length; i++)
            {
                inventoryItemUIs[i].SetSelected(false);
            }

            return;
        }

        for (int i = 0; i < inventoryItemUIs.Length; i++)
        {
            if (i == currentItemIndex)
            {
                inventoryItemUIs[i].SetSelected(true);
            }
            else
            {
                inventoryItemUIs[i].SetSelected(false);
            }
        }
    }

    // インベントリ一覧更新
    public void RefreshInventory()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        foreach (InventoryItem item in InventoryManager.Instance.inventoryList)
        {
            GameObject obj = Instantiate(inventoryItemPrefab, contentParent);
            InventoryItemUI itemUI = obj.GetComponent<InventoryItemUI>();
            itemUI.Setup(item);
        }

    }

    // 詳細画面テキスト更新
    private void UpdateDescription()
    {
        if (currentItemIndex < 0)
        {
            itemNameText.text = "";
            itemTypeText.text = "";
            descriptionText.text = "";
            behindDescriptionText.text = "";
            attackText.text = "";
            healText.text = "";
            deffenseText.text = "";
            return;
        }
        InventoryItem inventoryItem = InventoryManager.Instance.inventoryList[currentItemIndex];
        itemNameText.text = inventoryItem.itemData.itemName;
        itemTypeText.text = inventoryItem.itemData.itemType.ToString();
        descriptionText.text = inventoryItem.itemData.description;
        behindDescriptionText.text = inventoryItem.itemData.behindDescription;
        attackText.text = inventoryItem.itemData.attack.ToString();
        healText.text = inventoryItem.itemData.heal.ToString();
        deffenseText.text = inventoryItem.itemData.deffense.ToString();
    }

    // アイテム選択処理
    private void SelectCurrentItem()
    {
        if (currentItemIndex < 0)
        {
            return;
        }
        InventoryItem inventoryItem = InventoryManager.Instance.inventoryList[currentItemIndex];
        SetItemMenu(inventoryItem.itemData.itemType);
        OpenItemMenu();
    }

    // 選択アイテムメニューOpen処理
    private void OpenItemMenu()
    {
        isItemMenuOpen = true;
        InputDelayManager.Lock(0.15f);
        itemMenuPanel.SetActive(true);
        currentActionIndex = 0;

        UpdateItemMenuTextDisplay();
    }

    // 選択アイテムメニューClose処理
    private void CloseItemMenu()
    {
        isItemMenuOpen = false;
        InputDelayManager.Lock(0.15f);
        itemMenuPanel.SetActive(false);
        currentActionIndex = 0;
        BattleMenuManager.Instance.OpenBattleMenu();
    }

    private void ExecuteItemAction(ItemType itemType)
    {
        InventoryItem inventoryItem = InventoryManager.Instance.inventoryList[currentItemIndex];
        switch (itemType)
        {
            case ItemType.Recovery:
                switch (currentActionIndex)
                {
                    case 0:
                        if (GameStateManager.GetState() == GameStateManager.GameState.Battle)
                        {
                            BattleManager.Instance.DecideOrder(null, inventoryItem);
                            int itemCount = InventoryManager.Instance.inventoryList.Count;

                            if (itemCount == 0)
                            {
                                currentItemIndex = -1;
                            }
                            else if (currentItemIndex >= itemCount)
                            {
                                currentItemIndex = itemCount - 1;
                            }

                            BattleMenuManager.Instance.OpenBattleMenu();
                            RefreshInventory();
                            UpdateSelection();
                            UpdateDescription();
                            CloseItemMenu();
                            return;
                        }
                        else
                        {
                            if (PlayerManager.Instance.playerData.currentHp + inventoryItem.itemData.heal > PlayerManager.Instance.playerData.maxHp)
                            {
                                PlayerManager.Instance.playerData.currentHp = PlayerManager.Instance.playerData.maxHp;
                            }
                            else
                            {
                                PlayerManager.Instance.playerData.currentHp = PlayerManager.Instance.playerData.currentHp + inventoryItem.itemData.heal;
                            }
                            UseItemSuccess(inventoryItem);
                            BattleMenuManager.Instance.OpenBattleMenu();
                            return;
                        }
                    case 1:
                        UseItemSuccess(inventoryItem);
                        return;
                    case 2:
                        CloseItemMenu();
                        return;
                }
                return;
            case ItemType.BattleItem:
                switch (currentActionIndex)
                {
                    case 0:
                        if (GameStateManager.GetState() != GameStateManager.GameState.Battle)
                        {
                            ErrorText.text = "今は使えないみたいだ";
                            return;
                        }
                        else
                        {
                            CloseItemMenu();
                            ExitSelectionMode();

                            enemySelectPanelController.EnterSelectionMode(inventoryItem);
                            return;
                        }

                    case 1:
                        UseItemSuccess(inventoryItem);
                        return;
                    case 2:
                        CloseItemMenu();
                        return;
                }
                return;
            case ItemType.Material:
            case ItemType.QuestItem:
            case ItemType.KeyItem:
                switch (currentActionIndex)
                {
                    case 0:
                        Debug.Log("今は使えない");
                        return;
                    case 1:
                        UseItemSuccess(inventoryItem);
                        return;
                    case 2:
                        CloseItemMenu();
                        return;
                }
                return;
            case ItemType.Equipment:
                switch (currentActionIndex)
                {
                    case 0:

                        return;
                    case 1:
                        UseItemSuccess(inventoryItem);
                        return;
                    case 2:
                        CloseItemMenu();
                        return;
                }
                return;

        }
    }

    private void UseItemSuccess(InventoryItem inventoryItem)
    {
        InventoryManager.Instance.RemoveItem(inventoryItem.itemData.itemID, 1);

        int itemCount = InventoryManager.Instance.inventoryList.Count;

        if (itemCount == 0)
        {
            currentItemIndex = -1;
        }
        else if (currentItemIndex >= itemCount)
        {
            currentItemIndex = itemCount - 1;
        }

        RefreshInventory();
        UpdateSelection();
        UpdateDescription();
        CloseItemMenu();
    }

    private void SetItemMenu(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Recovery:
            case ItemType.BattleItem:
            case ItemType.Material:
            case ItemType.QuestItem:
            case ItemType.KeyItem:
                itemMenuTexts[0].text = "使う";
                itemMenuTexts[1].text = "捨てる";
                itemMenuTexts[2].text = "キャンセル";
                return;
            case ItemType.Equipment:
                itemMenuTexts[0].text = "装備";
                itemMenuTexts[1].text = "捨てる";
                itemMenuTexts[2].text = "キャンセル";
                return;

        }
    }

    // アイテムメニュー画面更新
    private void UpdateItemMenuTextDisplay()
    {
        for (int i = 0; i < itemMenuTexts.Length; i++)
        {
            itemMenuTexts[i].text = itemMenuTexts[i].text.Replace("＞ ", "");
        }

        itemMenuTexts[currentActionIndex].text = "＞ " + itemMenuTexts[currentActionIndex].text;
    }

    // 上セレクト処理
    private void MoveSelectionUp()
    {
        if (currentItemIndex == 0)
        {
            currentItemIndex = InventoryManager.Instance.inventoryList.Count - 1;
        }
        else
        {
            currentItemIndex--;
        }
        UpdateSelection();
        UpdateDescription();
    }

    // 下セレクト処理
    private void MoveSelectionDown()
    {
        if (currentItemIndex == InventoryManager.Instance.inventoryList.Count - 1)
        {
            currentItemIndex = 0;
        }
        else
        {
            currentItemIndex++;
        }
        UpdateSelection();
        UpdateDescription();
    }

    // 上セレクト処理
    private void MoveActionUp()
    {
        if (currentActionIndex == 0)
        {
            currentActionIndex = itemMenuTexts.Length - 1;
        }
        else
        {
            currentActionIndex--;
        }
        UpdateItemMenuTextDisplay();
    }

    // 下セレクト処理
    private void MoveActionDown()
    {
        if (currentActionIndex == itemMenuTexts.Length - 1)
        {
            currentActionIndex = 0;
        }
        else
        {
            currentActionIndex++;
        }
        UpdateItemMenuTextDisplay();
    }
}