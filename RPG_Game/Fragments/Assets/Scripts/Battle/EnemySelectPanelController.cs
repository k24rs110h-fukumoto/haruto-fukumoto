using UnityEngine;

public class EnemySelectPanelController : MonoBehaviour
{
    // 攻撃手段
    private enum SelectActionType
    {
        None,
        Attack,
        Magic,
        Item
    }

    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject enemySelectPrefab;
    [SerializeField] private GameObject selectPanel;

    private int currentEnemyIndex = -1;
    private int currentAttack;
    private MagicData currentMagic;
    private InventoryItem currentItem;
    private SelectActionType currentActionType = SelectActionType.None;
    private bool isSelectingEnemy;

    private void Start()
    {
        RefreshEnemyList();

        if (selectPanel != null)
        {
            selectPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (InputDelayManager.IsLocked())
        {
            return;
        }

        if (!isSelectingEnemy)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            ExitSelectionMode();
            BattleMenuManager.Instance.OpenBattleMenu();
            return;
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
            SelectCurrentEnemy();
        }
    }

    // 通常攻撃での敵選択アクセス
    public void EnterSelectionMode(int attack)
    {
        currentAttack = attack;
        currentActionType = SelectActionType.Attack;

        StartEnemySelection();
    }

    // 魔法攻撃での敵選択アクセス
    public void EnterSelectionMode(MagicData magic)
    {
        currentMagic = magic;
        currentActionType = SelectActionType.Magic;

        StartEnemySelection();
    }

    // アイテム攻撃での敵選択アクセス
    public void EnterSelectionMode(InventoryItem item)
    {
        currentItem = item;
        currentActionType = SelectActionType.Item;

        StartEnemySelection();
    }

    // 攻撃対象選択
    private void StartEnemySelection()
    {
        RefreshEnemyList();

        if (BattleManager.Instance.battleEnemyDataList.Count <= 0)
        {
            return;
        }
        if (BattleManager.Instance.battleEnemyDataList.Count == 1)
        {
            ExecuteAction(BattleManager.Instance.battleEnemyDataList[0]);
            ResetSelectionState();
            return;
        }

        isSelectingEnemy = true;
        currentEnemyIndex = 0;

        if (selectPanel != null)
        {
            selectPanel.SetActive(true);
        }

        InputDelayManager.Lock(0.15f);
        UpdateSelection();
    }

    // 選択中の敵に決定
    private void SelectCurrentEnemy()
    {
        if (currentEnemyIndex < 0)
        {
            return;
        }
        if (currentEnemyIndex >= BattleManager.Instance.battleEnemyDataList.Count)
        {
            return;
        }

        BattleEnemyData selectedEnemy = BattleManager.Instance.battleEnemyDataList[currentEnemyIndex];

        ExecuteAction(selectedEnemy);
        ExitSelectionMode();
    }

    // 攻撃手段ごとのアクション
    private void ExecuteAction(BattleEnemyData selectedEnemy)
    {
        switch (currentActionType)
        {
            case SelectActionType.Attack:
                BattleManager.Instance.DecideOrder(selectedEnemy, currentAttack);
                break;

            case SelectActionType.Magic:
                BattleManager.Instance.DecideOrder(selectedEnemy, currentMagic, false);
                break;

            case SelectActionType.Item:
                BattleManager.Instance.DecideOrder(selectedEnemy, currentItem);
                break;

            default:
                break;
        }
    }

    // 選択モードを抜ける
    private void ExitSelectionMode()
    {
        isSelectingEnemy = false;
        currentEnemyIndex = -1;

        if (selectPanel != null)
        {
            selectPanel.SetActive(false);
        }

        ResetSelectionState();
        RefreshEnemyList();
        InputDelayManager.Lock(0.15f);
    }

    // リセット
    private void ResetSelectionState()
    {
        currentActionType = SelectActionType.None;
        currentAttack = 0;
        currentMagic = null;
        currentItem = null;
    }

    // 敵の選択欄を更新
    public void RefreshEnemyList()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        if (BattleManager.Instance == null)
        {
            return;
        }

        foreach (BattleEnemyData enemy in BattleManager.Instance.battleEnemyDataList)
        {
            GameObject obj = Instantiate(enemySelectPrefab, contentParent);
            EnemySelectUI enemyUI = obj.GetComponent<EnemySelectUI>();

            if (enemyUI != null)
            {
                enemyUI.Setup(enemy);
            }
        }
    }

    // セレクターの更新
    private void UpdateSelection()
    {
        EnemySelectUI[] enemySelectUIs = contentParent.GetComponentsInChildren<EnemySelectUI>();

        for (int i = 0; i < enemySelectUIs.Length; i++)
        {
            enemySelectUIs[i].SetSelected(i == currentEnemyIndex);
        }
    }

    // セレクター上移動
    private void MoveSelectionUp()
    {
        int enemyCount = BattleManager.Instance.battleEnemyDataList.Count;

        if (enemyCount <= 0)
        {
            return;
        }

        currentEnemyIndex--;

        if (currentEnemyIndex < 0)
        {
            currentEnemyIndex = enemyCount - 1;
        }

        UpdateSelection();
    }

    // セレクター下移動
    private void MoveSelectionDown()
    {
        int enemyCount = BattleManager.Instance.battleEnemyDataList.Count;

        if (enemyCount <= 0)
        {
            return;
        }

        currentEnemyIndex++;

        if (currentEnemyIndex >= enemyCount)
        {
            currentEnemyIndex = 0;
        }

        UpdateSelection();
    }
}