using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private string characterMakeSceneName = "CharacterMakeScene";
    //[SerializeField] private int currentSlot = 1;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button debugButton;
    [SerializeField] private TitleDeleteSavePanelController deleteSavePanelController;
    [SerializeField] private TitleLoadSavePanelController loadSavePanelController;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(BGMType.Title);

        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(NewGame);
        }

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(Continue);
        }

        if (optionButton != null)
        {
            optionButton.onClick.AddListener(OpenDeleteSavePanel);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(Quit);
        }

        if (debugButton != null)
        {
            debugButton.onClick.AddListener(DebugMode);
        }

        UpdateContinueButton();
    }

    private void NewGame()
    {
        SceneTransitionManager.LoadScene(characterMakeSceneName, "");
    }

    private void Continue()
    {
        if (loadSavePanelController == null)
        {
            Debug.LogError("LoadSavePanelController がありません。");
            return;
        }

        loadSavePanelController.OpenPanel();
    }

    private void UpdateContinueButton()
    {
        if (continueButton == null)
        {
            return;
        }

        if (SaveManager.Instance == null)
        {
            continueButton.interactable = false;
            return;
        }

        bool hasAnySave =
            SaveManager.Instance.HasSaveData(1) ||
            SaveManager.Instance.HasSaveData(2) ||
            SaveManager.Instance.HasSaveData(3);

        continueButton.interactable = hasAnySave;
    }

    private void OpenDeleteSavePanel()
    {
        if (deleteSavePanelController == null)
        {
            Debug.LogError("DeleteSavePanelController がありません。");
            return;
        }

        deleteSavePanelController.OpenPanel();
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void DebugMode()
    {
        Debug.Log("DebugMode");
    }
}