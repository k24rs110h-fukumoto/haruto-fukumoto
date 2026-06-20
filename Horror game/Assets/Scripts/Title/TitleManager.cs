using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string map1SceneName = "Map1";

    [Header("Panels")]
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject optionPanel;

    private void Start()
    {
        GameStateManager.SetState(GameStateManager.GameState.Title);
        CloseAllPanels();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayTitleBGM();
        }
    }

    public void OpenPlayPanel()
    {
        OpenPanel(playPanel);
    }

    public void OpenHowToPlayPanel()
    {
        OpenPanel(howToPlayPanel);
    }

    public void OpenOptionPanel()
    {
        OpenPanel(optionPanel);
    }

    public void CloseAllPanels()
    {
        if (playPanel != null)
        {
            playPanel.SetActive(false);
        }

        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }

        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
        }

        if (titlePanel != null)
        {
            titlePanel.SetActive(true);
        }
    }

    private void OpenPanel(GameObject targetPanel)
    {
        CloseAllPanels();

        if (titlePanel != null)
        {
            titlePanel.SetActive(false);
        }

        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
        }
    }

    public void StartMap1()
    {
        GameStateManager.SetState(GameStateManager.GameState.Playing);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayStageBGM();
        }

        SceneManager.LoadScene(map1SceneName);
    }

    public void ComingSoon()
    {
        Debug.Log("Coming Soon");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}