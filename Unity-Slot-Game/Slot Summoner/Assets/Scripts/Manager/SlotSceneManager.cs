using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlotSceneManager : MonoBehaviour
{
    public GameObject descriptionPanel;
    public Button descriptionCloseButton;
    public Button goBattleButton;

    void Start()
    {
        descriptionPanel.SetActive(true);
        goBattleButton.gameObject.SetActive(false);

        descriptionCloseButton.onClick.AddListener(CloseDescription);
        goBattleButton.onClick.AddListener(GoBattle);
    }

    void CloseDescription()
    {
        descriptionPanel.SetActive(false);
    }

    public void ShowGoBattleButton()
    {
        goBattleButton.gameObject.SetActive(true);
    }

    void GoBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }
}