using UnityEngine;
using TMPro;

public class BattleMenuManager : MonoBehaviour
{
    public static BattleMenuManager Instance;

    [SerializeField] private GameObject battleMenuPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject magicPanel;

    [SerializeField] private EnemySelectPanelController enemySelectPanelController;
    [SerializeField] private InventoryPanelController inventoryPanelController;
    [SerializeField] private MagicPanelController magicPanelController;
    [SerializeField] private TextMeshProUGUI[] menuTexts;
    private GameStateManager.GameState returnState;
    private int currentMenuIndex;
    public bool isInMenuOpen;
    public float inputLockTimer;

    private void Awake()
    {
        Instance = this;
        AutoFindUI();
    }

    private void Start()
    {
        OpenBattleMenu();
    }

    private void Update()
    {
        if (InputDelayManager.IsLocked())
        {
            return;
        }
        if (GameStateManager.GetState() != GameStateManager.GameState.Battle)
        {
            return;
        }

        if (isInMenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
            {
                BackToMenuBar();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                MoveMenuUp();
                UpdateMenuDisplay();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                MoveMenuDown();
                UpdateMenuDisplay();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                SelectMenu();
            }
        }
    }

    private void AutoFindUI()
    {
        battleMenuPanel = transform.Find("BattleMenuPanel").gameObject;

        inventoryPanel = transform.Find("BattleMenuPanel/InventoryPanel").gameObject;
        magicPanel = transform.Find("BattleMenuPanel/MagicPanel").gameObject;

        enemySelectPanelController = transform.Find("BattleMenuPanel/EnemySelectPanel").GetComponent<EnemySelectPanelController>();

        Transform menuBar = transform.Find("BattleMenuPanel/MenuBar");
        menuTexts = menuBar.GetComponentsInChildren<TextMeshProUGUI>(true);

        inventoryPanelController = inventoryPanel.GetComponent<InventoryPanelController>();
        magicPanelController = magicPanel.GetComponent<MagicPanelController>();
    }

    private void MoveMenuUp()
    {
        if (currentMenuIndex == 0)
        {
            currentMenuIndex = menuTexts.Length - 1;
        }
        else
        {
            currentMenuIndex--;
        }
    }

    private void MoveMenuDown()
    {
        if (currentMenuIndex == menuTexts.Length - 1)
        {
            currentMenuIndex = 0;
        }
        else
        {
            currentMenuIndex++;
        }
    }

    private void UpdateMenuDisplay()
    {
        HideAllPanels();
        UpdateMenuTextDisplay();

        switch (currentMenuIndex)
        {
            case 0:

                break;
            case 1:
                inventoryPanel.SetActive(true);
                inventoryPanelController.RefreshInventory();
                break;
            case 2:
                magicPanel.SetActive(true);
                magicPanelController.RefreshMagic();
                break;
            case 3:
                break;
        }
    }

    private void UpdateMenuTextDisplay()
    {
        for (int i = 0; i < menuTexts.Length; i++)
        {
            menuTexts[i].text = menuTexts[i].text.Replace("＞ ", "");
        }

        menuTexts[currentMenuIndex].text = "＞ " + menuTexts[currentMenuIndex].text;
    }

    private void SelectMenu()
    {
        isInMenuOpen = true;
        switch (currentMenuIndex)
        {
            case 0:
                enemySelectPanelController.EnterSelectionMode(BattleManager.Instance.playerBattleData.attack);
                break;
            case 1:
                inventoryPanelController.EnterSelectionMode();
                break;
            case 2:
                magicPanelController.EnterSelectionMode();
                break;
            case 3:
                BattleManager.Instance.TryEscape();
                break;
        }
    }

    private void BackToMenuBar()
    {
        isInMenuOpen = false;
        InputDelayManager.Lock(0.15f);
    }

    public void OpenBattleMenu()
    {
        battleMenuPanel.SetActive(true);
        isInMenuOpen = false;
        currentMenuIndex = 0;

        HideAllPanels();
        UpdateMenuDisplay();

        InputDelayManager.Lock(0.15f);
    }

    private void HideAllPanels()
    {
        inventoryPanel.SetActive(false);
        magicPanel.SetActive(false);
    }

    private void CloseAllUI()
    {
        battleMenuPanel.SetActive(false);
        HideAllPanels();
    }

    public void ReturnToCommandMenu()
    {
        if (GameStateManager.GetState() != GameStateManager.GameState.Battle)
        {
            return;
        }

        isInMenuOpen = false;
        HideAllPanels();
        UpdateMenuDisplay();
        InputDelayManager.Lock(0.15f);
    }
}