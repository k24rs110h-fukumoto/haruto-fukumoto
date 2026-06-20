using TMPro;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager Instance;

    [SerializeField] private GameObject gameMenuPanel;
    [SerializeField] private GameObject statusPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject magicPanel;
    [SerializeField] private GameObject savePanel;

    [SerializeField] private StatusPanelController statusPanelController;
    [SerializeField] private InventoryPanelController inventoryPanelController;
    [SerializeField] private MagicPanelController magicPanelController;
    [SerializeField] private SavePanelController savePanelController;

    [SerializeField] private TextMeshProUGUI[] menuTexts;

    private GameStateManager.GameState returnState;
    private bool isMenuOpen;
    private int currentMenuIndex;
    public bool isInMenuOpen;

    private void Awake()
    {
        Instance = this;
        AutoFindUI();
    }

    private void Start()
    {
        CloseAllUI();
    }

    private void Update()
    {
        if (InputDelayManager.IsLocked())
        {
            return;
        }

        if (GameStateManager.GetState() != GameStateManager.GameState.Field &&
            GameStateManager.GetState() != GameStateManager.GameState.Menu)
        {
            return;
        }

        if (isMenuOpen && !isInMenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                CloseMenu();
                return;
            }
        }
        else if (!isMenuOpen && !isInMenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OpenMenu();
                return;
            }

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
        gameMenuPanel = transform.Find("GameMenuPanel").gameObject;

        statusPanel = transform.Find("GameMenuPanel/StatusPanel").gameObject;
        inventoryPanel = transform.Find("GameMenuPanel/InventoryPanel").gameObject;
        magicPanel = transform.Find("GameMenuPanel/MagicPanel").gameObject;
        savePanel = transform.Find("GameMenuPanel/SavePanel").gameObject;

        Transform menuBar = transform.Find("GameMenuPanel/MenuBar");
        menuTexts = menuBar.GetComponentsInChildren<TextMeshProUGUI>(true);

        statusPanelController = statusPanel.GetComponent<StatusPanelController>();
        inventoryPanelController = inventoryPanel.GetComponent<InventoryPanelController>();
        magicPanelController = magicPanel.GetComponent<MagicPanelController>();
        savePanelController = savePanel.GetComponent<SavePanelController>();
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
                statusPanel.SetActive(true);
                statusPanelController.RefreshStatus();
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
                savePanel.SetActive(true);

                if (savePanelController != null)
                {
                    savePanelController.RefreshSavePanel();
                }

                break;

            case 4:
                HideAllPanels();
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

    private void OpenMenu()
    {
        gameMenuPanel.SetActive(true);
        isMenuOpen = true;
        isInMenuOpen = false;
        currentMenuIndex = 0;

        returnState = GameStateManager.GetState();
        GameStateManager.SetState(GameStateManager.GameState.Menu);

        UpdateMenuDisplay();
    }

    private void CloseMenu()
    {
        gameMenuPanel.SetActive(false);
        isMenuOpen = false;
        isInMenuOpen = false;

        GameStateManager.SetState(returnState);
        HideAllPanels();
    }

    private void SelectMenu()
    {
        if (!isMenuOpen)
        {
            return;
        }

        isInMenuOpen = true;

        switch (currentMenuIndex)
        {
            case 0:
                isInMenuOpen = false;
                break;

            case 1:
                inventoryPanelController.EnterSelectionMode();
                break;

            case 2:
                magicPanelController.EnterSelectionMode();
                break;

            case 3:
                if (savePanelController != null)
                {
                    savePanelController.EnterSelectionMode();
                }
                break;

            case 4:
                SceneTransitionManager.LoadScene("TitleScene", "");
                break;
        }
    }

    private void BackToMenuBar()
    {
        isInMenuOpen = false;
    }

    private void HideAllPanels()
    {
        statusPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        magicPanel.SetActive(false);
        savePanel.SetActive(false);
    }

    private void CloseAllUI()
    {
        gameMenuPanel.SetActive(false);
        HideAllPanels();
    }

    public void ReturnToMenuBar()
    {
        isInMenuOpen = false;
        UpdateMenuDisplay();
    }

    public void CloseGameMenu()
    {
        CloseMenu();
    }
}