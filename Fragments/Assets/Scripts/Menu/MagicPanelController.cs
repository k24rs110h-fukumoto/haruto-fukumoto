using UnityEngine;
using TMPro;

public class MagicPanelController : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject magicItemPrefab;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private GameObject magicMenuPanel;
    [SerializeField] private TextMeshProUGUI magicIDText;
    [SerializeField] private TextMeshProUGUI magicNameText;
    [SerializeField] private TextMeshProUGUI magicTypeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI elementText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI ErrorText;
    [SerializeField] private EnemySelectPanelController enemySelectPanelController;
    private int currentMagicIndex;
    private bool isSelectingMagic;
    private bool isMagicMenuOpen;
    private int currentActionIndex;
    [SerializeField] private TextMeshProUGUI[] magicMenuTexts;

    void Start()
    {
        RefreshMagic();
        magicMenuTexts[0].text = "使う";
        magicMenuTexts[1].text = "捨てる";
        magicMenuTexts[2].text = "キャンセル";
    }

    private void Update()
    {
        if (InputDelayManager.IsLocked())
        {
            return;
        }
        if (!isSelectingMagic) return;

        if (!isMagicMenuOpen)
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
                CloseMagicMenu();
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
                MagicInventoryItem magicInventoryItem = MagicManager.Instance.magicInventoryList[currentMagicIndex];
                ExecuteMagicAction(magicInventoryItem);
            }
        }
    }

    private void ExecuteMagicAction(MagicInventoryItem magic)
    {
        if (magic != null)
        {
            switch (magic.magicData.magicType)
            {
                case MagicType.Attack:
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
                                CloseMagicMenu();
                                ExitSelectionMode();

                                switch (magic.magicData.targetType)
                                {
                                    case MagicTargetType.Enemy:
                                        enemySelectPanelController.EnterSelectionMode(magic.magicData);
                                        return;
                                    case MagicTargetType.AllEnemies:
                                        BattleManager.Instance.DecideOrder(null, magic.magicData, true);
                                        BattleMenuManager.Instance.OpenBattleMenu();
                                        return;
                                }

                                int magicCount = MagicManager.Instance.magicInventoryList.Count;

                                if (magicCount == 0)
                                {
                                    currentMagicIndex = -1;
                                }
                                else if (currentMagicIndex >= magicCount)
                                {
                                    currentMagicIndex = magicCount - 1;
                                }

                                RefreshMagic();
                                UpdateSelection();
                                UpdateDescription();
                                CloseMagicMenu();

                                return;
                            }
                        case 1:
                            UseMagicSuccess(magic);
                            return;
                        case 2:
                            CloseMagicMenu();
                            return;
                    }
                    return;
                case MagicType.Heal:
                    switch (currentActionIndex)
                    {
                        case 0:
                            if (GameStateManager.GetState() == GameStateManager.GameState.Battle)
                            {
                                CloseMagicMenu();
                                ExitSelectionMode();

                                BattleManager.Instance.DecideOrder(null, magic.magicData, false);
                                BattleMenuManager.Instance.OpenBattleMenu();
                                return;
                            }

                            if (PlayerManager.Instance.playerData.currentHp + magic.magicData.power > PlayerManager.Instance.playerData.maxHp)
                            {
                                PlayerManager.Instance.playerData.currentHp = PlayerManager.Instance.playerData.maxHp;
                            }
                            else
                            {
                                PlayerManager.Instance.playerData.currentHp = PlayerManager.Instance.playerData.currentHp + magic.magicData.power;
                            }

                            UseMagicSuccess(magic);
                            return;
                        case 1:
                            UseMagicSuccess(magic);
                            return;
                        case 2:
                            CloseMagicMenu();
                            return;
                    }
                    return;
                case MagicType.Guard:
                case MagicType.Buff:
                case MagicType.Debuff:
                case MagicType.Summon:
                case MagicType.Special:
                    switch (currentActionIndex)
                    {
                        case 0:
                            Debug.Log("今は使えない");
                            return;
                        case 1:
                            UseMagicSuccess(magic);
                            return;
                        case 2:
                            CloseMagicMenu();
                            return;
                    }
                    return;

            }
        }
    }


    // インベントリセレクト入り処理
    public void EnterSelectionMode()
    {
        if (MagicManager.Instance.magicInventoryList.Count == 0)
        {
            return;
        }

        isSelectingMagic = true;
        InputDelayManager.Lock(0.15f);
        currentMagicIndex = 0;
        descriptionPanel.SetActive(true);
        UpdateSelection();
        UpdateDescription();
    }

    // インベントリセレクト抜け処理
    private void ExitSelectionMode()
    {
        isSelectingMagic = false;
        InputDelayManager.Lock(0.15f);
        currentMagicIndex = -1;
        descriptionPanel.SetActive(false);
        UpdateSelection();
    }

    // アイテム選択処理
    private void SelectCurrentItem()
    {
        if (currentMagicIndex < 0)
        {
            return;
        }
        OpenMagicMenu();
    }

    // 選択アイテムメニューOpen処理
    private void OpenMagicMenu()
    {
        isMagicMenuOpen = true;
        InputDelayManager.Lock(0.15f);
        magicMenuPanel.SetActive(true);
        currentActionIndex = 0;

        UpdateMagicMenuTextDisplay();
    }

    // 選択アイテムメニューClose処理
    private void CloseMagicMenu()
    {
        isMagicMenuOpen = false;
        InputDelayManager.Lock(0.15f);
        magicMenuPanel.SetActive(false);
        currentActionIndex = 0;
        BattleMenuManager.Instance.OpenBattleMenu();
    }

    private void UpdateSelection()
    {
        MagicItemUI[] magicItemUIs = contentParent.GetComponentsInChildren<MagicItemUI>();
        if (currentMagicIndex < 0)
        {
            for (int i = 0; i < magicItemUIs.Length; i++)
            {
                magicItemUIs[i].SetSelected(false);
            }

            return;
        }

        for (int i = 0; i < magicItemUIs.Length; i++)
        {
            if (i == currentMagicIndex)
            {
                magicItemUIs[i].SetSelected(true);
            }
            else
            {
                magicItemUIs[i].SetSelected(false);
            }
        }
    }

    public void RefreshMagic()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        foreach (MagicInventoryItem magic in MagicManager.Instance.magicInventoryList)
        {
            GameObject obj = Instantiate(magicItemPrefab, contentParent);
            MagicItemUI magicUI = obj.GetComponent<MagicItemUI>();
            magicUI.Setup(magic);
        }
    }

    private void UpdateDescription()
    {
        if (currentMagicIndex < 0)
        {
            magicIDText.text = "";
            magicNameText.text = "";
            magicTypeText.text = "";
            descriptionText.text = "";
            elementText.text = "";
            rarityText.text = "";
            powerText.text = "";
            return;
        }
        MagicInventoryItem magicInventoryItem = MagicManager.Instance.magicInventoryList[currentMagicIndex];
        magicIDText.text = magicInventoryItem.magicData.magicID;
        magicNameText.text = magicInventoryItem.magicData.magicName;
        magicTypeText.text = magicInventoryItem.magicData.magicType.ToString();
        descriptionText.text = magicInventoryItem.magicData.description;
        elementText.text = magicInventoryItem.magicData.element.ToString();
        rarityText.text = magicInventoryItem.magicData.rarity.ToString();
        powerText.text = magicInventoryItem.magicData.power.ToString();
    }

    private void UseMagicSuccess(MagicInventoryItem magicInventoryItem)
    {
        MagicManager.Instance.UseMagic(magicInventoryItem.magicData.magicID, 1);

        int magicCount = MagicManager.Instance.magicInventoryList.Count;

        if (magicCount == 0)
        {
            currentMagicIndex = -1;
        }
        else if (currentMagicIndex >= magicCount)
        {
            currentMagicIndex = magicCount - 1;
        }

        RefreshMagic();
        UpdateSelection();
        UpdateDescription();
        CloseMagicMenu();
    }

    // 魔法メニュー画面更新
    private void UpdateMagicMenuTextDisplay()
    {
        for (int i = 0; i < magicMenuTexts.Length; i++)
        {
            magicMenuTexts[i].text = magicMenuTexts[i].text.Replace("＞ ", "");
        }

        magicMenuTexts[currentActionIndex].text = "＞ " + magicMenuTexts[currentActionIndex].text;
    }

    // 上セレクト処理
    private void MoveSelectionUp()
    {
        if (currentMagicIndex == 0)
        {
            currentMagicIndex = MagicManager.Instance.magicInventoryList.Count - 1;
        }
        else
        {
            currentMagicIndex--;
        }
        UpdateSelection();
        UpdateDescription();
    }

    // 下セレクト処理
    private void MoveSelectionDown()
    {
        if (currentMagicIndex == MagicManager.Instance.magicInventoryList.Count - 1)
        {
            currentMagicIndex = 0;
        }
        else
        {
            currentMagicIndex++;
        }
        UpdateSelection();
        UpdateDescription();
    }

    // 上セレクト処理
    private void MoveActionUp()
    {
        if (currentActionIndex == 0)
        {
            currentActionIndex = magicMenuTexts.Length - 1;
        }
        else
        {
            currentActionIndex--;
        }
        UpdateMagicMenuTextDisplay();
    }

    // 下セレクト処理
    private void MoveActionDown()
    {
        if (currentActionIndex == magicMenuTexts.Length - 1)
        {
            currentActionIndex = 0;
        }
        else
        {
            currentActionIndex++;
        }
        UpdateMagicMenuTextDisplay();
    }
}